using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class LosePopup : BasePopup
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _retryButton;

        public event Action OnHomePressed;
        public event Action OnRetryPressed;
        
        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _homeButton.onClick.AddListener(() => OnHomePressed?.Invoke());
            _retryButton.onClick.AddListener(() => OnRetryPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }
    }
}