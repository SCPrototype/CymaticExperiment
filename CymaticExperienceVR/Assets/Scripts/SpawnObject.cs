using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject spawningPoint;
    public GameObject targetObject;

    private bool _objectSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DoSpawnObject()
    {
        if (_objectSpawned == false)
        {
            GameObject.Instantiate(targetObject, spawningPoint.transform.position, spawningPoint.transform.rotation);
            _objectSpawned = true;
        }
    }

    public void SetReadyToSpawnObject()
    {
        _objectSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
