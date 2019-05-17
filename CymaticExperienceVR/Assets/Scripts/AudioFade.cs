using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class AudioFade : MonoBehaviour
{
    public float FadeTime;

    public AudioSource AudioToFade;
    private bool audioIsPlaying = false;
    private float fadeStartTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetFrequency(int pFrequency)
    {
        AudioToFade.pitch = pFrequency * 0.1f;
    }

    public void PlayAudio()
    {
        if (!audioIsPlaying)
        {
            AudioToFade.volume = 1;
            audioIsPlaying = true;
            fadeStartTime = Time.time;
            AudioToFade.Play();
        }
        else
        {
            AudioToFade.volume = 1;
            fadeStartTime = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioIsPlaying)
        {
            if (AudioToFade.isPlaying)
            {
                AudioToFade.volume = 1;
                audioIsPlaying = true;
                fadeStartTime = Time.time;
            }
        }
        else if (AudioToFade.volume > 0)
        {
            AudioToFade.volume = 1 - ((Time.time - fadeStartTime) / FadeTime);
        }
        else
        {
            AudioToFade.Stop();
            audioIsPlaying = false;
        }
    }
}
