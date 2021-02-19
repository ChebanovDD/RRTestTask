using System;
using System.Collections;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ScreenSpaceImplementation.Scripts
{
    public class AnimatedCardStatus : CardStatus
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

        public override bool SetValue(int value)
        {
            if (!CanSet(value))
            {
                return false;
            }
            
            StartCoroutine(AnimateValueChange(value));
            return true;
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
        
        protected override void SetDisplayValue(int value)
        {
            base.SetDisplayValue(value);
            _tmpText.text = value.ToString();
        }

        private Sequence AnimateNumber(GameObject numberObject)
        {
            return DOTween.Sequence()
                .Append(numberObject.transform.DOMoveY(numberObject.transform.position.y + _pathLength, _duration))
                .Append(numberObject.transform.DOScale(0, _duration / 4))
                .SetEase(_easeType);
        }
    }
}
