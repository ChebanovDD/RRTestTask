using Core.Extensions;
using Core.Interfaces;
using UnityEngine;

namespace Core
{
    public class GameLogic : MonoBehaviour
    {
        [SerializeField] private GameObject _card;
        [SerializeField] private CardStack _cardStack;
        
        public void AddCard()
        {
            _cardStack.AddCard(_card.CreateNew<ICard>(_cardStack.Container));
        }
    }
}
