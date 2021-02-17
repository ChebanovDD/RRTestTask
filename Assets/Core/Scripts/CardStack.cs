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

        public Transform Container => _container;

        public void AddCard(ICard card)
        {
            if (_cards.Contains(card))
            {
                throw new InvalidOperationException("Can not add the same card twiñe.");
            }

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
            var startAngle = isEven ? -_spacing / 2 * (count - 1) : -_spacing * ((count - 1) / 2);

            foreach (var card in _cards)
            {
                var x = Mathf.Sin(startAngle * Mathf.Deg2Rad) * _radius + _container.position.x;
                var y = Mathf.Cos(startAngle * Mathf.Deg2Rad) * _radius + _container.position.y;

                card.SetAngle(-startAngle);
                card.SetPosition(new Vector3(x, y - _radius + _verticalOffset));

                startAngle += _spacing;
            }
        }
    }
}
