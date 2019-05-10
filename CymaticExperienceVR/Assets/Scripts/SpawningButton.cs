using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SpawningButton : MonoBehaviour
{
    public VRTK_ControllerEvents controllerEvents;
    public GameObject spawningPoint;
    public GameObject sandBucket;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBucket()
    {
        //controllerEvents.buttonOnePressed += VRTK_ControllerEvents
        Debug.Log("Spawning bucket");
        Instantiate(sandBucket, this.transform.position, this.transform.rotation);
    }
}
