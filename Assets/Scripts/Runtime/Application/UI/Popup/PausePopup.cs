using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class PausePopup : BasePopup
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _homeButton;

        public event Action OnContinuePressed;
        public event Action OnHomePressed;
        
        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _continueButton.onClick.AddListener(() => OnContinuePressed?.Invoke());
            _homeButton.onClick.AddListener(() => OnHomePressed?.Invoke());
            return base.Show(data, cancellationToken);
        }
    }
}