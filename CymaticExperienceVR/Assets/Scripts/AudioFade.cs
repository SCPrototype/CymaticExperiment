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
    private float audioStartVolume = 1;
    public int MaxAmplitude = 12;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetFrequency(int pFrequency)
    {
        AudioToFade.pitch = (Chladni.frameNrArray[pFrequency] / 261.667f); //261.667f is the number that converts frequency to unity pitch.
        //261.667f frequency = unity pitch 1.
    }
    public void SetAmplitude(int pAmplitude)
    {
        audioStartVolume = (float)pAmplitude / MaxAmplitude;
    }

    public void PlayAudio()
    {
        if (!audioIsPlaying)
        {
            AudioToFade.volume = audioStartVolume;
            audioIsPlaying = true;
            fadeStartTime = Time.time;
            AudioToFade.Play();
        }
        else
        {
            AudioToFade.volume = audioStartVolume;
            fadeStartTime = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (audioIsPlaying)
        {
            if (AudioToFade.volume > 0)
            {
                AudioToFade.volume = audioStartVolume * (1 - ((Time.time - fadeStartTime) / FadeTime));
            }
            else
            {
                AudioToFade.Stop();
                audioIsPlaying = false;
            }
        }
    }
}
