using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablemover : MonoBehaviour
{
    public GameObject tableHandle;
    Vector2 fixedPositions = new Vector2(5.85f, -3.46f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tableHandle.transform.position = new Vector3(fixedPositions.x, tableHandle.transform.position.y, fixedPositions.y);
    }
}
