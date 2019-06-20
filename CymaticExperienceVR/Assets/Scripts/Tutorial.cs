using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpotlightHandler))]
public class Tutorial : MonoBehaviour
{
    private SpotlightHandler _spotLightHandler;
    private int _currentStage = 0;

    public float StartDelay = 5.0f;
    public float NextStageDelay = 0.5f;
    public float CompleteDelay = 0.5f;
    private float stageSwitchTime;
    private bool isSwitchingStage = false;

    private float _startupTime;
    private float _delayOnStart = 5.0f;
    private bool _sceneStarting = false;

    // Start is called before the first frame update
    void Start()
    {
        _spotLightHandler = this.GetComponent<SpotlightHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentStage <= 0)
        {
            if (Time.time >= StartDelay)
            {
                ResetTutorial();
            } else
            {
                return;
            }
        }

        if (isSwitchingStage)
        {
            if (Time.time - stageSwitchTime >= CompleteDelay)
            {
                isSwitchingStage = false;
            }
        }

        if (!_sceneStarting)
        {
            if (Time.time > _startupTime + _delayOnStart)
            {
                _sceneStarting = true;
                
            }
        }
    }

    public void CompleteStage(int pStage)
    {
        if (_currentStage == pStage && !isSwitchingStage)
        {
            if (CompleteDelay > 0)
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
        if (CompleteDelay > 0)
        {
            stageSwitchTime = Time.time;
            isSwitchingStage = true;
        }
        _currentStage = 1;
        _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
    }
}
