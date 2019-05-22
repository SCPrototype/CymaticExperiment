using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    const float _DestroyAfter = 2.0f;

    public bool isConnectedToTable = false;
    private Chladni chladni;
    private Collider plateCollider;
    private Rigidbody rb;

    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        chladni = GameObject.Find("ChladniTable").GetComponent<Chladni>();
        plateCollider = chladni.collisionBox.GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        spawnTime = Time.time;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
    public void SetVelocity(Vector3 pVel)
    {
        rb.velocity = pVel;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isConnectedToTable)
        {
            if (plateCollider.bounds.Contains(transform.position))
            {
                chladni.AddSand(this);
                isConnectedToTable = true;
            }
            else if (Time.time >= spawnTime + _DestroyAfter)
            {
                Debug.Log("Destroying sand.");
                Destroy(this.gameObject);
            }
        }
    }
}
