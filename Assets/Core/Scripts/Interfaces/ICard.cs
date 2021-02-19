using System;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Interfaces
{
    public interface ICard
    {
        int StatusCount { get; }
        GameObject GameObject { get; }
        Vector3 StackPosition { get; }
        Quaternion StackRotation { get; }

        event EventHandler<int> HealthChanged;

        void SetData(CardData data);
        void SetStatusValue(int index, int value);
        void SetStackAngle(float angle);
        void SetStackPosition(Vector3 position);
        void SetParent(Transform parent);
    }
}
