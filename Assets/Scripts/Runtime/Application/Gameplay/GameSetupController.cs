using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.UI;
using Core;
using Cysharp.Threading.Tasks;
using Runtime.Application.Gameplay;

public class GameSetupController
{
    private readonly GameData _gameData;
    private readonly ISettingProvider _settingProvider;
    private readonly MemoryCardFactory _cardsFactory;
    private readonly MemoryCardsAnimationContoller _cardsAnimationContoller;
    private readonly MemoryPairProgressDisplayFactory _pairProgressDisplayFactory;
    private readonly GameplayTimer _gameplayTimer;

    private List<MemoryCard> _memoryCards;
    private List<MemoryPairSolveProgressDisplay> _progressDisplayList;
    
    public List<MemoryCard> MemoryCards => _memoryCards;
    public List<MemoryPairSolveProgressDisplay> ProgressDisplayList => _progressDisplayList;
    
    public GameSetupController(GameData gameData, MemoryCardFactory cardsFactory, 
        ISettingProvider settingProvider, MemoryCardsAnimationContoller cardsAnimationContoller, 
        MemoryPairProgressDisplayFactory pairProgressDisplayFactory, GameplayTimer gameplayTimer)
    {
        _gameData = gameData;
        _cardsFactory = cardsFactory;
        _settingProvider = settingProvider;
        _cardsAnimationContoller = cardsAnimationContoller;
        _pairProgressDisplayFactory = pairProgressDisplayFactory;
        _gameplayTimer = gameplayTimer;
    }

    public async UniTask SetupGame(GameplayScreen screen, CancellationToken token)
    {
        int level = _gameData.LevelID;
        var levelConfig = _settingProvider.Get<GameConfig>().LevelConfigs[level];

        _memoryCards = _cardsFactory.CreateMemoryCards(levelConfig);
        screen.ParentMemoryCards(_memoryCards);
        
        _progressDisplayList = _pairProgressDisplayFactory.CreateMemoryPairSolveProgressDisplayList(_memoryCards);
        screen.ParentProgressDisplay(_progressDisplayList);
        
        SetGameData(levelConfig);
        await PlayRevelAnimation(levelConfig, token);
        _gameplayTimer.RunTimer(levelConfig.TimeTotal, token).Forget();
    }

    private async Task PlayRevelAnimation(LevelConfig levelConfig, CancellationToken token)
    {
        await _cardsAnimationContoller.ShowAllMemoryCards(_memoryCards, token);
        await UniTask.WaitForSeconds(levelConfig.RememberedTime, cancellationToken: token);
        await _cardsAnimationContoller.HideAllMemoryCards(_memoryCards, token);
    }

    private void SetGameData(LevelConfig levelConfig)
    {
        _gameData.PairsTarget = levelConfig.CardPairs;
        _gameData.PairsSolved = 0;
        _gameData.GameEnded = false;
        _gameData.TimeLeft = levelConfig.TimeTotal;
    }
}
