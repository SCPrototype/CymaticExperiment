namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using VRTK.Controllables;

    public class ControllableReactor : MonoBehaviour
    {
        public VRTK_BaseControllable controllable;
        public VRTK_ControllerEvents controllerEvents;

        public UnityEvent OnActivate;
        public UnityEvent OnUpdate;
        public UnityEvent OnReset;

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            controllable.ValueChanged += ValueChanged;
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            OnUpdate.Invoke(); 
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