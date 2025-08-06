using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class AccountScreen : UiScreen
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _accountButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _avatarSelectButton;
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TextMeshProUGUI _balanceText;
        
        public event Action<string> OnNameChanged;
        public event Action OnAvatarSelectPressed;
        
        public void Initialize(Action settingCallback, Action leaderboardCallback,
            Action homeCallback, Action accountCallback, Action helpCallback, Action shopCallback)
        {
            _settingsButton.onClick.AddListener(settingCallback.Invoke);
            _leaderboardButton.onClick.AddListener(leaderboardCallback.Invoke);
            _homeButton.onClick.AddListener(homeCallback.Invoke);
            _accountButton.onClick.AddListener(accountCallback.Invoke);
            _helpButton.onClick.AddListener(helpCallback.Invoke);
            _shopButton.onClick.AddListener(shopCallback.Invoke);
            
            _nameInputField.onEndEdit.AddListener((value) => OnNameChanged?.Invoke(value));
            _avatarSelectButton.onClick.AddListener(() => OnAvatarSelectPressed?.Invoke());
        }

        public void SetBalance(int balance)
        {
            _balanceText.text = balance.ToString();
        }
        
        public void SetName(string username)
        {
            _nameInputField.text = username;
        }

        public void SetAvatar(Sprite avatar)
        {
            if(avatar != null)
                _avatarSelectButton.image.sprite = avatar;
        }
    }
}