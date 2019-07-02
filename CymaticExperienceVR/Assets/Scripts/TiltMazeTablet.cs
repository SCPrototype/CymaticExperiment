﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TiltMazeTablet : VR_Object
{
    public Transform TabletHolder;

    public Transform TargetTerrain;
    public Vector3 RotationLimit = new Vector3(30, 0, 30);
    public Vector3 RotationOffset = new Vector3(0, 0, 0);

    [Range(0.1f, 1.0f)]
    public float RotationSpeed = 0.1f;


    private Vector3 startRotation;

    public TextMesh scoreText;
    public TextMesh highscoreText;
    private bool setHighScore = false;
    private int highScore = 0;
    private int currentScore = 0;

    public bool IsActiveOnAwake = false;
    public Vector3 ActivePosition = new Vector3(0, 0.1f, 0);
    public Vector3 ActiveRotation = new Vector3(50, 0, 0);
    public Transform LaunchPosition;
    public int LaunchForce = 225;
    public int LaunchOffsetY = 2;
    public float ActivateSpeed = 0.01f;
    private bool IsActive;
    private bool IsActivating = false;
    private FMODUnity.StudioEventEmitter _tabletShootSound;
    private FMODUnity.StudioEventEmitter _tabletstickSound;

    private Tutorial _tutorial;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _tutorial = GameObject.Find("LightHolders").GetComponent<Tutorial>();

        startRotation = TargetTerrain.transform.eulerAngles;

        IsActive = IsActiveOnAwake;
       
        this.SetActive(true);
        _tabletShootSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _tabletstickSound = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();

        _tabletShootSound.Event = GLOB.TabletShootSound;
        _tabletstickSound.Event = GLOB.TabletStickSound;

        _tabletShootSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        _tabletstickSound.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        this.SetActive(false);
        if (!IsActive)
        {
            HandleActiveState(false);
        }
    }
    protected override void HandleRespawn()
    {
        if (!IsActive)
        {
            base.HandleRespawn();
            _tabletShootSound.Play();
            rb.isKinematic = true;
        }
        else
        {
            SetActive(false);
            SetActive(true);
        }
    }

    private void HandleHolderSpawn()
    {
        rb.isKinematic = true;
        //Set object back to respawn point.
        transform.position = TabletHolder.position;
        transform.rotation = TabletHolder.rotation;
        _isOnSpawn = true;
        //Re-enable velocity.
        //rb.isKinematic = false;
        _spawnTime = Time.time;
    }

    public void SetActive(bool pToggle)
    {
        if (pToggle && !IsActive && !IsActivating)
        {
            IsActivating = true;
            transform.position = RespawnPoint.position;
        }
        else if (!pToggle && IsActive)
        {
            IsActivating = false;
            IsActive = false;
            HandleActiveState(false);
        }
    }
    private void HandleActiveState(bool pToggle)
    {
        if (pToggle)
        {
            rb.isKinematic = false;
            //rb.AddExplosionForce(LaunchForce, transform.position + (transform.position - LaunchPosition.position).normalized, 3, LaunchOffsetY, ForceMode.Impulse);
            rb.AddForce(((LaunchPosition.position - transform.position).normalized + new Vector3(0, LaunchOffsetY, 0)) * LaunchForce, ForceMode.Impulse);
            rb.AddRelativeTorque(new Vector3(LaunchForce * -0.1f, 0, 0), ForceMode.Impulse);
            _tabletstickSound.Play();
            _droppedTime = Time.time;
            _isOnSpawn = false;

            GetComponent<VRTK_InteractableObject>().isGrabbable = true;
        }
        else
        {
            HandleRespawn();
            rb.isKinematic = true;
            GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsActivating)
        {
            Vector3 tempRotation = ActiveRotation;
            if (ActiveRotation.x < 0)
            {
                tempRotation.x += 360;
            }
            if (ActiveRotation.y < 0)
            {
                tempRotation.y += 360;
            }
            if (ActiveRotation.z < 0)
            {
                tempRotation.z += 360;
            }
            if ((transform.position - RespawnPoint.position).magnitude < ActivePosition.magnitude)
            {
                transform.position += (ActivePosition * ActivateSpeed);
            }
            else if (false && ((tempRotation - (transform.eulerAngles - RespawnPoint.eulerAngles)).magnitude) >= (ActiveRotation * ActivateSpeed).magnitude)
            {
                transform.eulerAngles += (ActiveRotation * ActivateSpeed);
            }
            else
            {
                IsActivating = false;
                IsActive = true;
                HandleActiveState(true);
            }
        }
        else if (IsActive)
        {
            base.Update();

            if (_isBeingGrabbed)
            {
                Vector3 eulerHolder = transform.eulerAngles + RotationOffset;
                if (eulerHolder.x > 180)
                {
                    eulerHolder.x -= 360;
                }
                if (eulerHolder.z > 180)
                {
                    eulerHolder.z -= 360;
                }

                Vector3 terrainEulerHolder = TargetTerrain.transform.eulerAngles;
                if (terrainEulerHolder.x > 180)
                {
                    terrainEulerHolder.x -= 360;
                }
                if (terrainEulerHolder.z > 180)
                {
                    terrainEulerHolder.z -= 360;
                }

                //TargetTerrain.transform.eulerAngles = new Vector3(Mathf.Clamp(eulerHolder.x, -RotationLimit.x, RotationLimit.x), startRotation.y, Mathf.Clamp(eulerHolder.z, -RotationLimit.z, RotationLimit.z));
                TargetTerrain.transform.eulerAngles = Vector3.Lerp(terrainEulerHolder, new Vector3(Mathf.Clamp(eulerHolder.x, -RotationLimit.x, RotationLimit.x), terrainEulerHolder.y, Mathf.Clamp(eulerHolder.z, -RotationLimit.z, RotationLimit.z)), 0.1f);
            }
            else
            {
                if (!_isOnSpawn)
                {
                    if (TabletHolder.GetComponent<Collider>() != null)
                    {
                        if (TabletHolder.GetComponent<Collider>().bounds.Intersects(GetComponentInChildren<Collider>().bounds))
                        {
                            HandleHolderSpawn();
                        }
                    }
                }

                if (TargetTerrain.transform.eulerAngles != startRotation)
                {
                    Vector3 eulerHolder = TargetTerrain.transform.eulerAngles;
                    if (eulerHolder.x > 180)
                    {
                        eulerHolder.x -= 360;
                    }
                    if (eulerHolder.z > 180)
                    {
                        eulerHolder.z -= 360;
                    }
                    TargetTerrain.transform.eulerAngles = Vector3.Lerp(eulerHolder, startRotation, 0.1f);
                    //TargetTerrain.transform.eulerAngles = startRotation;
                }
            }
        }
    }

    public void AddScore(int pAmount)
    {
        currentScore += pAmount;
        scoreText.text = currentScore.ToString();
        if (currentScore > highScore)
        {
            SetNewHighscore();
        }
    }

    private void SetNewHighscore()
    {
        highScore = currentScore;
        highscoreText.text = highScore.ToString();

        if (!setHighScore)
        {
            //Put any victory effects here

            setHighScore = true;
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        setHighScore = false;
    }

    protected override void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {
        if (GetComponent<VRTK_InteractableObject>().GetGrabbingObject() == null && GetComponent<VRTK_InteractableObject>().GetSecondaryGrabbingObject() == null)
        {
            rb.isKinematic = false;
        }
        //rb.isKinematic = false;

        base.ObjectReleased(sender, e);

        //rb.iskinematic = true;

        if (TabletHolder.GetComponent<Collider>() != null)
        {
            if (TabletHolder.GetComponent<Collider>().bounds.Intersects(GetComponentInChildren<Collider>().bounds))
            {
                HandleHolderSpawn();
            }
        }
    }

    protected override void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        base.ObjectGrabbed(sender, e);
        _tutorial.CompleteStage(6);
    }
}
