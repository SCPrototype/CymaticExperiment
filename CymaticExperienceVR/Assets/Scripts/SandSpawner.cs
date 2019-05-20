using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class SandSpawner : VR_Object
{
    public GameObject SandPrefab;
    public AudioSource SandAudio;

    private int amountOfSand = 30;

    private Vector3 startingScale;
    private Vector3 localScale;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //Add event listener for interactable object.
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectReleased);
        if (GetComponent<VRTK_InteractableObject>() == null)
        {
            Debug.LogError("Team3_Interactable_Object_Extension is required to be attached to an Object that has the VRTK_InteractableObject script attached to it");
            return;
        }

        startingScale = transform.localScale / 10;
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (this.transform.worldToLocalMatrix[1, 1] < 0 && _isBeingGrabbed == true)
        {
            SpawnSand();
        }
        else if (SandAudio.isPlaying)
        {
            SandAudio.Stop();
        }
    }

    void SpawnSand()
    {
        for (int i = 0; i < amountOfSand; i++)
        {   
            float randomX = UnityEngine.Random.Range(-startingScale.x * startingScale.x, startingScale.x * startingScale.x);
            float randomZ = UnityEngine.Random.Range(-startingScale.z * startingScale.z, startingScale.z * startingScale.z);
            
            Vector2 vec2 = new Vector2(randomX, randomZ);
            if(vec2.magnitude <= Math.Min(startingScale.x * startingScale.x, startingScale.z * startingScale.z))
            {
                GameObject sand1 = Instantiate(SandPrefab, this.gameObject.transform);
                sand1.transform.localPosition = new Vector3(vec2.x, .15f, vec2.y);
                sand1.transform.SetParent(null);
                GameObject sand2 = Instantiate(SandPrefab, this.gameObject.transform);
                sand2.transform.localPosition = new Vector3(-vec2.x, .15f, -vec2.y);
                sand2.transform.SetParent(null);
            }
            break;
        }
        if (!SandAudio.isPlaying)
        {
            SandAudio.Play();
        }
    }

    //private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    //{
    //    Debug.Log("Im Grabbed");
    //    _isBeingGrabbed = true;
    //    _isOnSpawn = false;
    //}

    //private void ObjectReleased(object sender, InteractableObjectEventArgs e)
    //{
    //    Debug.Log("I'm Dropped");
    //    _droppedTime = Time.time;
    //    _isBeingGrabbed = false;
    //}
}
