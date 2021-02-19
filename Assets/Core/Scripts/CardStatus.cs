using System;
using Core.Enums;
using UnityEngine;

namespace Core
{
    public abstract class CardStatus : MonoBehaviour
    {
        [SerializeField] private CardStats _status;
        [SerializeField] private int _minValue;

        protected int _value;
        protected int _displayValue;

        public CardStats Status => _status;
        public int MinValue => _minValue;

        public int Value
        {
            get => _value;
            protected set
            {
                if (!CanSet(value))
                {
                    return;
                }

                _value = value;
                SetDisplayValue(value);
                RaiseValueChanged(value);
            }
        }

        public event EventHandler<int> ValueChanged;

        public virtual bool SetValue(int value)
        {
            if (!CanSet(value))
            {
                return false;
            }

            Value = value;
            return true;
        }

        protected bool CanSet(int value)
        {
            return _value != value;
        }

        protected virtual void SetDisplayValue(int value)
        {
            _displayValue = value;
        }

        protected void RaiseValueChanged(int value)
        {
            ValueChanged?.Invoke(this, value);
        }
    }
}
