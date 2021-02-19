using System;
using Core.Models;
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

        event EventHandler<CardStatus> StatusValueChanged;

        void SetData(CardData data);
        bool SetStatusValue(int index, int value);
        void SetParent(Transform parent);
        void SetTransform(CardTransform cardTransform);
    }
}
