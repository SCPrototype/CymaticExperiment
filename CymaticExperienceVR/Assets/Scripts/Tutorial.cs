using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private SpotlightHandler _spotLightHandler;
    private bool[] stages = new bool[3] { false, false, false };
    // Start is called before the first frame update
    void Start()
    {
        _spotLightHandler = this.GetComponent<SpotlightHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CompleteStage1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CompleteStage2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CompletedTutorial();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ResetTutorial();
        }
    }

    public void CompleteStage1()
    {
        _spotLightHandler.SetLightState(SpotlightHandler.LightState.PLATE);
        stages[0] = true;
    }

    public void CompleteStage2()
    {
        _spotLightHandler.SetLightState(SpotlightHandler.LightState.SLIDERS);
        stages[1] = true;
    }

    public void CompletedTutorial()
    {
        _spotLightHandler.SetLightState(SpotlightHandler.LightState.OFF);
        stages[2] = true;
    }

    public void ResetTutorial()
    {
        _spotLightHandler.SetLightState(SpotlightHandler.LightState.JARS);
    }
}
