using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    public bool isConnectedToTable = false;
    private Chladni chladni;

    // Start is called before the first frame update
    void Start()
    {
        chladni = GameObject.Find("TableHolder").GetComponent<Chladni>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (!isConnectedToTable)
        {
            if (chladni.collisionBox.GetComponent<Collider>().bounds.Contains(transform.position))
            {
                chladni.AddSand(this.gameObject);
                isConnectedToTable = true;
            }
        }
    }
}
