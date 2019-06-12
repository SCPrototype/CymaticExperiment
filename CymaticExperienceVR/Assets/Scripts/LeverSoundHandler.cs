using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSoundHandler : MonoBehaviour
{
    private FMOD.Studio.EventInstance _leverClickSound;
    private FMOD.Studio.PLAYBACK_STATE _leverClickPlaybackState;

    // Start is called before the first frame update
    void Start()
    {
        _leverClickSound = FMODUnity.RuntimeManager.CreateInstance(GLOB.GeneralPickupSound);
        _leverClickSound.getPlaybackState(out _leverClickPlaybackState);
    }

    // Update is called once per frame
    void Update()
    {
        _leverClickSound.getPlaybackState(out _leverClickPlaybackState);
    }

    public void PlayClickingSound()
    {
        if(_leverClickPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING && _leverClickPlaybackState != FMOD.Studio.PLAYBACK_STATE.STARTING)
        {
            _leverClickSound.start();
            FMODUnity.RuntimeManager.PlayOneShot(GLOB.GeneralPickupSound, GetComponent<Transform>().position);
        }
    }

    public void PlayEndClickSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(GLOB.LeverSound, GetComponent<Transform>().position);
    }
}
