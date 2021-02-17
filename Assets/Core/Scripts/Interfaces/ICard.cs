using Core.Models;
using UnityEngine;

namespace Core.Interfaces
{
    public interface ICard
    {
        void SetModel(CardModel model);
        void SetAngle(float angle);
        void SetPosition(Vector3 position);
        void SetParent(Transform parent);
    }
}
