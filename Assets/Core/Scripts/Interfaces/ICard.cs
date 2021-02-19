using System;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Interfaces
{
    public interface ICard
    {
        int CardParametersCount { get; }
        GameObject GameObject { get; }

        event EventHandler<int> HealthChanged;

        void SetData(CardData data);
        void SetParameterValue(int index, int value);
        void SetAngle(float angle);
        void SetPosition(Vector3 position);
        void SetParent(Transform parent);
    }
}
