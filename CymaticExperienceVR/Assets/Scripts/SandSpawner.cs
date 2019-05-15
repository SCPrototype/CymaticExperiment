using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class SandSpawner : MonoBehaviour
{
    public GameObject SandPrefab;

    private GameObject spawningPlate;
    private Chladni _chladniScript;
    private Vector3 _spawnPoint;
    private Vector3 widthHeight;
    private int amountOfSand = 30;
    private bool _isBeingGrabbed = false;
    private Vector3 startingScale;
    // Start is called before the first frame update
    void Start()
    {
        widthHeight = new Vector3(this.GetComponent<Renderer>().bounds.size.x, 0, this.GetComponent<Renderer>().bounds.size.z);
        //Add event listener for interactable object.
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectReleased);
        spawningPlate = GameObject.Find("SpawningPlate");
        if (GetComponent<VRTK_InteractableObject>() == null)
        {
            Debug.LogError("Team3_Interactable_Object_Extension is required to be attached to an Object that has the VRTK_InteractableObject script attached to it");
            return;
        }
        if(_chladniScript == null)
        {
            _chladniScript = GameObject.Find("TableHolder").GetComponent<Chladni>();
        }
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.worldToLocalMatrix[1, 1] < 0 && _isBeingGrabbed == true)
        {
            _spawnPoint = (transform.position + (transform.up * transform.lossyScale.y));
            SpawnSand();
        }
    }

    void SpawnSand()
    {
        for (int i = 0; i < amountOfSand; i++)
        {   
            
            float randomX = UnityEngine.Random.Range(-startingScale.x * transform.localScale.x, startingScale.x *transform.localScale.x);
            float randomZ = UnityEngine.Random.Range(-startingScale.z * transform.localScale.z, startingScale.z * transform.localScale.z);
            
            Vector2 vec2 = new Vector2(randomX, randomZ);
            if(vec2.magnitude <= Math.Min(startingScale.x * transform.localScale.x, startingScale.z * transform.localScale.z))
            {
                GameObject sand = Instantiate(SandPrefab, this.gameObject.transform);
                sand.transform.localPosition = new Vector3(vec2.x, 1, vec2.y);
                //Debug.Log(startingScale.y);
            }
            break;
            //Vector3 offSet = new Vector3(0.02f * i, 0, 0.02f * i);
            //sand.transform.position = (_spawnPoint - (widthHeight / 2)) + offSet;
        }
    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("Im Grabbed");
        _isBeingGrabbed = true;
    }

    private void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("I'm Dropped");
        _isBeingGrabbed = false;
    }
}
