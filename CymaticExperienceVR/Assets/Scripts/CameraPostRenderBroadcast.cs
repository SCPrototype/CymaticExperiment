using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class CameraPostRenderBroadcast : MonoBehaviour
{
    public UnityEvent Broadcast;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPostRender()
    {
        Broadcast.Invoke();
    }
}
