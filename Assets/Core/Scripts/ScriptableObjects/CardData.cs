using System;
using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCardData", menuName = "Card Data")]
    public class CardData : ScriptableObject, IDisposable
    {
        [Range(0, 9)] public int Mana;
        [Range(0, 9)] public int Attack;
        [Range(0, 9)] public int Health;

        public string Name;
        [TextArea] public string Description;

        public Texture Image;

        public void Dispose()
        {
            Image = null;
        }
    }
}
