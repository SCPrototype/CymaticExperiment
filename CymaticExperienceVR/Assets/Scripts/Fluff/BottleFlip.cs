using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(ParticleSystem))]
public class BottleFlip : VR_Object
{
    public AudioSource CelebrationSound;
    private ParticleSystem _partSystem;

    public float SuccesfullLandDelay;
    private float _landTime;

    private bool _landedSuccesfully = false;
    private bool _readyForSuccess = false;
    private bool _grounded = false;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _partSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!_isBeingGrabbed && !_isOnSpawn) //If the bottle has been thrown away by the player.
        {
            if (_readyForSuccess) //If the bottle has been upside down.
            {
                if (this.transform.worldToLocalMatrix[1, 1] > 0 && rb.velocity.magnitude <= 0.1f)// If the bottle is up right and no longer moving.
                {
                    if (!_grounded) //Start counting the time the bottle landed correctly.
                    {
                        _landTime = Time.time;
                        _grounded = true;
                    }
                    else if (Time.time - _landTime >= SuccesfullLandDelay) //After a delay, play celebration.
                    {
                        if (!_landedSuccesfully)
                        {
                            _landedSuccesfully = true;
                            _partSystem.Play();
                            CelebrationSound.Play();
                        }
                    }
                } else
                {
                    _grounded = false;
                }
            }
            else if (this.transform.worldToLocalMatrix[1, 1] < 0 && !_readyForSuccess) //If the bottle is facing down after the player dropped it.
            {
                _readyForSuccess = true; //The bottle has been upside down.
            }
        }
        else
        {
            _readyForSuccess = false;
            _grounded = false;
            _landedSuccesfully = false;
        }
    }
}
