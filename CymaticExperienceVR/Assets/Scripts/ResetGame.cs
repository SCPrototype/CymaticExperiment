using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    public float pressDelay = 3.0f;
    private float pressStartTime;
    public KeyCode DutchReset;
    public KeyCode GermanReset;
    public KeyCode DutchQuestions;
    public KeyCode GermanQuestions;

    void Awake()
    {
        FMODUnity.RuntimeManager.LoadBank("Master Bank");
        FMODUnity.RuntimeManager.LoadBank("Master Bank.strings");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(DutchReset) || Input.GetKeyDown(GermanReset))
        {
            pressStartTime = Time.time;
        }
        if (Input.GetKey(DutchReset) || Input.GetKey(GermanReset))
        {
            if (Time.time - pressStartTime >= pressDelay)
            {
                pressStartTime = Time.time;
                if(Input.GetKey(GermanReset))
                {
                    GLOB.LanguageSelected = GLOB.Language.German;
                }
                if(Input.GetKey(DutchReset))
                {
                    GLOB.LanguageSelected = GLOB.Language.Dutch;
                }
                DoResetGame();
            }
        }

        if (Input.GetKeyDown(DutchQuestions) || Input.GetKeyDown(GermanQuestions))
        {
            pressStartTime = Time.time;
        }
        if (Input.GetKey(DutchQuestions) || Input.GetKey(GermanQuestions))
        {
            if (Time.time - pressStartTime >= pressDelay)
            {
                pressStartTime = Time.time;
                GoToQuestions();
            }
        }
    }

    public void DoResetGame()
    {
        Application.LoadLevel(0);
    }
    public void GoToQuestions()
    {
        Application.LoadLevel(1);
    }
}
