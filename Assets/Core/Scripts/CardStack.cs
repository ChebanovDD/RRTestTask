using System;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace Core
{
    public class CardStack : MonoBehaviour
    {
        [Range(0, 50)]
        [SerializeField] private int _spacing = 10;
        [Range(100, 1000)]
        [SerializeField] private int _radius = 800;
        [SerializeField] private float _verticalOffset;
        
        [SerializeField] private Transform _container;

        private readonly List<ICard> _cards = new List<ICard>();
        
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

        public void RecalculateTransforms()
        {
            if (_cards.Count == 0)
            {
                return;
            }

            var count = _cards.Count;
            var isEven = count % 2 == 0;
            var angle = isEven ? -_spacing / 2 * (count - 1) : -_spacing * ((count - 1) / 2);

            foreach (var card in _cards)
            {
                var x = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius + _container.position.x;
                var y = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius + _container.position.y;

                card.SetAngle(angle);
                card.SetPosition(new Vector3(x, y - _radius + _verticalOffset));

                angle += _spacing;
            }
        }
    }
}
