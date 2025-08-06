using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Audio;
using Application.UI;
using Core.Services.Audio;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Application.Gameplay;
using ILogger = Core.ILogger;

namespace Application.Game
{
    public class GameplayStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly GameSetupController _gameSetupController;
        private readonly MemoryCardsAnimationContoller _cardsAnimationContoller;
        private readonly GameData _gameData;
        private readonly PausePopupStateController _pausePopupStateController;
        private readonly WinPopupStateController _winPopupStateController;
        private readonly LosePopupStateController _losePopupStateController;
        private readonly UserProgressService _userProgressService;
        private readonly IAudioService _audioService;

        private GameplayScreen _screen;

        private CancellationTokenSource _cancellationTokenSource;
        private Queue<MemoryCard> _cardsQueue = new();

        public GameplayStateController(ILogger logger, IUiService uiService,
            GameSetupController gameSetupController, GameData gameData,
            MemoryCardsAnimationContoller memoryCardsAnimationContoller,
            PausePopupStateController pausePopupStateController,
            WinPopupStateController winPopupStateController,
            LosePopupStateController losePopupStateController,
            UserProgressService userProgressService, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _gameSetupController = gameSetupController;
            _gameData = gameData;
            _cardsAnimationContoller = memoryCardsAnimationContoller;
            _pausePopupStateController = pausePopupStateController;
            _winPopupStateController = winPopupStateController;
            _losePopupStateController = losePopupStateController;
            _userProgressService = userProgressService;
            _audioService = audioService;

            _gameData.OnPairSolved += ProcessPairSolve;
            _gameData.OnTimeChanged += ProcessTimeChange;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = new();
            CreateScreen();
            StartGame(_cancellationTokenSource.Token).Forget();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            await _uiService.HideScreen(ConstScreens.GameplayScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<GameplayScreen>(ConstScreens.GameplayScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
            _screen.OnPausePressed += OpenPausePopup;
        }

        private async UniTask StartGame(CancellationToken token)
        {
            _cardsQueue.Clear();

            await _gameSetupController.SetupGame(_screen, token);
            token.ThrowIfCancellationRequested();

            foreach (var card in _gameSetupController.MemoryCards)
                card.OnPressed += ProcessMemoryCardPress;
        }

        private async void ProcessMemoryCardPress(MemoryCard card)
        {
            _audioService.PlaySound(ConstAudio.CardSound);
            await _cardsAnimationContoller.ShowCard(card, _cancellationTokenSource.Token);

            _cardsQueue.Enqueue(card);

            if (_cardsQueue.Count >= 2)
                await ProcessMemoryCardPair();
        }

        private async UniTask ProcessMemoryCardPair()
        {
            var firstCard = _cardsQueue.Dequeue();
            var secondCard = _cardsQueue.Dequeue();

            if (firstCard.CardName != secondCard.CardName)
            {
                _audioService.PlaySound(ConstAudio.MismatchSound);
                try
                {
                    await HideBothCards(firstCard, secondCard);
                    return;
                }
                catch
                {
                }
            }

            ProcessSolvedPair(firstCard, secondCard);

            UpdateProgressDisplay(firstCard);
        }

        private async Task HideBothCards(MemoryCard firstCard, MemoryCard secondCard)
        {
            await UniTask.WaitForSeconds(0.25f, cancellationToken: _cancellationTokenSource.Token);
            _cardsAnimationContoller.HideCard(firstCard, _cancellationTokenSource.Token).Forget();
            _cardsAnimationContoller.HideCard(secondCard, _cancellationTokenSource.Token).Forget();
        }

        private void ProcessSolvedPair(MemoryCard firstCard, MemoryCard secondCard)
        {
            _audioService.PlaySound(ConstAudio.MatchSound);
            _gameData.PairsSolved++;
            firstCard.SetSolved();
            secondCard.SetSolved();
        }

        private void UpdateProgressDisplay(MemoryCard card)
        {
            foreach (var progressDisplay in _gameSetupController.ProgressDisplayList)
            {
                if (progressDisplay.CardSprite.name == card.CardName)
                {
                    progressDisplay.UpdateProgress();
                    return;
                }
            }
        }

        private void ProcessPairSolve(int pairs)
        {
            if (pairs >= _gameData.PairsTarget && !_gameData.GameEnded)
            {
                _audioService.PlaySound(ConstAudio.VictorySound);
                _gameData.GameEnded = true;
                _userProgressService.SaveProgress();
                _winPopupStateController.Enter().Forget();
            }
        }

        private void ProcessTimeChange(float time)
        {
            if (time <= 0 && !_gameData.GameEnded)
            {
                _audioService.PlaySound(ConstAudio.LoseSound);
                _gameData.GameEnded = true;
                _losePopupStateController.Enter().Forget();
            }
        }

        private void OpenPausePopup()
        {
            _pausePopupStateController.Enter().Forget();
        }
    }
}