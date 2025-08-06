using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class RulesPopup : BasePopup
    {
        [SerializeField] private Button _closeButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _closeButton.onClick.AddListener(DestroyPopup);
            return base.Show(data, cancellationToken);
        }
    }
}