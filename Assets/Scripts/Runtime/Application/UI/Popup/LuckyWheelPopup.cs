using Application.Services.UserData;
using Application.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI
{
    public class LuckyWheelPopup : BasePopup
    {
        private IUiService _uiService;
        private UserDataService _userDataService;

        [SerializeField] private WheelOfFortune WheelOfFortune;
        [SerializeField] private SimpleButton SpinButton;

        [SerializeField] private TextMeshProUGUI _balanceText;

        public event Action OnEndSpin;

        [Inject]
        public void Construct(IUiService uiService,
               UserDataService userDataService)
        {
            _uiService = uiService;
            _userDataService = userDataService;
        }

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _balanceText.text = _userDataService.GetUserData().UserInventory.Balance.ToString();
            WheelOfFortune.GenerateSectors(12, 100);
            SpinButton.Button.onClick.RemoveAllListeners();
            SpinButton.Button.onClick.AddListener(StartGame);
            WheelOfFortune.WinAmount += AddAmount;
            return base.Show(data, cancellationToken);
        }

        public void StartGame()
        {
            SpinButton.Button.interactable = false;
            WheelOfFortune.SpinWheel();
        }

        public void AddAmount(int amount)
        {
            _balanceText.text = amount.ToString();

            _userDataService.GetUserData().UserInventory.Balance += amount;
            _userDataService.SaveUserData();

            var popup = _uiService.GetPopup<CongratsPopup>(ConstPopups.CongratsPopup);

            popup.Show(new CongratsPopupData() { Amount = amount });
            popup.DestroyPopupEvent += () =>
            {
                DestroyPopup();
                OnEndSpin?.Invoke();
            };
        }
    }
}