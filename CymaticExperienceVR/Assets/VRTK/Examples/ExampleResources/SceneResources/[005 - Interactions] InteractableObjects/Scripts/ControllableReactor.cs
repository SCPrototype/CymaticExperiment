﻿namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using VRTK.Controllables;

    [System.Serializable]
    public class ValueChangedEvent : UnityEvent<int>
    {

    }

    public class ControllableReactor : MonoBehaviour
    {
        public VRTK_BaseControllable controllable;
        public VRTK_ControllerEvents controllerEvents;

        public bool SnapToInt = false;
        private int currentValue = -5000;

        public UnityEvent OnActivate;
        public UnityEvent OnReset;
        public ValueChangedEvent OnValueChanged;

        private float _timer;
        private float _gracePeriod = 0.2f;
        private bool _enabledBool = false;

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            controllable.ValueChanged += ValueChanged;
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
        }

        protected virtual void Start()
        {
            _timer = Time.time;
        }

        protected virtual void Update()
        {
            if (_enabledBool == false)
            {
                if (Time.time > _timer + _gracePeriod)
                {
                    _enabledBool = true;
                }
            }
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            if (_enabledBool)
            {
                if (SnapToInt)
                {
                    if (currentValue != (int)e.value)
                    {
                        OnValueChanged.Invoke((int)e.value);
                        currentValue = (int)e.value;
                    }
                }
                else
                {
                    OnValueChanged.Invoke((int)e.value);
                }
            }
        }

        protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
        {
            if (_enabledBool)
            {
                OnActivate.Invoke();
            }
        }

        protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
        {
            if (_enabledBool)
            {
                OnReset.Invoke();
            }
        }
    }
}