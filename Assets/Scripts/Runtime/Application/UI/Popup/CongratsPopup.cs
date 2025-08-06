using Application.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class CongratsPopup : BasePopup
    {
        [SerializeField] private SimpleButton _backgroundButton;
        [SerializeField] private TextMeshProUGUI _amount;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            CongratsPopupData congratsPopupData = data as CongratsPopupData;

            _amount.text = congratsPopupData.Amount.ToString();

            _backgroundButton.Button.onClick.AddListener(DestroyPopup);

            return base.Show(data, cancellationToken);
        }
    }
}