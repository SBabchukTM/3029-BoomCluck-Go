using System.Threading;
using Application.Game;
using Application.UI;
using Core.StateMachine;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ILogger = Core.ILogger;

public class LosePopupStateController : StateController
{
    private readonly IUiService _uiService;
    
    public LosePopupStateController(ILogger logger, IUiService uiService) : base(logger)
    {
        _uiService = uiService;
    }

    public override async UniTask Enter(CancellationToken cancellationToken = default)
    {
        Time.timeScale = 0;
        LosePopup losePopup = await _uiService.ShowPopup(ConstPopups.LosePopup) as LosePopup;

        losePopup.OnRetryPressed += async () =>
        {
            Time.timeScale = 1;
            losePopup.DestroyPopup();
            await GoTo<GameplayStateController>();
        };

        losePopup.OnHomePressed += async () =>
        {
            Time.timeScale = 1;
            losePopup.DestroyPopup();
            await GoTo<MenuStateController>();
        };
    }
}
