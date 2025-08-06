using System.Threading;
using Application.Services.UserData;
using Core.StateMachine;
using Application.UI;
using Cysharp.Threading.Tasks;
using Runtime.Application.ApplicationStates.Game.Controllers;
using ILogger = Core.ILogger;
using Core.UI;

namespace Application.Game
{
    public class MenuStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly StartSettingsController _startSettingsController;
        private readonly IUserInventoryService _userInventoryService;

        private MenuScreen _screen;

        public MenuStateController(ILogger logger, IUiService uiService,
            StartSettingsController startSettingsController, IUserInventoryService userInventoryService) : base(logger)
        {
            _uiService = uiService;
            _startSettingsController = startSettingsController;
            _userInventoryService = userInventoryService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.MenuScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<MenuScreen>(ConstScreens.MenuScreen);
            _screen.Initialize(ShowSettingsButtonPopup,
                GoToLeaderboard, GoToMainMenu,
                GoToAccount, OpenHelpPopup,
                GoToLevelSelection,
                GoToShop,
                OpenFreeSpin
                );

            _screen.ShowAsync().Forget();
            _screen.SetBalance(_userInventoryService.GetBalance());
        }

        private void ShowSettingsButtonPopup() => _startSettingsController.Run(default).Forget();
        private async void GoToLeaderboard() => await GoTo<LeaderboardStateController>();
        private async void GoToAccount() => await GoTo<AccountStateController>();
        private async void GoToMainMenu() => await GoTo<MenuStateController>();
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
        private async void GoToLevelSelection() => await GoTo<LevelSelectionStateController>();
        private async void GoToShop() => await GoTo<InitShopState>();
        private async void OpenFreeSpin()
        {
            var popup = await _uiService.ShowPopup(ConstPopups.LuckyWheelPopup) as LuckyWheelPopup;
            popup.OnEndSpin += async () => await GoTo<MenuStateController>();
        }
    }
}