using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasImplementation.ViewModels
{
    public class CardViewModel : MonoBehaviour, ICard
    {
        [SerializeField] private TMP_Text _manaLabel;
        [SerializeField] private TMP_Text _attackLabel;
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _descriptionLabel;
        [SerializeField] private RawImage _image;

        public void SetModel(CardModel model)
        {
            _manaLabel.text = model.Mana.ToString();
            _attackLabel.text = model.Attack.ToString();
            _healthLabel.text = model.Health.ToString();
            
            _nameLabel.text = model.Name;
            _descriptionLabel.text = model.Description;

            if (model.Image != null)
            {
                _image.texture = model.Image;
                _image.FillParent();
            }
        }

        public void SetAngle(float angle)
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(Vector3.forward, angle);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
