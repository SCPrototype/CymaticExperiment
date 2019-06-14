using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSoundHandler : MonoBehaviour
{
    private FMOD.Studio.EventInstance _sliderSound;
    private bool _soundPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        _sliderSound = FMODUnity.RuntimeManager.CreateInstance(GLOB.TouchingSliderSound);
        _sliderSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
    }

    // Update is called once per frame
    void Update()
    {
        if (!_soundPlayed)
        {
            if (this.gameObject.transform.childCount > 1 || Input.GetKeyDown(KeyCode.Q))
            {
                _soundPlayed = true;
                _sliderSound.start();
            } 
        }
        if(this.gameObject.transform.childCount <= 1 && _soundPlayed == true)
        {
            _soundPlayed = false;
        }
    }
}
