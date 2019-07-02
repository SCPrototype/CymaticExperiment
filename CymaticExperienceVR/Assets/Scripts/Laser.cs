using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{
    private Vector3 StartRotation;
    public Vector3 EndRotation;
    public float RotationSpeed;
    private bool shouldDoRotation = false;
    public bool PlayOnAwake = false;
    public UnityEvent OnFinished;
    private FMODUnity.StudioEventEmitter _resetBeamSound;
    public GameObject soundEmitter;

    // Start is called before the first frame update
    void Start()
    {
        StartRotation = transform.localEulerAngles;
        if (PlayOnAwake)
        {
            DoScan();
        } else
        {
            handleEndOfScan(false);
        }
       
        if(soundEmitter != null)
        {
            _resetBeamSound = soundEmitter.AddComponent<FMODUnity.StudioEventEmitter>();
            _resetBeamSound.Event = GLOB.LaserSound;
            _resetBeamSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(soundEmitter.transform));
        } else
        {
            _resetBeamSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
            _resetBeamSound.Event = GLOB.LaserSound;
            _resetBeamSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        }
        
    }

    public void DoScan()
    {
        transform.localEulerAngles = StartRotation;
        gameObject.SetActive(true);
        shouldDoRotation = true;
        _resetBeamSound.Play();
    }
    private void handleEndOfScan(bool pRealEnd)
    {
        gameObject.SetActive(false);
        shouldDoRotation = false;

        if (pRealEnd)
        {
            OnFinished.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldDoRotation)
        {
            Vector3 tempRotation = EndRotation;
            if (EndRotation.x < 0)
            {
                tempRotation.x += 360;
            }
            if (EndRotation.y < 0)
            {
                tempRotation.y += 360;
            }
            if (EndRotation.z < 0)
            {
                tempRotation.z += 360;
            }
            if (((tempRotation - transform.localEulerAngles).magnitude) >= ((EndRotation - StartRotation) * RotationSpeed).magnitude)
            {
                transform.localEulerAngles += (EndRotation - StartRotation) * RotationSpeed;
            }
            else
            {
                handleEndOfScan(true);
            }
        }
    }
}
