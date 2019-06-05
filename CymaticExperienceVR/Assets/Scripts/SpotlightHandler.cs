using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightHandler : MonoBehaviour
{
    public enum LightState
    {
        OFF,
        JARS,
        PLATE,
        SLIDERS
    };

    public GameObject _lightJars;
    public GameObject _lightPlate;
    public GameObject _lightSliders;

    private LightState _lightState;
    // Start is called before the first frame update
    public void Start()
    {
        SwitchLights(LightState.JARS);
    }
    private void SwitchLights(LightState pLightState)
    {
        _lightJars.SetActive(false);
        _lightPlate.SetActive(false);
        _lightSliders.SetActive(false);
        switch (pLightState)
        {
            case LightState.OFF:
                
                break;
            case LightState.JARS:
                _lightJars.SetActive(true);
                break;
            case LightState.PLATE:
                _lightPlate.SetActive(true);
                break;
            case LightState.SLIDERS:
                _lightSliders.SetActive(true);
                break;
        }
    }

    public LightState GetLightState()
    {
        return _lightState;
    }

    public void SetLightState(LightState pLightState)
    {
        _lightState = pLightState;
        SwitchLights(_lightState);
    }
}
