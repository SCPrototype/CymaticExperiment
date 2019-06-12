using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpotlightHandler))]
public class Tutorial : MonoBehaviour
{
    private SpotlightHandler _spotLightHandler;
    private int _currentStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        _spotLightHandler = this.GetComponent<SpotlightHandler>();
        ResetTutorial();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CompleteStage(int pStage)
    {
        if (_currentStage == pStage)
        {
            _currentStage++;
            _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
        }
    }

    public void ResetTutorial()
    {
        _currentStage = 1;
        _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
    }
}
