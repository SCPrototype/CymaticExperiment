using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpotlightHandler))]
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

    }

    public void CompleteStage1()
    {
        if (stages[0] == false)
        {
            _spotLightHandler.SetLightState(SpotlightHandler.LightState.PLATE);
            stages[0] = true;
        }
    }

    public void CompleteStage2()
    {
        if (stages[1] == false && stages[0] == true)
        {
            _spotLightHandler.SetLightState(SpotlightHandler.LightState.SLIDERS);
            stages[1] = true;
        }
    }

    public void CompletedTutorial()
    {
        if (stages[2] == false)
        {
            if (stages[0] == true && stages[1] == true) {
                _spotLightHandler.SetLightState(SpotlightHandler.LightState.OFF);
                stages[2] = true;
            }
        }
    }

    public void ResetTutorial()
    {
        _spotLightHandler.SetLightState(SpotlightHandler.LightState.JARS);
    }
}
