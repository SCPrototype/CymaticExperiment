using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class SandSpawner : VR_Object
{
    public GameObject SandPrefab;
    public AudioSource SandAudio;
    public AudioSource SandShakeAudio;

    private int amountOfSand = 30;

    private Vector3 startingScale;
    private Vector3 localScale;
    private Rigidbody _rigidBody;

    private Vector3 velocity;
    private Vector3 prevPos;
    private float _shakeSensitivity = 1.5f;
    private FMOD.Studio.EventInstance ShakeSand;
    private Tutorial _tutorial;
    private FMOD.Studio.PLAYBACK_STATE _playBackState;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startingScale = transform.localScale / 100;
        ShakeSand = FMODUnity.RuntimeManager.CreateInstance(GLOB.JarPourSandSound);
        _tutorial = GameObject.Find("LightHolders").GetComponent<Tutorial>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        ShakeSand.getPlaybackState(out _playBackState);
        if (_isBeingGrabbed)
        {
            float fwdDotProduct = Vector3.Dot(transform.forward, velocity);
            float upDotProduct = Vector3.Dot(transform.up, velocity);
            float rightDotProduct = Vector3.Dot(transform.right, velocity);

            Vector3 velocityVector = new Vector3(rightDotProduct, upDotProduct, fwdDotProduct);
            if (velocity.magnitude > _shakeSensitivity)
            {
                //Debug.DrawRay(transform.position, velocity * 10, Color.yellow, 10, false);
                if (SandShakeAudio != null)
                {
                    if (!SandShakeAudio.isPlaying)
                    {
                        Debug.Log("Playing SandShake");
                        SandShakeAudio.Play();
                    }
                }
            }
        }
        if (this.transform.worldToLocalMatrix[1, 1] < 0 && _isBeingGrabbed == true)
        {
            SpawnSand();
        }
        else if (_playBackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            Debug.Log("Stopping audio");
            //ShakeSand.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ShakeSand.release();
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
        ShakeSand.getPlaybackState(out _playBackState);
        if (_playBackState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            ShakeSand.start();
            Debug.Log("Start sound");
        }
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

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        _tutorial.CompleteStage1();
    }

    //private void ObjectReleased(object sender, InteractableObjectEventArgs e)
    //{
    //    Debug.Log("I'm Dropped");
    //    _droppedTime = Time.time;
    //    _isBeingGrabbed = false;
    //}
}
