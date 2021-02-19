using Core;
using Core.Models;
using UnityEngine;

namespace CanvasImplementation
{
    public class ArcCardStack : CardStack
    {
        [Space]
        [Range(0.1f, 50)]
        [SerializeField] private float _spacing = 5;
        [Range(100, 2000)]
        [SerializeField] private float _radius = 650;
        [SerializeField] private float _bottomOffset = 100;

        public void Awake()
        {
            NormalizeValues();
        }

        protected override CardTransform[] CalculateCardTransforms(int cardsCount)
        {
            var isEven = cardsCount % 2 == 0;
            var angle = isEven ? -_spacing / 2 * (cardsCount - 1) : -_spacing * ((cardsCount - 1) / 2.0f);

            var result = new CardTransform[cardsCount];
            for (var i = 0; i < cardsCount; i++)
            {
                var x = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius + _container.position.x;
                var y = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius + _container.position.y;

                result[i] = new CardTransform(angle, new Vector3(x, y - _radius + _bottomOffset));

                angle += _spacing;
            }

            return result;
        }

        private void NormalizeValues()
        {
            var containerRect = _container.GetComponent<RectTransform>();
            containerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _radius);
            containerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _bottomOffset);

            var worldRect = GetWorldRect(containerRect);
            var containerScale = _container.lossyScale.x;

            _radius = worldRect.width * 2 / containerScale;
            _spacing = _spacing * containerScale;
            _bottomOffset = worldRect.height;
        }

        private Rect GetWorldRect(RectTransform rectTransform, float reduceBorders = 0)
        {
            var worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            return new Rect(worldCorners[0].x + reduceBorders, worldCorners[0].y + reduceBorders,
                worldCorners[3].x - worldCorners[0].x - reduceBorders * 2,
                worldCorners[1].y - worldCorners[0].y - reduceBorders * 2);
        }
    }
}
