using System;
using System.Collections.Generic;
using Core.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public abstract class CardStack : MonoBehaviour
    {
        [SerializeField] protected Transform _container;
        [SerializeField] protected CardAnimator _cardAnimator;

        private int _activeCardIndex = -1;
        protected readonly List<ICard> _cards = new List<ICard>();

        public int Count => _cards.Count;
        public bool HasCards => _cards.Count > 0;
        public bool HasActiveCard => _activeCardIndex >= 0;

        public event EventHandler<ICard> CardAdded;
        public event EventHandler<ICard> CardRemoved;

        public void AddCard(ICard card)
        {
            if (card == null)
            {
                throw new NullReferenceException("Card can not be null.");
            }

            if (_cards.Contains(card))
            {
                throw new InvalidOperationException("Can not add the same card twiñe.");
            }

            card.SetParent(_container.transform);

            _cards.Add(card);
            RecalculateTransforms();

            CardAdded?.Invoke(this, card);
        }

        public ICard GetCard(int index)
        {
            if (index < 0 || index >= _cards.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return _cards[index];
        }

        public Sequence ActivateCard(int index, out ICard card)
        {
            card = GetCard(index);

            _activeCardIndex = index;
            return _cardAnimator.ActivateCard(card);
        }

        public void DeactivateCard()
        {
            if (!HasActiveCard)
            {
                throw new InvalidOperationException("No active card found.");
            }

            _cardAnimator.DeactivateCard(_cards[_activeCardIndex]);
            _activeCardIndex = -1;
        }

        public void RemoveCard(ICard card)
        {
            _cards.Remove(card);
            _activeCardIndex = -1;

            RecalculateTransforms();

            CardRemoved?.Invoke(this, card);
        }

        public abstract void RecalculateTransforms();
    }
}
