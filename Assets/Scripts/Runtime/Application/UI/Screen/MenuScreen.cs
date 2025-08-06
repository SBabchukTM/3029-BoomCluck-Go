using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _accountButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TextMeshProUGUI _balanceText;

        public void Initialize(Action settingCallback, Action leaderboardCallback,
            Action homeCallback, Action accountCallback, Action helpCallback, Action playCallback,
            Action shopCallback, Action spinCallback)
        {
            _settingsButton.onClick.AddListener(settingCallback.Invoke);
            _leaderboardButton.onClick.AddListener(leaderboardCallback.Invoke);
            _homeButton.onClick.AddListener(homeCallback.Invoke);
            _accountButton.onClick.AddListener(accountCallback.Invoke);
            _helpButton.onClick.AddListener(helpCallback.Invoke);
            _playButton.onClick.AddListener(playCallback.Invoke);
            _shopButton.onClick.AddListener(shopCallback.Invoke);
            _spinButton.onClick.AddListener(spinCallback.Invoke);
        }

        public void SetBalance(int balance) => _balanceText.text = balance.ToString();
    }
}