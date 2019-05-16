using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMe : MonoBehaviour
{
    int waveSwitch = 1;
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counter <= 45)
        {
            transform.eulerAngles += new Vector3(0, 0, waveSwitch);
            counter++;
        } else
        {
            waveSwitch = -waveSwitch;
            counter = 0;
        }
    }
}
