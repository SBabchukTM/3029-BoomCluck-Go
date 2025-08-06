using System.Collections.Generic;
using Application.Services;
using Application.Services.UserData;
using Application.Tools;
using Core;
using Core.Factory;
using UnityEngine;
using Zenject;

public class MemoryCardFactory : IInitializable
{
    private readonly IAssetProvider _assetProvider;
    private readonly GameObjectFactory _factory;
    private readonly IUserInventoryService _userInventoryService;

    private GameObject _prefab;

    public MemoryCardFactory(IAssetProvider assetProvider, GameObjectFactory factory, IUserInventoryService userInventoryService)
    {
        _assetProvider = assetProvider;
        _factory = factory;
        _userInventoryService = userInventoryService;
    }
    
    public async void Initialize()
    {
        _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.MemoryCardPrefab);
    }

    public List<MemoryCard> CreateMemoryCards(LevelConfig levelConfig)
    {
        int pairs = levelConfig.CardPairs;
        int uniqueCards = levelConfig.UniqueCards;

        var spritesInGame = GetCardsInGame(uniqueCards);

        var memoryCards = CreateMemoryCards(pairs, spritesInGame);

        return memoryCards;
    }

    private List<Sprite> GetCardsInGame(int uniqueCards)
    {
        var purchasedCards = _userInventoryService.GetPurchasedGameItemSprites();
        Tools.Shuffle(purchasedCards);
        List<Sprite> spritesInGame = new(uniqueCards);
        for (int i = 0; i < uniqueCards; i++)
            spritesInGame.Add(purchasedCards[i]);
        return spritesInGame;
    }

    private List<MemoryCard> CreateMemoryCards(int pairs, List<Sprite> spritesInGame)
    {
        List<MemoryCard> memoryCards = new (pairs * 2);

        for (int i = 0; i < pairs; i++)
        {
            Sprite sprite = spritesInGame[i % spritesInGame.Count];

            for (int j = 0; j < 2; j++)
            {
                var memoryCard = _factory.Create<MemoryCard>(_prefab);
                memoryCard.Initialize(sprite);
                memoryCards.Add(memoryCard);
            }
        }

        Tools.Shuffle(memoryCards);
        return memoryCards;
    }
}

