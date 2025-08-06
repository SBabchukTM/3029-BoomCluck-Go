using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.UI
{
    public class GameplayScreen : UiScreen
    {
        [SerializeField] private GridLayoutGroup _gridLayout;
        [SerializeField] private RectTransform _progressParent;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TextMeshProUGUI _timeText;
        
        private GridLayoutSetupController _gridLayoutSetupController;
        private GameData _gameData;

        public event Action OnPausePressed;
        
        [Inject]
        private void Construct(GridLayoutSetupController gridLayoutSetupController, GameData gameData)
        {
            _gridLayoutSetupController = gridLayoutSetupController;
            _gameData = gameData;
            
            _gameData.OnTimeChanged += UpdateTime;
        }

        public void Initialize()
        {
            _pauseButton.onClick.AddListener(() => OnPausePressed?.Invoke());
        }

        public void ParentMemoryCards(List<MemoryCard> memoryCards)
        {
            _gridLayoutSetupController.AdjustGridLayout(_gridLayout, memoryCards.Count);
            foreach (var memoryCard in memoryCards)
            {
                memoryCard.transform.SetParent(_gridLayout.transform, false);
            }
        }

        public void ParentProgressDisplay(List<MemoryPairSolveProgressDisplay> progressDisplayList)
        {
            foreach (var progress in progressDisplayList)
            {
                progress.transform.SetParent(_progressParent, false);
            }
        }

        private void UpdateTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            
            _timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}