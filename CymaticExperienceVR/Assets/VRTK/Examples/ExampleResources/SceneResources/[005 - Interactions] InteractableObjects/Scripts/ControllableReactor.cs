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

        private bool _bucketSpawned = false;

        public UnityEvent OnActivate;
        public UnityEvent OnReset;
        public ValueChangedEvent OnValueChanged;

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            controllable.ValueChanged += ValueChanged;
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            OnValueChanged.Invoke((int)e.value);
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