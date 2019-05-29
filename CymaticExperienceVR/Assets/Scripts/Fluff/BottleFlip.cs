using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BottleFlip : VR_Object
{
    public AudioSource CelebrationSound;
    public ParticleSystem PartSystem;

    public float SuccesfullLandDelay;
    private float _landTime;

    private bool _landedSuccesfully = false;
    private bool _readyForSuccess = false;
    private bool _grounded = false;

    private Vector3 targetCenterOfMass;
    private Vector3 previousMassChange = new Vector3(0, 0, 0);
    public Vector3 CenterOfMassDistance = new Vector3(0.05f, 0.1f, 0.05f);
    [Range(0.0f, 1.0f)]
    public float MassShiftSpeed = 0.3f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!_isBeingGrabbed && !_isOnSpawn) //If the bottle has been thrown away by the player.
        {
            targetCenterOfMass = -transform.worldToLocalMatrix.GetRow(1).normalized;
            targetCenterOfMass.Scale(CenterOfMassDistance);

            if (rb.centerOfMass != targetCenterOfMass)
            {
                Vector3 massHolder = rb.centerOfMass;
                rb.centerOfMass = Vector3.Lerp(rb.centerOfMass, targetCenterOfMass, MassShiftSpeed) + (previousMassChange * (1 - MassShiftSpeed));
                previousMassChange = rb.centerOfMass - massHolder;
            }

            
            if (_readyForSuccess) //If the bottle has been upside down.
            {
                if (transform.worldToLocalMatrix.GetRow(1).normalized[1] > 0.75f && rb.velocity.magnitude <= 0.1f)// If the bottle is up right and no longer moving.
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
                            PartSystem.Play();
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
            rb.ResetCenterOfMass();
            _readyForSuccess = false;
            _grounded = false;
            _landedSuccesfully = false;
        }
    }
}
