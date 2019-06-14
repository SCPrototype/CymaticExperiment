using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSoundHandler : MonoBehaviour
{
    public FMOD.Studio.EventInstance _sliderSound;
    private FMODUnity.StudioEventEmitter _eventEmitter;
    private bool _soundPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        _eventEmitter = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _eventEmitter.Event = GLOB.TouchingSliderSound;
        _eventEmitter.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
    }

    // Update is called once per frame
    void Update()
    {
        if (!_soundPlayed)
        {
            if (this.gameObject.transform.childCount > 1 || Input.GetKeyDown(KeyCode.Q))
            {
                _soundPlayed = true;
                _eventEmitter.Play();
            } 
        }
        if(this.gameObject.transform.childCount <= 1 && _soundPlayed == true)
        {
            _soundPlayed = false;
        }
    }
}
