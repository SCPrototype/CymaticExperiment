using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VR_Object : MonoBehaviour
{
    private static float RespawnDelay = 30.0f;

    public Transform RespawnPoint;
    public AudioSource ImpactSound;

    protected bool _isBeingGrabbed = false;
    private bool _isOnSpawn = true;
    private float _droppedTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //If the object has been moved, and is not currently being held, and the respawn delay has elapsed.
        if (!_isBeingGrabbed && !_isOnSpawn && Time.time - _droppedTime >= RespawnDelay)
        {
            //Disable velocity.
            GetComponent<Rigidbody>().isKinematic = true;
            //Set object back to respawn point.
            transform.position = RespawnPoint.position;
            transform.rotation = RespawnPoint.rotation;
            _isOnSpawn = true;
            //Re-enable velocity.
            GetComponent<Rigidbody>().isKinematic = false;
        }
        //If the object is being moved from its spawn position.
        else if (_isOnSpawn && GetComponent<Rigidbody>().velocity.magnitude > 0)
        {
            _droppedTime = Time.time;
            _isOnSpawn = false;
        }
    }

    protected virtual void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("Im Grabbed");
        _isBeingGrabbed = true;
        _isOnSpawn = false;
    }

    protected virtual void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("I'm Dropped");
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
