using System.Collections.Generic;
using System.Linq;
using Application.Services;
using Application.Services.UserData;
using Core;
using Core.Factory;
using UnityEngine;
using Zenject;

public class RecordsFactory : IInitializable
{
    private readonly UserDataService _userDataService;
    private readonly IAssetProvider _assetProvider;
    private readonly GameObjectFactory _gameObjectFactory;

    private GameObject _recordPrefab;
    
    public RecordsFactory(UserDataService userDataService, IAssetProvider assetProvider,
        GameObjectFactory gameObjectFactory)
    {
        _userDataService = userDataService;
        _assetProvider = assetProvider;
        _gameObjectFactory = gameObjectFactory;
    }
    
    public async void Initialize()
    {
        _recordPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.RecordDisplayPrefab);
    }

    public List<RecordDisplay> CreateRecordDisplayList()
    {
        var recordsData = CreateRecordsDataList();
        
        List<RecordDisplay> result = new List<RecordDisplay>(recordsData.Count);
        
        for (int i = 0; i < recordsData.Count; i++)
        {
            var display = _gameObjectFactory.Create<RecordDisplay>(_recordPrefab);
            display.Initialize(i + 1, recordsData[i].Name, recordsData[i].Score);
            result.Add(display);
        }
        
        return result;
    }

    private List<RecordData> CreateRecordsDataList()
    {
        var records = CreateFakeRecords();

        var usedData = _userDataService.GetUserData();
        var userRecord = new RecordData(usedData.UserAccountData.Username, usedData.UserProgressData.LastUnlockedLevelID);
        records.Add(userRecord);

        records = records.OrderByDescending(x => x.Score).ToList();
        return records;
    }

    private List<RecordData> CreateFakeRecords() => new()
    {
        new RecordData("Marta", 14),
        new RecordData("John", 13),
        new RecordData("Michael", 12),
        new RecordData("Micah", 12),
        new RecordData("Sandy", 11),
        new RecordData("Moe", 11),
        new RecordData("Dorothy", 10),
        new RecordData("Lisa", 10),
        new RecordData("Arthur", 9),
        new RecordData("Mike", 8),
        new RecordData("Luke", 8),
        new RecordData("Mona", 7),
        new RecordData("Dan", 6),
        new RecordData("Mob", 6),
        new RecordData("Ivan", 5),
        new RecordData("Lucy", 4),
        new RecordData("Bill", 4),
        new RecordData("Sophie", 3),
        new RecordData("Lamar", 3),
        new RecordData("Daniel", 3),
    };
    
    private class RecordData
    {
        public string Name;
        public int Score;

        public RecordData(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
