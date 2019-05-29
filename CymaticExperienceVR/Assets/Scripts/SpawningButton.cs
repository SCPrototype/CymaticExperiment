using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SpawningButton : MonoBehaviour
{
    public VRTK_ControllerEvents controllerEvents;
    public GameObject spawningPoint;
    public GameObject sandBucket;
    private bool _bucketSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBucket()
    {
        if (_bucketSpawned == false)
        {
            GameObject.Instantiate(sandBucket, spawningPoint.transform.position, spawningPoint.transform.rotation);
            _bucketSpawned = true;
        }
    }

    public void EnableSpawnBucket()
    {
        _bucketSpawned = false;
    }
}
