using System;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class InfoPopup : BasePopup
    {
        [SerializeField] private SimpleButton _okButton;
        [SerializeField] private SimpleButton _ruleButton;
        [SerializeField] private SimpleButton _termsButton;
        [SerializeField] private SimpleButton _privacyPolicyButton;

        public event Action OnRulePress;
        public event Action OnTermsPress;
        public event Action OnPrivacyPolicyPress;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _okButton.Button.onClick.AddListener(Hide);
            _ruleButton.Button.onClick.AddListener(() => OnRulePress?.Invoke());
            _termsButton.Button.onClick.AddListener(() => OnTermsPress?.Invoke());
            _privacyPolicyButton.Button.onClick.AddListener(() => OnPrivacyPolicyPress?.Invoke());

            return base.Show(data, cancellationToken);
        }
    }
}