using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Interfaces;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class GameLogic : IDisposable
    {
        private int _currentCardIndex;
        private readonly CardStack _cardStack;
        
        private delegate void StatusOperationDelegate(ICard card, CardStatus status);
        private readonly Dictionary<CardStats, StatusOperationDelegate> _statusOperations;

        public event EventHandler AttackFinish;

        public GameLogic(CardStack cardStack)
        {
            _cardStack = cardStack;
            _cardStack.CardAdded += OnCardAddedToStack;
            _cardStack.CardRemoved += OnCardRemovedFromStack;

            _statusOperations = new Dictionary<CardStats, StatusOperationDelegate>
            {
                {CardStats.Mana, OnManaValueChanged },
                {CardStats.Attack, OnAttackValueChanged },
                {CardStats.Health, OnHealthValueChanged }
            };
        }

        public void Dispose()
        {
            _cardStack.CardAdded -= OnCardAddedToStack;
            _cardStack.CardRemoved -= OnCardRemovedFromStack;
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

            if (_cardStack.HasActiveCard && _cardStack.Count > 1)
            {
                _cardStack.DeactivateCard();
            }

            _cardStack.ActivateCard(_currentCardIndex, out var card).OnComplete(() => { RandomAttack(card); });
        }

        private void RandomAttack(ICard card)
        {
            var attackValue = Random.Range(-2, 9);
            var parameterIndex = Random.Range(0, card.StatusCount);

            if (card.SetStatusValue(parameterIndex, attackValue))
            {
                _currentCardIndex++;
            }
            else
            {
                RandomAttack(card);
            }
        }

        private void OnCardAddedToStack(object sender, ICard card)
        {
            card.StatusValueChanged += OnCardStatusValueChanged;
        }

        private void OnCardRemovedFromStack(object sender, ICard card)
        {
            card.StatusValueChanged -= OnCardStatusValueChanged;
            RaiseAttackFinish();
        }

        private void OnCardStatusValueChanged(object sender, CardStatus cardStatus)
        {
            if (cardStatus == null)
            {
                throw new NullReferenceException();
            }

            if (_statusOperations.TryGetValue(cardStatus.Status, out var statusOperation))
            {
                statusOperation.Invoke(sender as ICard, cardStatus);
                RaiseAttackFinish();
            }
            else
            {
                throw new ArgumentException($"Status operation '{cardStatus.Status}' not found.");
            }
        }

        private void OnManaValueChanged(ICard card, CardStatus status)
        {
            Debug.Log($"Mana: {status.Value}");
        }

        private void OnAttackValueChanged(ICard card, CardStatus status)
        {
            Debug.Log($"Attack: {status.Value}");
        }

        private void OnHealthValueChanged(ICard card, CardStatus status)
        {
            if (card == null)
            {
                throw new NullReferenceException();
            }

            if (status.Value >= 1)
            {
                return;
            }

            _currentCardIndex = _cardStack.GetIndex(card);
            if (_currentCardIndex == -1)
            {
                throw new InvalidOperationException("Card not found in stack.");
            }

            _cardStack.RemoveCard(card);
        }

        private void RaiseAttackFinish()
        {
            AttackFinish?.Invoke(this, EventArgs.Empty);
        }
    }
}
