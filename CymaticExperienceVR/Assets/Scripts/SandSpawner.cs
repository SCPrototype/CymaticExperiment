using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class SandSpawner : VR_Object
{
    public GameObject SandPrefab;
    private AudioSource SandPourAudio;
    private int amountOfSand = 30;
    private Vector3 startingScale;
    private Vector3 localScale;
    private Rigidbody _rigidBody;
    private Vector3 velocity;
    private Vector3 prevPos;
    private float _shakeSensitivity = 1.5f;
    private FMOD.Studio.EventInstance _sandShakeSound;
    private FMOD.Studio.EventInstance _jarPickUpSound;
    private FMOD.Studio.PLAYBACK_STATE _playBackStateShake;
    private Tutorial _tutorial;
    private bool _sandPourPlaying = false;
    private float fadeStartTime;
    private float audioStartVolume = 1;
    private float FadeTime = 0.5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startingScale = transform.localScale / 100;
        _sandShakeSound = FMODUnity.RuntimeManager.CreateInstance(GLOB.JarShakeSound);
        _jarPickUpSound = FMODUnity.RuntimeManager.CreateInstance(GLOB.JarPickUpSound);
        _tutorial = GameObject.Find("LightHolders").GetComponent<Tutorial>();
        SandPourAudio = this.gameObject.AddComponent<AudioSource>();
        AudioClip audioClip = Resources.Load<AudioClip>(GLOB.SandPourSoundPath);
        SandPourAudio.clip = audioClip;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        _sandShakeSound.getPlaybackState(out _playBackStateShake);
        //_pourSandSound.getPlaybackState(out _playBackStatePour);
        if (_isBeingGrabbed)
        {
            float fwdDotProduct = Vector3.Dot(transform.forward, velocity);
            float upDotProduct = Vector3.Dot(transform.up, velocity);
            float rightDotProduct = Vector3.Dot(transform.right, velocity);

            Vector3 velocityVector = new Vector3(rightDotProduct, upDotProduct, fwdDotProduct);
            if (velocity.magnitude > _shakeSensitivity)
            {
                if (_playBackStateShake != FMOD.Studio.PLAYBACK_STATE.PLAYING && _playBackStateShake != FMOD.Studio.PLAYBACK_STATE.STARTING)
                {
                    _sandShakeSound.start();
                }
            }
        }
        if (this.transform.worldToLocalMatrix[1, 1] < 0 && _isBeingGrabbed == true)
        {
            SpawnSand();
            fadeStartTime = Time.time;
        }
        else if (SandPourAudio.isPlaying)
        {
            FadeOutSandPour();
        }
    }

    private void FixedUpdate()
    {
        if (_isBeingGrabbed)
        {
            velocity = (transform.position - prevPos) / Time.deltaTime;
            prevPos = transform.position;
        }
    }

    void SpawnSand()
    {
        SandPourSound();
        for (int i = 0; i < amountOfSand; i++)
        {
            float randomX = UnityEngine.Random.Range(-startingScale.x * startingScale.x, startingScale.x * startingScale.x);
            float randomZ = UnityEngine.Random.Range(-startingScale.z * startingScale.z, startingScale.z * startingScale.z);

            Vector2 vec2 = new Vector2(randomX, randomZ);
            if (vec2.magnitude <= Math.Min(startingScale.x * startingScale.x, startingScale.z * startingScale.z))
            {

                GameObject sand1 = Instantiate(SandPrefab, this.gameObject.transform);
                sand1.transform.localPosition = new Vector3(vec2.x, .01f, vec2.y);
                sand1.transform.localScale = startingScale / 50;
                sand1.transform.SetParent(null);
                GameObject sand2 = Instantiate(SandPrefab, this.gameObject.transform);
                sand2.transform.localPosition = new Vector3(-vec2.x, .01f, -vec2.y);
                sand2.transform.localScale = startingScale / 50;
                sand2.transform.SetParent(null);
            }
            break;
        }
    }

    private void SandPourSound()
    {
        if (!SandPourAudio.isPlaying)
        {
            SandPourAudio.volume = 1;
            _sandPourPlaying = true;
            SandPourAudio.Play();
        }
        //if (_playBackStatePour != FMOD.Studio.PLAYBACK_STATE.PLAYING && _playBackStatePour != FMOD.Studio.PLAYBACK_STATE.STARTING)
        //{
        //    _pourSandSound.start();
        //    _pourSandSound.getPlaybackState(out _playBackStatePour);
        //    FMODUnity.RuntimeManager.PlayOneShot(GLOB.JarPourSandSound, GetComponent<Transform>().position);
        //}
    }

    protected override void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        base.ObjectGrabbed(sender, e);
        _jarPickUpSound.start();
        _tutorial.CompleteStage(1);
    }

    //private void ObjectReleased(object sender, InteractableObjectEventArgs e)
    //{
    //    Debug.Log("I'm Dropped");
    //    _droppedTime = Time.time;
    //    _isBeingGrabbed = false;
    //}

    private void FadeOutSandPour()
    {
        if (SandPourAudio.volume > 0)
        {
            SandPourAudio.volume = audioStartVolume * (1 - ((Time.time - fadeStartTime) / FadeTime));
        }
        else
        {
            SandPourAudio.Stop();
            _sandPourPlaying = false;
        }

    }
}