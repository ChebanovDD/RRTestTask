using Random = UnityEngine.Random;

namespace Core
{
    public class GameLogic
    {
        private int _currentCardIndex;
        private readonly CardStack _cardStack;

        public GameLogic(CardStack cardStack)
        {
            _cardStack = cardStack;
        }

        public void RandomAttack()
        {
            if (!_cardStack.HasCards)
            {
                return;
            }

            if (_currentCardIndex >= _cardStack.Count)
            {
                _currentCardIndex = 0;
            }

            var card = _cardStack.GetCard(_currentCardIndex);
            var attackValue = Random.Range(-2, 9);
            var parameterIndex = Random.Range(0, card.StatusCount);

            card.SetStatusValue(parameterIndex, attackValue);
            _currentCardIndex++;
        }
    }
}
