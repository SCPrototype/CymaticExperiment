using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class SandSpawner : VR_Object
{
    public GameObject SandPrefab;
    private FMOD.Studio.EventInstance _sandShakeSound;
    private AudioSource _sandPourAudio;
    private AudioSource _sandShakeAudio;
    private AudioSource _sandShakeAudio2;
    private AudioSource _sandShakeAudio3;
    
    private int amountOfSand = 30;
    private Vector3 startingScale;
    private Vector3 localScale;
    private Rigidbody _rigidBody;
    private Vector3 velocity;
    private Vector3 prevPos;
    private float _shakeSensitivity = 1.5f;
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
        _tutorial = GameObject.Find("LightHolders").GetComponent<Tutorial>();

        _sandPourAudio = this.gameObject.AddComponent<AudioSource>();
        _sandShakeAudio = this.gameObject.AddComponent<AudioSource>();
        _sandShakeAudio2 = this.gameObject.AddComponent<AudioSource>();
        _sandShakeAudio3 = this.gameObject.AddComponent<AudioSource>();

        AudioClip audioSandPour = Resources.Load<AudioClip>(GLOB.SandPourSoundPath);
        AudioClip audioShake = Resources.Load<AudioClip>(GLOB.SandShakeSoundPath);
        AudioClip audioShake2 = Resources.Load<AudioClip>(GLOB.SandShakeSoundPath2);
        AudioClip audioShake3 = Resources.Load<AudioClip>(GLOB.SandShakeSoundPath3);

        _sandPourAudio.clip = audioSandPour;
        _sandShakeAudio.clip = audioShake;
        _sandShakeAudio2.clip = audioShake2;
        _sandShakeAudio3.clip = audioShake3;

        _sandPourAudio.spatialBlend = 1;
        _sandShakeAudio.spatialBlend = 1;
        _sandShakeAudio2.spatialBlend = 1;
        _sandShakeAudio3.spatialBlend = 1;
   
        _jarPickUpSound = FMODUnity.RuntimeManager.CreateInstance(GLOB.JarPickUpSound);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (_isBeingGrabbed)
        {
            float fwdDotProduct = Vector3.Dot(transform.forward, velocity);
            float upDotProduct = Vector3.Dot(transform.up, velocity);
            float rightDotProduct = Vector3.Dot(transform.right, velocity);

            Vector3 velocityVector = new Vector3(rightDotProduct, upDotProduct, fwdDotProduct);
            if (velocity.magnitude > _shakeSensitivity)
            {
                if(!_sandShakeAudio.isPlaying && !_sandShakeAudio2.isPlaying && !_sandShakeAudio3.isPlaying)
                {
                    switch(UnityEngine.Random.Range(0,2))
                    {
                        case 0:
                            _sandShakeAudio.Play();
                                break;
                        case 1:
                            _sandShakeAudio2.Play();
                            break;
                        case 2:
                            _sandShakeAudio3.Play();
                            break;
                    }
                }
            }
        }
        if (this.transform.worldToLocalMatrix[1, 1] < 0 && _isBeingGrabbed == true)
        {
            SpawnSand();
            fadeStartTime = Time.time;
        }
        else if (_sandPourAudio.isPlaying)
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
        if (!_sandPourAudio.isPlaying)
        {
            _sandPourAudio.volume = 1;
            _sandPourPlaying = true;
            _sandPourAudio.Play();
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
        if (_sandPourAudio.volume > 0)
        {
            _sandPourAudio.volume = audioStartVolume * (1 - ((Time.time - fadeStartTime) / FadeTime));
        }
        else
        {
            _sandPourAudio.Stop();
            _sandPourPlaying = false;
        }

    }
}