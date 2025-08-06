using System;
using System.Collections.Generic;

namespace Application.Services.UserData
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new List<GameSessionData>();
        public SettingsData SettingsData = new SettingsData();
        public GameData GameData = new GameData();
        public UserInventory UserInventory = new UserInventory();
        public UserProgressData UserProgressData = new UserProgressData();
        public UserAccountData UserAccountData = new UserAccountData();
    }
}