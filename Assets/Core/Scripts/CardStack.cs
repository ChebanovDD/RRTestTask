using System;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace Core
{
    public abstract class CardStack : MonoBehaviour
    {
        [SerializeField] protected Transform _container;

        protected readonly List<ICard> _cards = new List<ICard>();

        public int Count => _cards.Count;
        public bool HasCards => _cards.Count > 0;

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
        }

        public ICard GetCard(int index)
        {
            if (index >= 0 && index < _cards.Count)
            {
                return _cards[index];
            }

            throw new IndexOutOfRangeException();
        }

        public void RemoveCard(ICard card)
        {
            _cards.Remove(card);
            RecalculateTransforms();
        }

        public abstract void RecalculateTransforms();
    }
}
