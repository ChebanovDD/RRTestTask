using System;
using System.Collections.Generic;
using Core.Interfaces;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Core
{
    public class GameLogic : IDisposable
    {
        private int _currentCardIndex;
        private readonly CardStack _cardStack;

        public event EventHandler AttackFinish;

        public GameLogic(CardStack cardStack)
        {
            _cardStack = cardStack;
            _cardStack.CardAdded += OnCardAddedToStack;
            _cardStack.CardRemoved += OnCardRemovedFromStack;
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

        private void OnCardStatusValueChanged(object sender, int value)
        {
            RaiseAttackFinish();
        }

        private void RaiseAttackFinish()
        {
            AttackFinish?.Invoke(this, EventArgs.Empty);
        }
    }
}
