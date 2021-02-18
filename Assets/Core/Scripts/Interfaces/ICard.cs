using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Interfaces
{
    public interface ICard
    {
        void SetData(CardData data);
        void SetMana(int value);
        void SetAngle(float angle);
        void SetPosition(Vector3 position);
        void SetParent(Transform parent);
    }
}
