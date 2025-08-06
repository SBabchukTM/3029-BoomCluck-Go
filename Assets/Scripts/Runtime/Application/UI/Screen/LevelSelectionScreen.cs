using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class LevelSelectionScreen : UiScreen
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _accountButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private TextMeshProUGUI _balanceText;
        
        [SerializeField, Space] private RectTransform _levelSelectionButtonsParent;

        [SerializeField, Space] private LevelSelectionButtonStatusDisplay _selectedButtonDisplay;

        [SerializeField] private LevelSelectionButtonStatusDisplay _unlockedButtonDisplay;

        [SerializeField] private LevelSelectionButtonStatusDisplay _lockedButtonDisplay;

        public event Action<int> OnLevelSelected;
        
        private LevelSelectionButton[] _levelSelectionButtons;

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        public void Initialize(int cardsPurchased, List<LevelConfig> levelConfigs)
        {
            FindLevelSelectionButtons();
            InitializeButtons(cardsPurchased, levelConfigs);
        }

        public void SetBalance(int balance) => _balanceText.text = balance.ToString();

        public void SubscribeCallbacks(Action settingCallback, Action leaderboardCallback,
            Action homeCallback, Action accountCallback, Action helpCallback, Action shopCallback)
        {
            _settingsButton.onClick.AddListener(settingCallback.Invoke);
            _leaderboardButton.onClick.AddListener(leaderboardCallback.Invoke);
            _homeButton.onClick.AddListener(homeCallback.Invoke);
            _accountButton.onClick.AddListener(accountCallback.Invoke);
            _helpButton.onClick.AddListener(helpCallback.Invoke);
            _shopButton.onClick.AddListener(shopCallback.Invoke);
        }

        private void UnsubscribeFromEvents()
        {
            int size = _levelSelectionButtons.Length;
            for (int i = 0; i < size; i++)
                _levelSelectionButtons[i].OnLevelSelected -= UpdateSelectedLevel;
        }

        private void FindLevelSelectionButtons()
        {
            _levelSelectionButtons = _levelSelectionButtonsParent.GetComponentsInChildren<LevelSelectionButton>();
        }

        private void InitializeButtons(int cardsPurchased, List<LevelConfig> levelConfigs)
        {
            int size = _levelSelectionButtons.Length;
            
            for (int i = 0; i < size; i++)
            {
                int cardsRequired = levelConfigs[i].UniqueCards;
                bool locked = cardsPurchased < cardsRequired;

                var button = _levelSelectionButtons[i];
                InitializeButton(locked, button, cardsRequired);
                button.OnLevelSelected += UpdateSelectedLevel;
            }
        }

        private void InitializeButton(bool locked, LevelSelectionButton button, int cardsRequired)
        {
            button.Initialize(locked, cardsRequired);

            if(locked)
                SetButtonStatusDisplay(button, _lockedButtonDisplay);
            else
                SetButtonStatusDisplay(button, _unlockedButtonDisplay);
        }

        private void SetButtonStatusDisplay(LevelSelectionButton button, LevelSelectionButtonStatusDisplay display)
        {
            button.SetColor(display.Color);
            if(display.Sprite)
                button.SetSprite(display.Sprite);
        }

        private void UpdateSelectedLevel(int level)
        {
            OnLevelSelected?.Invoke(level);
        }
    }

    [Serializable]
    public class LevelSelectionButtonStatusDisplay
    {
        [Header("If Sprite is Null, it won't be set")]
        public Sprite Sprite;
        public Color Color = Color.white;
    }
}