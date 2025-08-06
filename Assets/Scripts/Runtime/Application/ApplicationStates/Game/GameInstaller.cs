using Application.Services.AccountData;
using Runtime.Application.ApplicationStates.Game.Controllers;
using Runtime.Application.Gameplay;
using Runtime.Application.Services.Shop;
using Runtime.Application.ShopSystem;
using UnityEngine;
using Zenject;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            BindController();
            BindServices();
            BindData();
        }

        private void BindData()
        {
            Container.Bind<IShopItemsStorage>().To<ShopItemsStorage>().AsSingle();

            Container.Bind<ShopItemDisplayModel>().AsTransient();
        }

        private void BindServices()
        {
            Container.Bind<IProcessPurchaseService>().To<ProcessPurchaseService>().AsSingle();
            Container.Bind<ISelectPurchaseItemService>().To<SelectPurchaseItemService>().AsSingle();
            Container.Bind<IPurchaseEffectsService>().To<PurchaseEffectsService>().AsSingle();
            Container.Bind<IShopItemsDisplayService>().To<ShopItemsDisplayService>().AsSingle();
            Container.Bind<UserAccountService>().AsSingle();
            Container.Bind<AvatarSelectionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<RecordsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MemoryCardFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MemoryPairProgressDisplayFactory>().AsSingle();
        }

        private void BindController()
        {
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<InitShopState>().AsSingle();
            Container.Bind<ShopStateController>().AsSingle();
            Container.Bind<StartSettingsController>().AsSingle();
            Container.Bind<ShopItemDisplayController>().AsSingle();
            Container.Bind<LevelSelectionStateController>().AsSingle();
            Container.Bind<UserProgressService>().AsSingle();
            Container.Bind<GameData>().AsSingle();
            Container.Bind<AccountStateController>().AsSingle();
            Container.Bind<GameplayStateController>().AsSingle();
            Container.Bind<LeaderboardStateController>().AsSingle();
            Container.Bind<GameSetupController>().AsSingle();
            Container.Bind<GridLayoutSetupController>().AsSingle();
            Container.Bind<MemoryCardsAnimationContoller>().AsSingle();
            Container.Bind<GameplayTimer>().AsSingle();
            Container.Bind<PausePopupStateController>().AsSingle();
            Container.Bind<WinPopupStateController>().AsSingle();
            Container.Bind<LosePopupStateController>().AsSingle();
        }
    }
}