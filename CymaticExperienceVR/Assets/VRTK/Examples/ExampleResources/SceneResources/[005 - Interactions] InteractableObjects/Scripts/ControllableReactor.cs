namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.UI;
    using VRTK.Controllables;

    public class ControllableReactor : MonoBehaviour
    {
        public VRTK_BaseControllable controllable;
        public VRTK_ControllerEvents controllerEvents;
        public GameObject spawningPoint;
        public GameObject sandBucket;

        private bool _bucketSpawned = false;

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            controllable.ValueChanged += ValueChanged;
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {

        }

        protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
        {
            if (_bucketSpawned == false)
            {
                GameObject.Instantiate(sandBucket, spawningPoint.transform.position, spawningPoint.transform.rotation);
                _bucketSpawned = true;
            }
        }

        protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
        {
            _bucketSpawned = false;
            Debug.Log("Reset");
        }
    }
}