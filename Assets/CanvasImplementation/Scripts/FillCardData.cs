using CanvasImplementation.ViewModels;
using Core.Models;
using UnityEngine;

namespace CanvasImplementation
{
    public class FillCardData : MonoBehaviour
    {
        [SerializeField] private int _mana;
        [SerializeField] private int _attack;
        [SerializeField] private int _health;
        [SerializeField] private string _name;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private Texture _image;

        [Space]
        [SerializeField] private CardViewModel _card;

        public void Start()
        {
            _card.SetModel(new CardModel
            {
                Mana = _mana,
                Attack = _attack,
                Health = _health,
                Name = _name,
                Description = _description,
                Image = _image
            });
        }
    }
}
