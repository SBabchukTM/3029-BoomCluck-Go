using System.Threading;
using Application.Services.UserData;
using Core.StateMachine;
using Application.UI;
using Core;
using Cysharp.Threading.Tasks;
using Runtime.Application.ApplicationStates.Game.Controllers;
using UnityEngine;
using ILogger = Core.ILogger;

namespace Application.Game
{
    public class LevelSelectionStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly IUserInventoryService _userInventoryService;
        private readonly ISettingProvider _settingProvider;
        private readonly GameData _gameData;
        private readonly StartSettingsController _startSettingsController;

        private LevelSelectionScreen _screen;

        public LevelSelectionStateController(ILogger logger, IUiService uiService,
            IUserInventoryService userInventoryService, ISettingProvider settingProvider,
            GameData gameData, StartSettingsController startSettingsController) : base(logger)
        {
            _uiService = uiService;
            _userInventoryService = userInventoryService;
            _settingProvider = settingProvider;
            _gameData = gameData;
            _startSettingsController = startSettingsController;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.LevelSelectionScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LevelSelectionScreen>(ConstScreens.LevelSelectionScreen);

            int purchasedCardsCount = _userInventoryService.GetPurchasedGameItemsIDs().Count;
            var levels = _settingProvider.Get<GameConfig>().LevelConfigs;

            _screen.Initialize(purchasedCardsCount, levels);
            _screen.ShowAsync().Forget();
            _screen.SubscribeCallbacks(ShowSettingsButtonPopup,
                GoToLeaderboard, GoToMainMenu,
                GoToAccount, OpenHelpPopup, GoToShop);
            _screen.SetBalance(_userInventoryService.GetBalance());
        }

        private void SubscribeToEvents()
        {
            _screen.OnLevelSelected += async (level) =>
            {
                _gameData.LevelID = level;
                await GoTo<GameplayStateController>();
            };
        }

        private void ShowSettingsButtonPopup() => _startSettingsController.Run(default).Forget();
        private async void GoToLeaderboard() => await GoTo<LeaderboardStateController>();
        private async void GoToAccount() => await GoTo<AccountStateController>();
        private async void GoToMainMenu() => await GoTo<MenuStateController>();
        private async void GoToShop() => await GoTo<InitShopState>();
        private async void OpenHelpPopup()
        {
            var popup = await _uiService.ShowPopup(ConstPopups.InfoPopup) as InfoPopup;
            popup.OnPrivacyPolicyPress += () =>
            {
                _uiService.ShowPopup(ConstPopups.PrivacyPolicyPopup);
                popup.Hide();
            };
            popup.OnRulePress += () =>
            {
                _uiService.ShowPopup(ConstPopups.RulesPopup);
                popup.Hide();
            };
            popup.OnTermsPress += () =>
            {
                _uiService.ShowPopup(ConstPopups.TermsPopup);
                popup.Hide();
            };

        }
    }
}