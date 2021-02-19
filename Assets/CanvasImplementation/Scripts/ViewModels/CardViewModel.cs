using System;
using System.Collections.Generic;
using CanvasImplementation.BaseElements;
using Core.Extensions;
using Core.Interfaces;
using Core.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasImplementation.ViewModels
{
    public class CardViewModel : MonoBehaviour, ICard
    {
        [SerializeField] private AnimatedNumberLabel _manaLabel;
        [SerializeField] private AnimatedNumberLabel _attackLabel;
        [SerializeField] private AnimatedNumberLabel _healthLabel;
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _descriptionLabel;
        [SerializeField] private RawImage _image;

        private List<AnimatedNumberLabel> _cardParameters;

        public int CardParametersCount => _cardParameters.Count;
        public GameObject GameObject => gameObject;

        public event EventHandler<int> HealthChanged;

        private void Awake()
        {
            _healthLabel.MinValue = 0;
            _cardParameters = new List<AnimatedNumberLabel>
            {
                _manaLabel,
                _attackLabel,
                _healthLabel
            };

            _healthLabel.ValueChanged += OnHealthValueChanged;
        }
        
        private void OnDestroy()
        {
            _cardParameters.Clear();
            _healthLabel.ValueChanged -= OnHealthValueChanged;
        }

        public void SetData(CardData data)
        {
            _manaLabel.SetValueWithoutAnimation(data.Mana);
            _attackLabel.SetValueWithoutAnimation(data.Attack);
            _healthLabel.SetValueWithoutAnimation(data.Health);

            _nameLabel.text = data.Name;
            _descriptionLabel.text = data.Description;

            _image.texture = data.Image;
            _image.FillParent();
        }

        public void SetParameterValue(int index, int value)
        {
            if (index >= 0 && index < _cardParameters.Count)
            {
                var parameter = _cardParameters[index];
                parameter.SetValue(value >= parameter.MinValue ? value : parameter.MinValue);
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public void SetAngle(float angle)
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(Vector3.back, angle);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        private void OnHealthValueChanged(object sender, int value)
        {
            HealthChanged?.Invoke(this, value);
        }
    }
}
