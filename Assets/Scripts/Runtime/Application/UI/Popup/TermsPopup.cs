using Application.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Core.UI
{
    public class TermsPopup : BasePopup
    {
        [SerializeField] private SimpleButton _okButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _okButton.Button.onClick.AddListener(DestroyPopup);
            return base.Show(data, cancellationToken);
        }
    }
}