using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class LeaderboardScreen : UiScreen
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _accountButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private TextMeshProUGUI _balanceText;
        
        public void Initialize(Action settingCallback, Action leaderboardCallback,
            Action homeCallback, Action accountCallback, Action helpCallback, Action shopCallback)
        {
            _settingsButton.onClick.AddListener(settingCallback.Invoke);
            _leaderboardButton.onClick.AddListener(leaderboardCallback.Invoke);
            _homeButton.onClick.AddListener(homeCallback.Invoke);
            _accountButton.onClick.AddListener(accountCallback.Invoke);
            _helpButton.onClick.AddListener(helpCallback.Invoke);
            _shopButton.onClick.AddListener(shopCallback.Invoke);
        }

        public void SetBalance(int balance) => _balanceText.text = balance.ToString();
        
        public void SetRecords(List<RecordDisplay> records)
        {
            foreach (var record in records)
            {
                record.transform.SetParent(_parent, false);
            }
        }
    }
}