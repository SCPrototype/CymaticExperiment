using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VR_Object : MonoBehaviour
{
    private static float RespawnDelay = 5.0f;

    public Transform RespawnPoint;
    public AudioSource ImpactSound;

    protected bool _isBeingGrabbed = false;
    private bool _isOnSpawn = true;
    private float _droppedTime;
    private float _spawnTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
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
            GetComponent<Rigidbody>().isKinematic = true;
            //Set object back to respawn point.
            transform.position = RespawnPoint.position;
            transform.rotation = RespawnPoint.rotation;
            _isOnSpawn = true;
            //Re-enable velocity.
            GetComponent<Rigidbody>().isKinematic = false;
            _spawnTime = Time.time;

        }
        //If the object is being moved from its spawn position.
        else if (_isOnSpawn && GetComponent<Rigidbody>().velocity.magnitude > 0 && Time.time > _spawnTime + 0.5f)
        {
            Debug.Log("I'm moved");
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
