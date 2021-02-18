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
        
        public void SetMana(int value)
        {
            _manaLabel.SetValue(value);
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
    }
}
