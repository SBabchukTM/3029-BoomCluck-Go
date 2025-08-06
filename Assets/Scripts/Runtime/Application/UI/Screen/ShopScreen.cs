using System;
using System.Collections.Generic;
using Application.Services.UserData;
using Runtime.Application.ShopSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.UI
{
    public class ShopScreen : UiScreen
    {
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private RectTransform _shopItemsParent;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _accountButton;
        [SerializeField] private Button _helpButton;

        private IUserInventoryService _userInventoryService;
        
        [Inject]
        public void Construct(IUserInventoryService userInventoryService) =>
                _userInventoryService = userInventoryService;

        public void SubscribeCallbacks(Action settingCallback, Action leaderboardCallback,
            Action homeCallback, Action accountCallback, Action helpCallback)
        {
            _settingsButton.onClick.AddListener(settingCallback.Invoke);
            _leaderboardButton.onClick.AddListener(leaderboardCallback.Invoke);
            _homeButton.onClick.AddListener(homeCallback.Invoke);
            _accountButton.onClick.AddListener(accountCallback.Invoke);
            _helpButton.onClick.AddListener(helpCallback.Invoke);
        }

        private void Start()
        {
            SubscribeToEvents();
            UpdateBalance(_userInventoryService.GetBalance());
        }

        private void OnDestroy() =>
                UnSubscribe();

        public void SetShopItems(List<ShopItemDisplayView> items)
        {
            foreach (var item in items)
                item.transform.SetParent(_shopItemsParent, false);
        }

        private void UpdateBalance(int balance) => 
                _balanceText.text = balance.ToString();

        private void SubscribeToEvents()
        {
            _userInventoryService.BalanceChangedEvent += UpdateBalance;
        }

        private void UnSubscribe()
        {
            _userInventoryService.BalanceChangedEvent -= UpdateBalance;
        }
    }
}