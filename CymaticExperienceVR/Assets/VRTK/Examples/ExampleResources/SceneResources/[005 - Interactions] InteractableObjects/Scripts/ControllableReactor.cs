namespace VRTK.Examples
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
        public UnityEvent OnEnabled;
        public ValueChangedEvent OnValueChanged;

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            controllable.ValueChanged += ValueChanged;
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
            OnEnabled.Invoke();
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
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

        protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
        {
            OnActivate.Invoke();
        }

        protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
        {
            OnReset.Invoke();
        }
    }
}