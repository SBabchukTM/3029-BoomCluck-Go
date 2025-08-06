using System.Collections.Generic;
using System.Threading;
using Application.Game;
using Application.UI;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Application.Services.Shop;
using Runtime.Application.ShopSystem;

namespace Runtime.Application.ApplicationStates.Game.Controllers
{
    public class InitShopState : StateController
    {
        private readonly IShopItemsStorage _shopItemsStorage;
        private readonly IUiService _uiService;
        private readonly IShopItemsDisplayService _shopItemsDisplayService;
        private readonly ISettingProvider _settingProvider;
        private readonly ShopService _shopService;
        private readonly ShopItemDisplayController _shopItemDisplayController;
        private readonly List<ISetShopSetup> _setShopConfigs;
        private readonly StartSettingsController _startSettingsController;

        public InitShopState(ILogger logger, IShopItemsStorage shopItemsStorage, IUiService uiService,
                IShopItemsDisplayService shopItemsDisplayService, ISettingProvider settingProvider,
                IProcessPurchaseService processPurchaseService, IPurchaseEffectsService purchaseEffectsService,
                ISelectPurchaseItemService selectPurchaseItemService, ShopService shopService,
                ShopItemDisplayController shopItemDisplayController, StartSettingsController startSettingsController) : base(logger)
        {
            _shopItemsStorage = shopItemsStorage;
            _uiService = uiService;
            _shopItemsDisplayService = shopItemsDisplayService;
            _settingProvider = settingProvider;
            _shopService = shopService;
            _shopItemDisplayController = shopItemDisplayController;
            _startSettingsController = startSettingsController;

            _setShopConfigs = new()
            {
                processPurchaseService,
                purchaseEffectsService,
                selectPurchaseItemService,
                shopItemsDisplayService
            };
        }

        public override UniTask Enter(CancellationToken cancellationToken = default)
        {
            SetShopConfig();
            var screen = CreateScreen();

            return GoTo<ShopStateController>(cancellationToken);
        }

        private ShopScreen CreateScreen()
        {
            var screen = _uiService.GetScreen<ShopScreen>(ConstScreens.ShopScreen);
            screen.ShowAsync().Forget();
            _shopItemDisplayController.SetShop(_shopService);
            _shopItemsDisplayService.CreateShopItems();
            _shopItemsDisplayService.UpdateItemsStatus();
            screen.SetShopItems(_shopItemsStorage.GetItemDisplay());
            screen.SubscribeCallbacks(ShowSettingsButtonPopup,
                GoToLeaderboard,
                GoToMainMenu,
                GoToAccount,
                OpenHelpPopup
                );

            return screen;
        }

        private void SetShopConfig()
        {
            var shopConfig = _settingProvider.Get<ShopSetup>();

            _shopItemsStorage.SetShopStateConfigs(shopConfig.ShopItemStateConfigs);

            foreach (var setShopConfig in _setShopConfigs)
                setShopConfig.SetShopSetup(shopConfig);
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
            popup.OnRulePress += () =>
            {
                _uiService.ShowPopup(ConstPopups.TermsPopup);
                popup.Hide();
            };

        }
    }
}