using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBall : VR_Object
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        FMODUnity.RuntimeManager.PlayOneShot(GLOB.BouncyBallSound, GetComponent<Transform>().position);
    }
}
