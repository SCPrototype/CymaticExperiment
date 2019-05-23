﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
[RequireComponent(typeof(VRTK_InteractObjectHighlighter))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))] 
public class VR_Object : MonoBehaviour
{
    private static float RespawnDelay = 5.0f;

    public Transform RespawnPoint;
    public AudioSource ImpactSound;

    protected Rigidbody rb;
    protected bool _isBeingGrabbed = false;
    protected bool _isOnSpawn = true;
    private float _droppedTime;
    private float _spawnTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Add event listener for interactable object.
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectReleased);
        if (GetComponent<VRTK_InteractableObject>() == null)
        {
            Debug.LogError("Team3_Interactable_Object_Extension is required to be attached to an Object that has the VRTK_InteractableObject script attached to it");
            return;
        }

        rb = GetComponent<Rigidbody>();

        //Adds the time of the spawning of the object (only listening on movement after X amount) This to patch the non-perfect spawns.
        _spawnTime = Time.time;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //If the object has been moved, and is not currently being held, and the respawn delay has elapsed.
        if (!_isBeingGrabbed && !_isOnSpawn && Time.time - _droppedTime >= RespawnDelay)
        {
            Debug.Log("Respawning item." + _isOnSpawn + " " + _isBeingGrabbed);
            //Disable velocity.
            rb.isKinematic = true;
            //Set object back to respawn point.
            transform.position = RespawnPoint.position;
            transform.rotation = RespawnPoint.rotation;
            _isOnSpawn = true;
            //Re-enable velocity.
            rb.isKinematic = false;
            _spawnTime = Time.time;

        }
        //If the object is being moved from its spawn position.
        else if (_isOnSpawn && rb.velocity.magnitude > 0 && Time.time > _spawnTime + 0.5f)
        {
            _droppedTime = Time.time;
            _isOnSpawn = false;
        }
    }

    protected virtual void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        _isBeingGrabbed = true;
        _isOnSpawn = false;
    }

    protected virtual void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {
        _droppedTime = Time.time;
        _isBeingGrabbed = false;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (ImpactSound != null)
        {
            ImpactSound.Play();
        }
    }
}
