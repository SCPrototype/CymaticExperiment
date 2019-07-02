using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerHandler : MonoBehaviour
{
    public Camera gameObjectToFollow;
    public Camera gameObjectToFollow2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObjectToFollow.enabled)
        {
            this.gameObject.transform.position = gameObjectToFollow.transform.position;
            this.gameObject.transform.rotation = gameObjectToFollow.transform.rotation;
        } 
        if(gameObjectToFollow2)
        {
            this.gameObject.transform.position = gameObjectToFollow2.transform.position;
            this.gameObject.transform.rotation = gameObjectToFollow2.transform.rotation;
        }
    }
}
