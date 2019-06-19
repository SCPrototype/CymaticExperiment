using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SliderSoundHandler : VR_Object
{
    private FMODUnity.StudioEventEmitter _sliderSoundSoundEmitter;
    private bool _soundPlayed = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _sliderSoundSoundEmitter = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _sliderSoundSoundEmitter.Event = GLOB.TouchingSliderSound;
        _sliderSoundSoundEmitter.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
    }

    protected override void Update()
    {

    }

    protected override void HandleRespawn()
    {
        
    }

    protected override void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        base.ObjectGrabbed(sender, e);
        if (!_sliderSoundSoundEmitter.IsPlaying())
        {
            _sliderSoundSoundEmitter.Play();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }

    protected override void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {

    }
}
