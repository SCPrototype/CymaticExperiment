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
    private Tutorial _tutorial;
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        chladni = GameObject.Find("TableFunctionality").GetComponent<Chladni>();
        if (chladni != null)
        {
            plateCollider = chladni.collisionBox.GetComponent<Collider>();
        } else
        {
            Debug.Log("Chladni plate not found.");
        }
        rb = GetComponent<Rigidbody>();
        _tutorial = GameObject.Find("LightHolders").GetComponent<Tutorial>();
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
                _tutorial.CompleteStage(2);
            }
            else if (Time.time >= spawnTime + _DestroyAfter)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
