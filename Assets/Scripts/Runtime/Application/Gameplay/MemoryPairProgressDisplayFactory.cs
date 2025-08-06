using System.Collections.Generic;
using Application.Services;
using Application.Services.UserData;
using Core;
using Core.Factory;
using UnityEngine;
using Zenject;

namespace Runtime.Application.Gameplay
{
    public class MemoryPairProgressDisplayFactory : IInitializable
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameObjectFactory _factory;
        private readonly IUserInventoryService _userInventoryService;

        private GameObject _prefab;

        public MemoryPairProgressDisplayFactory(IAssetProvider assetProvider, GameObjectFactory factory)
        {
            _assetProvider = assetProvider;
            _factory = factory;
        }

        public async void Initialize()
        {
            _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.MemoryPairSolveProgressDisplay);
        }

        public List<MemoryPairSolveProgressDisplay> CreateMemoryPairSolveProgressDisplayList(List<MemoryCard> memoryCards)
        {
            var dictionary = GetPairsTarget(memoryCards);

            var displayList = CreateDislayList(dictionary);

            return displayList;
        }

        private Dictionary<Sprite, int> GetPairsTarget(List<MemoryCard> memoryCards)
        {
            Dictionary<Sprite, int> dictionary = new ();
            foreach (var card in memoryCards)
            {
                if(!dictionary.TryAdd(card.CardSprite, 1))
                    dictionary[card.CardSprite]++;
            }

            return dictionary;
        }

        private List<MemoryPairSolveProgressDisplay> CreateDislayList(Dictionary<Sprite, int> dictionary)
        {
            var displayList = new List<MemoryPairSolveProgressDisplay>();

            foreach (var kv in dictionary)
            {
                var display = _factory.Create<MemoryPairSolveProgressDisplay>(_prefab);
                display.Initialize(kv.Key, kv.Value / 2);
                displayList.Add(display);
            }

            return displayList;
        }
    }
}