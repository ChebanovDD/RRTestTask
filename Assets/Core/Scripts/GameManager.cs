using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interfaces;
using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameView _gameView;
        [SerializeField] private CardStack _cardStack;

        [Space]
        [SerializeField] private AssetReference _cardReference;
        [SerializeField] private AssetLabelReference _cardsLabelReference;
        
        private GameLogic _gameLogic;
        private readonly CardLoader _cardLoader = new CardLoader();
        private readonly ImageLoader _imageLoader = new ImageLoader();
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private void Awake()
        {
            _gameLogic = new GameLogic(_cardStack);
            _gameLogic.AttackFinish += OnAttackFinish;
            _cardLoader.CardDatasReady += OnCardDatasReady;
            _gameView.GameBoard.RandomAttack += OnRandomAttack;
        }

        private void Start()
        {
            _cardLoader.LoadCards(_cardsLabelReference.labelString);
        }

        private void OnDestroy()
        {
            _gameLogic.Dispose();
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        private void OnCardDatasReady(object sender, IReadOnlyCollection<CardData> cardDatas)
        {
            StartCoroutine(InstantiateCards(cardDatas));
        }

        private IEnumerator InstantiateCards(IReadOnlyCollection<CardData> cardDatas)
        {
            if (cardDatas == null || cardDatas.Count == 0)
            {
                yield break;
            }

            var progressBar = _gameView.SplashScreen.ProgressBar;
            progressBar.SetIterationCount(cardDatas.Count);

            foreach (var cardData in cardDatas)
            {
                if (cardData.Image == null)
                {
                    yield return _imageLoader.DownloadImage(GetImageUrl(), texture =>
                    {
                        cardData.Image = texture;
                        _disposables.Add(cardData);
                    }, progressBar);
                }

                progressBar.NextIteration();

                var asyncOperation = _cardReference.InstantiateAsync();
                asyncOperation.Completed += handle => { OnCardInstantiated(handle.Result, cardData); };

                yield return asyncOperation;
            }

            _gameView.SplashScreen.Hide();
            _cardStack.RecalculateTransforms();
        }

        private void OnCardInstantiated(GameObject cardObject, CardData data)
        {
            var card = cardObject.GetComponent<ICard>();
            card.SetData(data);
            card.HealthChanged += OnCardHealthChanged;

            _cardStack.AddCard(card, false);
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

            if (!_cardStack.HasCards)
            {
                _gameView.GameOverScreen.Show();
            }
        }

        private void OnRandomAttack(object sender, EventArgs e)
        {
            _gameView.GameBoard.SetRandomAttackButtonActive(false);
            _gameLogic.RandomAttack();
        }

        private void OnAttackFinish(object sender, EventArgs e)
        {
            _gameView.GameBoard.SetRandomAttackButtonActive(true);
        }
    }
}
