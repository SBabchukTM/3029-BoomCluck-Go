using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class WinPopup : BasePopup
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private RectTransform _errorGO;
        
        public event Action OnHomePressed;
        public event Action OnNextLevelPressed;
        
        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _homeButton.onClick.AddListener(() => OnHomePressed?.Invoke());
            _nextLevelButton.onClick.AddListener(() => OnNextLevelPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetReward(int reward)
        {
            _rewardText.text = reward.ToString();
        }
        
        public void EnableError(bool enable)
        {
            _errorGO.gameObject.SetActive(enable);
            _nextLevelButton.gameObject.SetActive(!enable);
        }
    }
}