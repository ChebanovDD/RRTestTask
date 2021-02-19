using System;
using System.Collections.Generic;
using CanvasImplementation.BaseElements;
using Core;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;
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

        private List<CardStatus> _cardStatuses;

        public int StatusCount => _cardStatuses.Count;
        public GameObject GameObject => gameObject;
        public Vector3 StackPosition { get; private set; }
        public Quaternion StackRotation { get; private set; }

        public event EventHandler<CardStatus> StatusValueChanged;

        private void Awake()
        {
            _cardStatuses = new List<CardStatus>
            {
                _manaLabel,
                _attackLabel,
                _healthLabel
            };

            foreach (var cardStatus in _cardStatuses)
            {
                cardStatus.ValueChanged += OnCardStatusValueChanged;
            }
        }

        private void OnDestroy()
        {
            foreach (var cardStatus in _cardStatuses)
            {
                cardStatus.ValueChanged -= OnCardStatusValueChanged;
            }

            _cardStatuses.Clear();
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

        public bool SetStatusValue(int index, int value)
        {
            if (index < 0 || index >= _cardStatuses.Count)
            {
                throw new IndexOutOfRangeException();
            }

            var cardStatus = _cardStatuses[index];
            return cardStatus.SetValue(value >= cardStatus.MinValue ? value : cardStatus.MinValue);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetTransform(CardTransform cardTransform)
        {
            SetStackAngle(cardTransform.Angle);
            SetStackPosition(cardTransform.Position);
        }

        private void OnCardStatusValueChanged(object sender, int value)
        {
            StatusValueChanged?.Invoke(this, sender as CardStatus);
        }

        private void SetStackAngle(float angle)
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(Vector3.back, angle);
            StackRotation = transform.rotation;
        }

        public void SetStackPosition(Vector3 position)
        {
            transform.position = position;
            StackPosition = position;
        }
    }
}
