using UnityEngine;

namespace Core.Models
{
    public class CardTransform
    {
        public CardTransform(float angle, Vector3 position)
        {
            Angle = angle;
            Position = position;
        }

        public float Angle { get; }
        public Vector3 Position { get; }
    }
}