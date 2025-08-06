﻿using System.Threading;
using Application.Services.UserData;
using Application.UI;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;

namespace Runtime.Application.ApplicationStates.Game.Controllers
{
    public sealed class StartSettingsController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;

        public StartSettingsController(IUiService uiService, UserDataService userDataService, IAudioService audioService)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);

            SettingsPopup settingsPopup = _uiService.GetPopup<SettingsPopup>(ConstPopups.SettingsPopup);

            settingsPopup.SoundVolumeChangeEvent += OnChangeSoundVolume;
            settingsPopup.MusicVolumeChangeEvent += OnChangeMusicVolume;

            var userData = _userDataService.GetUserData();

            var isSoundVolume = userData.SettingsData.IsSoundVolume;
            var isMusicVolume = userData.SettingsData.IsMusicVolume;
            var gameDifficultyMode = userData.SettingsData.GameDifficultyMode;

            settingsPopup.Show(new SettingsPopupData(isSoundVolume, isMusicVolume, gameDifficultyMode), cancellationToken).Forget();
            CurrentState = ControllerState.Complete;
            return UniTask.CompletedTask;
        }
        
        
        private void OnChangeSoundVolume(bool state)
        {
            _audioService.SetVolume(AudioType.Sound, state ? 1 : 0);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.IsSoundVolume = state;
        }

        private void OnChangeMusicVolume(bool state)
        {
            _audioService.SetVolume(AudioType.Music, state ? 1 : 0);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.IsMusicVolume = state;
        }

        private void OnChangeGameDifficultyMode(GameDifficultyMode gameDifficultyMode)
        {
            var userData = _userDataService.GetUserData();
            userData.SettingsData.GameDifficultyMode = gameDifficultyMode;
        }
    }
}