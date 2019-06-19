using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class SandSpawner : VR_Object
{
    public GameObject SandPrefab;
    private FMODUnity.StudioEventEmitter _sandPourSoundEmitter;
    private FMODUnity.StudioEventEmitter _sandShakeSoundEmitter;
    private FMODUnity.StudioEventEmitter _sandJarPickUpSoundEmitter;

    private int amountOfSand = 30;
    private Vector3 startingScale;
    private Vector3 localScale;
    private Rigidbody _rigidBody;
    private Vector3 velocity;
    private Vector3 prevPos;
    private float _shakeSensitivity = 1.5f;
    private Tutorial _tutorial;
    private bool _sandPourPlaying = false;
    private float fadeStartTime;
    private float audioStartVolume = 1;
    private float FadeTime = 0.5f;
    private float sandPourVolume = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startingScale = transform.localScale / 100;
        _tutorial = GameObject.Find("LightHolders").GetComponent<Tutorial>();

        _sandPourSoundEmitter = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _sandShakeSoundEmitter = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _sandJarPickUpSoundEmitter = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();

        _sandPourSoundEmitter.Event = GLOB.JarPourSandSound;
        _sandShakeSoundEmitter.Event = GLOB.JarShakeSound;
        _sandJarPickUpSoundEmitter.Event = GLOB.JarPickUpSound;

        _sandPourSoundEmitter.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        _sandShakeSoundEmitter.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        _sandJarPickUpSoundEmitter.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
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
                if(!_sandShakeSoundEmitter.IsPlaying())
                {
                    _sandShakeSoundEmitter.Play();
                }
            }
        }
        if (this.transform.worldToLocalMatrix[1, 1] < 0 && _isBeingGrabbed == true)
        {
            SpawnSand();
            fadeStartTime = Time.time;
        }
        else if (_sandPourSoundEmitter.IsPlaying())
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
        if (!_sandPourSoundEmitter.IsPlaying())
        {
            _sandPourSoundEmitter.Play();
        }
    }

    protected override void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        base.ObjectGrabbed(sender, e);
        _sandJarPickUpSoundEmitter.Play();
        _tutorial.CompleteStage(1);
    }

    private void FadeOutSandPour()
    {
        _sandPourSoundEmitter.EventInstance.getVolume(out sandPourVolume, out float finalvolume);
        if (sandPourVolume > 0)
        {
            _sandPourSoundEmitter.EventInstance.setVolume(audioStartVolume * (1 - ((Time.time - fadeStartTime) / FadeTime))); 
        }
        else
        {
            _sandPourSoundEmitter.Stop();
            _sandPourPlaying = false;
        }
    }
}
 