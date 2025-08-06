using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Application.Game;
using Application.UI;
using Core.StateMachine;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ILogger = Core.ILogger;

public class WinPopupStateController : StateController
{
    private readonly IUiService _uiService;
    private readonly UserProgressService _userProgressService;
    private readonly GameData _gameData;
    
    public WinPopupStateController(ILogger logger, IUiService uiService, UserProgressService userProgressService, GameData gameData) : base(logger)
    {
        _uiService = uiService;
        _userProgressService = userProgressService;
        _gameData = gameData;
    }

    public override async UniTask Enter(CancellationToken cancellationToken = default)
    {
        Time.timeScale = 0;
        WinPopup winPopup = await _uiService.ShowPopup(ConstPopups.WinPopup) as WinPopup;

        winPopup.EnableError(!_userProgressService.CanGoToNextLevel());
        winPopup.SetReward(_gameData.Reward);
        
        winPopup.OnNextLevelPressed += async () =>
        {
            Time.timeScale = 1;
            winPopup.DestroyPopup();

            if (_userProgressService.NextLevelExists())
                _gameData.LevelID++;
            
            await GoTo<GameplayStateController>();
        };

        winPopup.OnHomePressed += async () =>
        {
            Time.timeScale = 1;
            winPopup.DestroyPopup();
            await GoTo<MenuStateController>();
        };
    }
}
