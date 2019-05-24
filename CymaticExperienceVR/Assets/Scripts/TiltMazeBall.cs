using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TiltMazeBall : MonoBehaviour
{
    public Transform RespawnPoint;
    public int RespawnLevelY = -500;
    public BoxCollider ParentBounds;

    private Rigidbody rb;
    private Transform myParent;
    private SphereCollider myColl;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myParent = transform.parent;
        myColl = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ParentBounds.bounds.Intersects(myColl.bounds))
        {
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
        }
        else
        {
            if (transform.parent == null)
            {
                transform.SetParent(myParent);
            }
        }

        if (transform.position.y < RespawnLevelY)
        {
            ResetBall();
        }
    }

    public void ResetBall()
    {
        rb.isKinematic = true;
        transform.position = RespawnPoint.position;
        transform.rotation = RespawnPoint.rotation;
        rb.isKinematic = false;
    }
}
