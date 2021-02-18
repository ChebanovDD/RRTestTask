using System;
using System.Collections.Generic;
using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Core
{
    public class CardLoader
    {
        private int _cardsCount;
        private readonly List<CardData> _cardDatas = new List<CardData>();

        public event EventHandler<IReadOnlyCollection<CardData>> CardDatasReady;

        public void LoadCards(string labelString)
        {
            Addressables.LoadResourceLocationsAsync(labelString).Completed += OnCardsReady;
        }

        private void OnCardsReady(AsyncOperationHandle<IList<IResourceLocation>> obj)
        {
            if (obj.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError(obj.OperationException.Message);
                return;
            }

            _cardsCount = obj.Result.Count;
            foreach (var cardResourceLocation in obj.Result)
            {
                Addressables.LoadAssetAsync<CardData>(cardResourceLocation).Completed += OnCardDataLoaded;
            }
        }

        private void OnCardDataLoaded(AsyncOperationHandle<CardData> obj)
        {
            _cardDatas.Add(obj.Result);

            if (_cardsCount == _cardDatas.Count)
            {
                RaiseCardDatasReady(_cardDatas);
            }
        }

        private void RaiseCardDatasReady(IReadOnlyCollection<CardData> cardDatas)
        {
            CardDatasReady?.Invoke(this, cardDatas);
        }
    }
}
