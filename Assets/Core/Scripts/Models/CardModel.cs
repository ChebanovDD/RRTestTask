using UnityEngine;

namespace Core.Models
{
    public class CardModel
    {
        public int Mana { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Texture Image { get; set; }
    }
}