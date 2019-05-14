using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    public bool isConnectedToTable = false;
    private Chladni chladni;
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        chladni = GameObject.Find("TableHolder").GetComponent<Chladni>();
        collider = chladni.collisionBox.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isConnectedToTable)
        {
            if (collider.bounds.Contains(transform.position))
            {
                chladni.AddSand(this.gameObject);
                isConnectedToTable = true;
                Destroy(this);
            }
            if (transform.position.y <= 0.1f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
