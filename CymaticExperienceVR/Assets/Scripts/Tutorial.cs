using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpotlightHandler))]
public class Tutorial : MonoBehaviour
{
    private SpotlightHandler _spotLightHandler;
    private int _currentStage = 0;

    public float StageDelay = 0.5f;
    private float stageSwitchTime;
    private bool isSwitchingStage = false;

    // Start is called before the first frame update
    void Start()
    {
        _spotLightHandler = this.GetComponent<SpotlightHandler>();
        ResetTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSwitchingStage)
        {
            if (Time.time - stageSwitchTime >= StageDelay)
            {
                isSwitchingStage = false;
            }
        }
    }

    public void CompleteStage(int pStage)
    {
        if (_currentStage == pStage && !isSwitchingStage)
        {
            if (StageDelay > 0)
            {
                stageSwitchTime = Time.time;
                isSwitchingStage = true;
            }
            _currentStage++;
            _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
        }
    }

    public void ResetTutorial()
    {
        if (StageDelay > 0)
        {
            stageSwitchTime = Time.time;
            isSwitchingStage = true;
        }
        _currentStage = 1;
        _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
    }
}
