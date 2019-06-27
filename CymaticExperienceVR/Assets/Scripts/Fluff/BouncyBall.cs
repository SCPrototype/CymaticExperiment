using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BouncyBall : VR_Object
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void HandleRespawn()
    {
        base.HandleRespawn();
    }

    protected override void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        base.ObjectGrabbed(sender, e);
    }

    protected override void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {
        base.ObjectReleased(sender, e);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
