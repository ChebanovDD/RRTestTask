using System;
using UnityEngine;

namespace Core.ViewModels
{
    public class GameBoardViewModel : MonoBehaviour
    {
        public event EventHandler RandomAttack;

        public void RandomAttack_Click()
        {
            RandomAttack?.Invoke(this, EventArgs.Empty);
        }
    }
}
