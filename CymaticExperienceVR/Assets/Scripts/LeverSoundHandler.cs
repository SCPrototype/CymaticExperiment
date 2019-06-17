using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSoundHandler : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter _leverClickSound;
    private FMODUnity.StudioEventEmitter _leverEndClickSound;
    private FMODUnity.StudioEventEmitter _leverResetSound;

    private bool goingBack = false;
    // Start is called before the first frame update
    void Start()
    {
        _leverClickSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _leverEndClickSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _leverResetSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();

        _leverClickSound.Event = GLOB.LeverClickSound;
        _leverEndClickSound.Event = GLOB.LeverSound;
        _leverResetSound.Event = GLOB.LeverReleaseSound;

        _leverClickSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        _leverEndClickSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        _leverResetSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayClickingSound()
    {
        if (!goingBack)
        {
            if (!_leverClickSound.IsPlaying())
            {
                _leverClickSound.Play();
            }
        }
    }

    public void PlayEndClickSound()
    {
        if(!_leverEndClickSound.IsPlaying())
        {
            _leverEndClickSound.Play();
            goingBack = true;
        }
    }

    public void PlayResetSound()
    {
        if(!_leverResetSound.IsPlaying())
        {
            _leverResetSound.Play();
            goingBack = false;
        }
    }
}
