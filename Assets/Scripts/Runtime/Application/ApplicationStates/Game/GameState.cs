using System.Threading;
using Cysharp.Threading.Tasks;
using Application.Game;
using Core.StateMachine;
using Runtime.Application.ApplicationStates.Game.Controllers;
using ILogger = Core.ILogger;

namespace Application.GameStateMachine
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly MenuStateController _menuStateController;
        private readonly ShopStateController _shopStateController;
        private readonly LevelSelectionStateController _levelSelectionController;
        private readonly UserDataStateChangeController _userDataStateChangeController;
        private readonly InitShopState _initShopState;
        private readonly GameplayStateController _gameplayStateController;
        private readonly AccountStateController _accountStateController;
        private readonly LeaderboardStateController _leaderboardStateController;
        private readonly PausePopupStateController _pausePopupStateController;
        private readonly WinPopupStateController _winPopupStateController;
        private readonly LosePopupStateController _losePopupStateController;

        public GameState(ILogger logger,
            MenuStateController menuStateController,
            ShopStateController shopStateController,
            LevelSelectionStateController levelSelectionController,
            StateMachine stateMachine,
            UserDataStateChangeController userDataStateChangeController,
            InitShopState initShopState,
            GameplayStateController gameplayStateController,
            AccountStateController accountStateController,
            LeaderboardStateController leaderboardStateController,
            PausePopupStateController pausePopupStateController,
            WinPopupStateController winPopupStateController,
            LosePopupStateController losePopupStateController) : base(logger)
        {
            _stateMachine = stateMachine;
            _menuStateController = menuStateController;
            _shopStateController = shopStateController;
            _levelSelectionController = levelSelectionController;
            _userDataStateChangeController = userDataStateChangeController;
            _initShopState = initShopState;
            _gameplayStateController = gameplayStateController;
            _accountStateController = accountStateController;
            _leaderboardStateController = leaderboardStateController;
            _pausePopupStateController = pausePopupStateController;
            _winPopupStateController = winPopupStateController;
            _losePopupStateController = losePopupStateController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await _userDataStateChangeController.Run(default);

            _stateMachine.Initialize(_menuStateController, _shopStateController, _initShopState, 
                _levelSelectionController, _gameplayStateController, _accountStateController, _leaderboardStateController,
                _pausePopupStateController, _winPopupStateController, _losePopupStateController);
            
            _stateMachine.GoTo<MenuStateController>().Forget();
        }
    }
}