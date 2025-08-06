using Application.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class PrivacyPolicyPopup : BasePopup
    {
        [SerializeField] private SimpleButton _okButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _okButton.Button.onClick.AddListener(DestroyPopup);
            return base.Show(data, cancellationToken);
        }
    }
}