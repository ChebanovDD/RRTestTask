using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ViewModels
{
    public class GameBoardViewModel : MonoBehaviour
    {
        [SerializeField] private Button _randomAttackButton;
        
        public event EventHandler RandomAttack;

        public void RandomAttack_Click()
        {
            RandomAttack?.Invoke(this, EventArgs.Empty);
        }

        public void SetRandomAttackButtonActive(bool value)
        {
            _randomAttackButton.interactable = value;
        }
    }
}
