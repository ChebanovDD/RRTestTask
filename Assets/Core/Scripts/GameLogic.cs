using System.Collections;
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

        private int _currentCardIndex;
        private readonly CardLoader _cardLoader = new CardLoader();
        private readonly ImageLoader _imageLoader = new ImageLoader();

        private void Awake()
        {
            _cardLoader.CardDatasReady += OnCardDatasReady;
        }

        private void Start()
        {
            _cardLoader.LoadCards(_cardsLabelReference.labelString);
        }

        public void RandomAttack()
        {
            if (!_cardStack.HasCards)
            {
                return;
            }

            if (_currentCardIndex >= _cardStack.Count)
            {
                _currentCardIndex = 0;
            }

            var card = _cardStack.GetCard(_currentCardIndex);
            var attackValue = Random.Range(-2, 9);
            var parameterIndex = Random.Range(0, card.CardParametersCount);

            card.SetParameterValue(parameterIndex, attackValue);
            _currentCardIndex++;
        }

        private void OnCardDatasReady(object sender, IReadOnlyCollection<CardData> cardDatas)
        {
            StartCoroutine(InstantiateCards(cardDatas));
        }

        private IEnumerator InstantiateCards(IReadOnlyCollection<CardData> cardDatas)
        {
            foreach (var cardData in cardDatas)
            {
                if (cardData.Image == null)
                {
                    yield return _imageLoader.DownloadImage(GetImageUrl(), texture => { cardData.Image = texture; });
                }

                _cardReference.InstantiateAsync().Completed += handle =>
                {
                    var card = handle.Result.GetComponent<ICard>();
                    card.SetData(cardData);
                    card.HealthChanged += OnCardHealthChanged;

                    _cardStack.AddCard(card);
                };

                yield return null;
            }
        }

        private string GetImageUrl()
        {
            return $"https://picsum.photos/256/?random&t={Time.time}";
        }

        private void OnCardHealthChanged(object sender, int value)
        {
            if (value >= 1)
            {
                return;
            }

            var card = (ICard) sender;
            _cardStack.RemoveCard(card);
            _cardReference.ReleaseInstance(card.GameObject);
        }
    }
}
