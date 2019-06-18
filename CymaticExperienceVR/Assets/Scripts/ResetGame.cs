using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    private static float pressDelay = 3.0f;
    private float pressStartTime;

    void Awake()
    {
        FMODUnity.RuntimeManager.LoadBank("Master Bank");
        FMODUnity.RuntimeManager.LoadBank("Master Bank.strings");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            pressStartTime = Time.time;
        }
        if (Input.GetKey(KeyCode.R))
        {
            if (Time.time - pressStartTime >= pressDelay)
            {
                pressStartTime = Time.time;
                DoResetGame();
            }
        }
    }

    public void DoResetGame()
    {
        Application.LoadLevel(0);
    }
}
