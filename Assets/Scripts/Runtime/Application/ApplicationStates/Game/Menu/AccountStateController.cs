using System;
using System.Threading;
using Application.Services.AccountData;
using Application.Services.UserData;
using Application.UI;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Application.ApplicationStates.Game.Controllers;
using UnityEngine;
using ILogger = Core.ILogger;

namespace Application.Game
{
    public class AccountStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly StartSettingsController _startSettingsController;
        private readonly UserAccountService _userAccountService;
        private readonly AvatarSelectionService _avatarSelectionService;
        private readonly IUserInventoryService _userInventoryService;
        private AccountScreen _screen;
        private UserAccountData _modifiedData;

        public AccountStateController(ILogger logger, IUiService uiService, StartSettingsController startSettingsController,
            UserAccountService userAccountService, AvatarSelectionService avatarSelectionService, IUserInventoryService userInventoryService) : base(logger)
        {
            _uiService = uiService;
            _startSettingsController = startSettingsController;
            _userAccountService = userAccountService;
            _avatarSelectionService = avatarSelectionService;
            _userInventoryService = userInventoryService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CopyAccount();
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            SaveAccount();
            await _uiService.HideScreen(ConstScreens.AccountScreen);
        }

        private void CopyAccount()
        {
            _modifiedData = _userAccountService.GetAccountDataCopy();
        }

        private void SaveAccount()
        {
            _userAccountService.SaveAccountData(_modifiedData);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<AccountScreen>(ConstScreens.AccountScreen);
            _screen.Initialize(ShowSettingsButtonPopup, 
                GoToLeaderboard, GoToMainMenu, 
                GoToAccount, OpenHelpPopup, GoToShop);
            _screen.ShowAsync().Forget();
            
            _screen.SetName(_modifiedData.Username);
            _screen.SetAvatar(_userAccountService.GetUsedAvatarSprite());
            _screen.SetBalance(_userInventoryService.GetBalance());
        }

        private void SubscribeToEvents()
        {
            _screen.OnNameChanged += ValidateName;
            _screen.OnAvatarSelectPressed += SelectNewAvatar;
        }

        private async void SelectNewAvatar()
        {
            Sprite newAvatar = await _avatarSelectionService.PickImage(512, CancellationToken.None);

            if (newAvatar != null)
            {
                _screen.SetAvatar(newAvatar);
                _modifiedData.AvatarBase64 = _userAccountService.ConvertToBase64(newAvatar);
            }
        }

        private void ValidateName(string value)
        {
            if (value.Length is < 2 or > 12)
            {
                _screen.SetName(_modifiedData.Username);
                return;
            }

            if (Char.IsDigit(value[0]))
            {
                _screen.SetName(_modifiedData.Username);
                return;
            }
            
            _modifiedData.Username = value;
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