using System.Collections.Generic;
using Core.Interfaces;
using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Core
{
    public class GameLogic : MonoBehaviour
    {
        [SerializeField] private AssetReference _cardReference;
        [SerializeField] private AssetLabelReference _cardsLabelReference;
        [SerializeField] private CardStack _cardStack;

        private readonly CardLoader _cardLoader = new CardLoader();

        public void Awake()
        {
            _cardLoader.CardDatasReady += OnCardDatasReady;
        }

        public void Start()
        {
            _cardLoader.LoadCards(_cardsLabelReference.labelString);
        }

        public void DecreaseCardMana()
        {
            var cardIndex = Random.Range(0, _cardStack.Count - 1);
            var manaValue = Random.Range(-2, 9);

            _cardStack.GetCard(cardIndex).SetMana(manaValue);
        }

        private void OnCardDatasReady(object sender, IReadOnlyCollection<CardData> cardDatas)
        {
            foreach (var cardData in cardDatas)
            {
                if (cardData.Image == null)
                {
                    // Download image.
                }

                _cardReference.InstantiateAsync().Completed += handle =>
                {
                    var card = handle.Result.GetComponent<ICard>();
                    card.SetData(cardData);
                    _cardStack.AddCard(card);
                };
            }
        }
    }
}
