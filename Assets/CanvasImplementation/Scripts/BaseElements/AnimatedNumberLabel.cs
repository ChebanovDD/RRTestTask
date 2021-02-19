using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CanvasImplementation.BaseElements
{
    public class AnimatedNumberLabel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;

        [Header("Animation")]
        [SerializeField] private float _delay = 0.2f;
        [SerializeField] private float _duration = 0.4f;
        [SerializeField] private float _pathLength = 250;
        [SerializeField] private Ease _easeType;
        [SerializeField] private Transform _spawnPoint;

        [Header("Prefabs")]
        [SerializeField] private AssetReference _increaseAssetReference;
        [SerializeField] private AssetReference _decreaseAssetReference;

        private int _value;
        private int _displayValue;

        public int Value
        {
            get => _value;
            private set
            {
                if (!CanSet(value))
                {
                    return;
                }

                _value = value;
                SetDisplayValue(value);
                RaiseValueChanged(value);
            }
        }

        public int MinValue { get; set; } = int.MinValue;

        public event EventHandler<int> ValueChanged;

        private void Awake()
        {
            if (TryParse(_tmpText.text, out var value))
            {
                _value = value;
                _displayValue = value;
            }
            else
            {
                throw new FormatException("Invalid string.");
            }
        }

        public void SetValue(int value)
        {
            if (CanSet(value))
            {
                StartCoroutine(AnimateValueChange(value));
            }
        }

        public void SetValueWithoutAnimation(int value)
        {
            Value = value;
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

        private bool CanSet(int value)
        {
            return _value != value;
        }

        private IEnumerator AnimateValueChange(int newValue)
        {
            var spritesCount = Mathf.Abs(_value - newValue);
            var isDecreaseMode = _value > newValue;
            var spritesCounter = 0;

            for (var i = 0; i < spritesCount; i++)
            {
                if (isDecreaseMode)
                {
                    AnimateNumber(_decreaseAssetReference, _displayValue - 1, () => spritesCounter++);
                }
                else
                {
                    AnimateNumber(_increaseAssetReference, _displayValue + 1, () => spritesCounter++);
                }

                yield return new WaitForSeconds(_delay);
            }

            while (spritesCounter != spritesCount)
            {
                yield return null;
            }

            Value = newValue;
        }

        private void AnimateNumber(AssetReference assetReference, int newValue, Action onComplete)
        {
            InstantiateNumberAsync(assetReference).Completed += handle =>
            {
                SetDisplayValue(newValue);
                AnimateNumber(handle.Result).OnComplete(() =>
                {
                    assetReference.ReleaseInstance(handle.Result);
                    onComplete?.Invoke();
                });
            };
        }

        private AsyncOperationHandle<GameObject> InstantiateNumberAsync(AssetReference assetReference)
        {
            return assetReference.InstantiateAsync(_spawnPoint.position, Quaternion.identity, transform);
        }
        
        private void SetDisplayValue(int value)
        {
            _tmpText.text = value.ToString();
            _displayValue = value;
        }

        private Sequence AnimateNumber(GameObject numberObject)
        {
            return DOTween.Sequence()
                .Append(numberObject.transform.DOMoveY(numberObject.transform.position.y + _pathLength, _duration))
                .Append(numberObject.transform.DOScale(0, _duration / 4))
                .SetEase(_easeType);
        }

        private void RaiseValueChanged(int value)
        {
            ValueChanged?.Invoke(this, value);
        }
    }
}
