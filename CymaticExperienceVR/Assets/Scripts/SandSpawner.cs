using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SandSpawner : MonoBehaviour
{
    public GameObject SandPrefab;

    private Chladni _chladniScript;
    private Vector3 _spawnPoint;
    private Vector3 widthHeight;
    private int amountOfSand = 5;
    private bool _isBeingGrabbed = false;
    // Start is called before the first frame update
    void Start()
    {
        widthHeight = new Vector3(this.GetComponent<Renderer>().bounds.size.x, 0, this.GetComponent<Renderer>().bounds.size.z);
        //Add event listener for interactable object.
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectReleased);
        if (GetComponent<VRTK_InteractableObject>() == null)
        {
            Debug.LogError("Team3_Interactable_Object_Extension is required to be attached to an Object that has the VRTK_InteractableObject script attached to it");
            return;
        }
        if(_chladniScript == null)
        {
            _chladniScript = GameObject.Find("TableHolder").GetComponent<Chladni>();
        }
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
            Vector3 offSet = new Vector3(0.02f * i, 0, 0.02f * i);
            GameObject sand = Instantiate(SandPrefab, _chladniScript.TargetPlane.transform);
            sand.transform.localPosition = (_spawnPoint - (widthHeight / 2)) + offSet;
            _chladniScript.AddSand(sand);
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
