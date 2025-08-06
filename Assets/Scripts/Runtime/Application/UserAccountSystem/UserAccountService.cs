using System;
using Application.Services.UserData;
using Core;
using UnityEngine;

namespace Application.Services.AccountData
{
    public class UserAccountService
    {
        private readonly UserDataService _userDataService;
        private readonly ISettingProvider _settingProvider;
    
        public UserAccountService(UserDataService userDataService, ISettingProvider settingProvider)
        {
            _userDataService = userDataService;
            _settingProvider = settingProvider;
        }
    
        public UserAccountData GetAccountDataCopy()
        {
            return _userDataService.GetUserData().UserAccountData.Copy();
        }

        public void SaveAccountData(UserAccountData modifiedData)
        {
            var origData = _userDataService.GetUserData().UserAccountData;

            foreach (var field in typeof(UserAccountData).GetFields())
                field.SetValue(origData, field.GetValue(modifiedData));

            _userDataService.SaveUserData();
        }

        public Sprite GetUsedAvatarSprite(bool trySetDefaultIfNull = false)
        {
            var accountData = _userDataService.GetUserData().UserAccountData;
        
            if(accountData.AvatarBase64 == String.Empty)
                return null;

            return CreateAvatarSprite();
        }
        
        [Tooltip("Pass in the selected avatar and assign the returned string to the account data")]
        public string ConvertToBase64(Sprite sprite)
        {
            Texture2D readableTexture = GetReadableTexture(sprite.texture);
            return Convert.ToBase64String(readableTexture.EncodeToPNG());
        }

        private Sprite CreateAvatarSprite()
        {
            byte[] imageData = Convert.FromBase64String(_userDataService.GetUserData().UserAccountData.AvatarBase64);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        private void TrySetDefaultSprite()
        {
            var avatarsConfig = _settingProvider.Get<DefaultAvatarsConfig>();
            if (avatarsConfig == null)
                return;
        
            if(avatarsConfig.Avatars.Count == 0)
                return;
        
            _userDataService.GetUserData().UserAccountData.AvatarBase64 = ConvertToBase64(avatarsConfig.Avatars[0]);
            _userDataService.SaveUserData();
        }

        private Texture2D GetReadableTexture(Texture2D texture)
        {
            Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

            RenderTexture renderTex = RenderTexture.GetTemporary(
                texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
            
            Graphics.Blit(texture, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;

            newTexture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            newTexture.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            return newTexture;
        }
    }
}
