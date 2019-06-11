﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightHandler : MonoBehaviour
{
    public enum LightState
    {
        OFF = 0,
        JARS = 1,
        PLATE = 2,
        SLIDERS = 3,
        LEVER = 4
    };

    public GameObject _lightJars;
    public ParticleSystem[] _partJars;
    public GameObject _lightPlate;
    public ParticleSystem[] _partPlate;
    public GameObject _lightSliders;
    public ParticleSystem[] _partSliders;
    public GameObject _lightLever;
    public ParticleSystem[] _partLever;

    private LightState _lightState;
    private FMOD.Studio.EventInstance _spotLightSound;
    // Start is called before the first frame update
    public void Start()
    {
        _spotLightSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayArea/LeverRelease");
    }
    private void SwitchLights(LightState pLightState)
    {
        _lightJars.SetActive(false);
        _lightPlate.SetActive(false);
        _lightSliders.SetActive(false);
        _lightLever.SetActive(false);
        for (int i = 0; i < _partJars.Length; i++)
        {
            _partJars[i].Stop();
        }
        for (int i = 0; i < _partPlate.Length; i++)
        {
            _partPlate[i].Stop();
        }
        for (int i = 0; i < _partSliders.Length; i++)
        {
            _partSliders[i].Stop();
        }
        for (int i = 0; i < _partLever.Length; i++)
        {
            _partLever[i].Stop();
        }

        switch (pLightState)
        {
            case LightState.OFF:
                
                break;
            case LightState.JARS:
                _lightJars.SetActive(true);
                for (int i = 0; i < _partJars.Length; i++)
                {
                    _partJars[i].Play();
                }
                break;
            case LightState.PLATE:
                _lightPlate.SetActive(true);
                for (int i = 0; i < _partPlate.Length; i++)
                {
                    _partPlate[i].Play();
                }
                break;
            case LightState.SLIDERS:
                _lightSliders.SetActive(true);
                for (int i = 0; i < _partSliders.Length; i++)
                {
                    _partSliders[i].Play();
                }
                break;
            case LightState.LEVER:
                _lightLever.SetActive(true);
                for (int i = 0; i < _partLever.Length; i++)
                {
                    _partLever[i].Play();
                }
                break;
            default:

                break;
        }
    }

    public LightState GetLightState()
    {
        return _lightState;
    }

    public void SetLightState(LightState pLightState)
    {
        //_spotLightSound.start();
        _lightState = pLightState;
        SwitchLights(_lightState);
    }
}
