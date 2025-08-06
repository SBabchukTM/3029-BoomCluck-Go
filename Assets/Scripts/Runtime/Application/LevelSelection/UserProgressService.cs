using Application.Services.UserData;
using Core;

public class UserProgressService
{
    private readonly UserDataService _userDataService;
    private readonly ISettingProvider _settingProvider;
    private readonly IUserInventoryService _userInventoryService;
    private readonly GameData _gameData;

    public UserProgressService(UserDataService userDataService, 
        ISettingProvider settingProvider, GameData gameData, IUserInventoryService userInventoryService)
    {
        _userDataService = userDataService;
        _settingProvider = settingProvider;
        _gameData = gameData;
        _userInventoryService = userInventoryService;
    }

    public bool NextLevelExists()
    {
        return _gameData.LevelID + 1 < GetLevelsAmountInGame();
    }

    public void SaveProgress()
    {
        int currentLevelID = _gameData.LevelID;
        var progressData = _userDataService.GetUserData().UserProgressData;

        int levelReward = _settingProvider.Get<GameConfig>().LevelConfigs[currentLevelID].Reward;
        _gameData.Reward = levelReward;
        _userDataService.GetUserData().UserInventory.Balance += levelReward;
        
        int lastUnlockedLevel = GetLastUnlockedLevelID();

        if (currentLevelID != lastUnlockedLevel)
            return;

        if (currentLevelID + 1 >= GetLevelsAmountInGame())
            return;

        progressData.LastUnlockedLevelID = lastUnlockedLevel + 1;
    }
    
    public int GetLastUnlockedLevelID() => 
        _userDataService.GetUserData().UserProgressData.LastUnlockedLevelID;

    public bool CanGoToNextLevel()
    {
        if (!NextLevelExists())
            return true;
        
        int currentLevelID = _gameData.LevelID;
        LevelConfig nextLevelConfig = _settingProvider.Get<GameConfig>().LevelConfigs[currentLevelID + 1];
        return _userInventoryService.GetPurchasedGameItemsIDs().Count >= nextLevelConfig.UniqueCards;
    }

    private int GetLevelsAmountInGame() => 
        _settingProvider.Get<GameConfig>().LevelConfigs.Count;
}
