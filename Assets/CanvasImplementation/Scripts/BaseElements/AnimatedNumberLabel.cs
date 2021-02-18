using System;
using System.Collections;
using Core.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CanvasImplementation.BaseElements
{
    public class AnimatedNumberLabel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;

        [Header("Animation")]
        [SerializeField] private float _delay;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _easeType;

        [Header("Prefabs")]
        [SerializeField] private GameObject _increaseSpritePrefab;
        [SerializeField] private GameObject _decreaseSpritePrefab;

        private int _value;
        private int _previousValue;

        private void Awake()
        {
            if (TryParse(_tmpText.text, out var value))
            {
                _value = value;
                _previousValue = value;
            }
            else
            {
                throw new FormatException("Invalid string.");
            }
        }

        public void SetValue(int value)
        {
            if (_value == value)
            {
                return;
            }

            SetValueWithoutAnimation(value);
            StartCoroutine(AnimateValueChange(value));
        }

        public void SetValueWithoutAnimation(int value)
        {
            _value = value;
            _tmpText.text = value.ToString();
        }

        private bool TryParse(string text, out int value)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                value = default;
                return false;
            }

            return int.TryParse(text, out value);
        }

        private IEnumerator AnimateValueChange(int newValue)
        {
            var spritesCount = Mathf.Abs(_previousValue - newValue);
            var isDecreaseMode = _previousValue > newValue;

            for (var i = 0; i < spritesCount; i++)
            {
                if (isDecreaseMode)
                {
                    UpdateValue(_previousValue - 1);
                    AnimateNumber(_decreaseSpritePrefab.CreateNew());
                }
                else
                {
                    UpdateValue(_previousValue + 1);
                    AnimateNumber(_increaseSpritePrefab.CreateNew());
                }

                yield return new WaitForSeconds(_delay);
            }
        }

        private void UpdateValue(int value)
        {
            _tmpText.text = value.ToString();
            _previousValue = value;
        }

        private void AnimateNumber(GameObject numberObject)
        {
            numberObject.transform.SetParent(transform);
            numberObject.transform.position = new Vector3(transform.position.x, transform.position.y + 25);

            DOTween.Sequence()
                .Append(numberObject.transform.DOMoveY(numberObject.transform.position.y + 250, _duration))
                .Append(numberObject.transform.DOScale(0, _duration / 4))
                .SetEase(_easeType)
                .OnComplete(() => { Destroy(numberObject); });
        }
    }
}
