using System.Threading;
using Application.Services.UserData;
using Application.UI;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Application.ApplicationStates.Game.Controllers;
using ILogger = Core.ILogger;

namespace Application.Game
{
    public class LeaderboardStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly StartSettingsController _startSettingsController;
        private readonly RecordsFactory _recordsFactory;
        private readonly IUserInventoryService _userInventoryService;

        private LeaderboardScreen _screen;

        public LeaderboardStateController(ILogger logger, IUiService uiService,
            StartSettingsController startSettingsController, RecordsFactory recordsFactory,
            IUserInventoryService userInventoryService) : base(logger)
        {
            _uiService = uiService;
            _startSettingsController = startSettingsController;
            _recordsFactory = recordsFactory;
            _userInventoryService = userInventoryService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.LeaderboardScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LeaderboardScreen>(ConstScreens.LeaderboardScreen);
            _screen.Initialize(ShowSettingsButtonPopup,
                GoToLeaderboard, GoToMainMenu,
                GoToAccount, OpenHelpPopup, GoToShop);
            _screen.ShowAsync().Forget();
            _screen.SetRecords(_recordsFactory.CreateRecordDisplayList());
            _screen.SetBalance(_userInventoryService.GetBalance());
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
            popup.OnRulePress += () =>
            {
                _uiService.ShowPopup(ConstPopups.TermsPopup);
                popup.Hide();
            };

        }
    }
}