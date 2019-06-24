using System.Collections;
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
        LEVER = 4,
        TABLET = 5,
        FINISHED = 6
    };

    public Light[] GlobalLights;
    public VideoScreen[] _videoScreens; //NOTE: The video players cause frame rate drops when switching videos.

    [Space(10)]
    public GameObject _lightJars;
    public GameObject _softLightJars;
    public ParticleSystem[] _partJars;
    public GameObject _fakeJars;
    public GameObject _realJars;
    [Space(10)]
    public GameObject _lightPlate;
    public GameObject _softLightPlate;
    public ParticleSystem[] _partPlate;
    public GameObject _fakePlate;
    public GameObject _realPlate;
    [Space(10)]
    public GameObject _lightSliders;
    public GameObject _softLightSliders;
    public ParticleSystem[] _partSliders;
    public GameObject _fakeSliders;
    public GameObject _realSliders;
    [Space(10)]
    public GameObject _lightLever;
    public GameObject _softLightLever;
    public ParticleSystem[] _partLever;
    public GameObject _fakeLever;
    public GameObject _realLever;

    [Space(10)]
    public GameObject[] _realSceneObjects;
    public GameObject[] _fakeSceneObjects;

    public bool DoFakeVersions = false;

    [Space(10)]
    public Transform TargetMoveObject1;
    private Vector3 startRotation1;
    public Vector3[] StepRotation1;
    public Transform TargetMoveObject2;
    private Vector3 startRotation2;
    public Vector3[] StepRotation2;

    [Space(10)]
    [Range(0.0f, 1.0f)]
    public float RotationSpeed = 0.01f;
    private float rotationLerpTime = 0.0f;

    [Space(10)]
    [Range(0.0f, 1.0f)]
    public float LightIntensitySpeed = 0.01f;
    private float lightLerpTime = 0.0f;
    private bool lightsAreOn = false;

    private LightState _lightState;
    private FMOD.Studio.EventInstance _spotLightSound;
    // Start is called before the first frame update
    public void Start()
    {
        startRotation1 = TargetMoveObject1.localEulerAngles;
        startRotation2 = TargetMoveObject2.localEulerAngles;

        if (DoFakeVersions)
        {
            _realJars.SetActive(false);
            _realLever.SetActive(false);
            _realPlate.SetActive(false);
            _realSliders.SetActive(false);
            for (int i = 0; i < _realSceneObjects.Length; i++)
            {
                _realSceneObjects[i].SetActive(false);
            }

            _fakeJars.SetActive(true);
            _fakeLever.SetActive(true);
            _fakePlate.SetActive(true);
            _fakeSliders.SetActive(true);
            for (int i = 0; i < _fakeSceneObjects.Length; i++)
            {
                _fakeSceneObjects[i].SetActive(true);
            }
        }

        _softLightJars.SetActive(false);
        _softLightPlate.SetActive(false);
        _softLightSliders.SetActive(false);
        _softLightLever.SetActive(false);

        SwitchLights(0);

        _spotLightSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayArea/LeverRelease");
    }

    public void Update()
    {
        if (rotationLerpTime < 1)
        {
            rotationLerpTime = Mathf.Clamp(rotationLerpTime + RotationSpeed, 0.0f, 1.0f);
            if (TargetMoveObject1.localEulerAngles != StepRotation1[(int)_lightState])
            {
                TargetMoveObject1.localEulerAngles = Vector3.LerpUnclamped(startRotation1, StepRotation1[(int)_lightState], rotationLerpTime);
                //TargetMoveObject1.localEulerAngles += (StepRotation1[(int)_lightState] - TargetMoveObject1.localEulerAngles) * RotationSpeed;
            }
            if (TargetMoveObject2.localEulerAngles != StepRotation2[(int)_lightState])
            {
                TargetMoveObject2.localEulerAngles = Vector3.LerpUnclamped(startRotation2, StepRotation2[(int)_lightState], rotationLerpTime);
                //TargetMoveObject2.localEulerAngles += (StepRotation2[(int)_lightState] - TargetMoveObject2.localEulerAngles) * RotationSpeed;
            }
        }

        if (lightsAreOn)
        {
            if (lightLerpTime < 1)
            {
                lightLerpTime = Mathf.Clamp(lightLerpTime + LightIntensitySpeed, 0.0f, 1.0f);
                for (int i = 0; i < GlobalLights.Length; i++)
                {
                    GlobalLights[i].intensity = lightLerpTime;
                }
            }
        }
    }

    private void SwitchLights(LightState pLightState)
    {
        rotationLerpTime = 0;
        _lightJars.SetActive(false);
        _lightPlate.SetActive(false);
        _lightSliders.SetActive(false);
        _lightLever.SetActive(false);
        for (int i = 0; i < _partJars.Length; i++)
        {
            _partJars[i].Stop();
            _partJars[i].Clear(true);
        }
        for (int i = 0; i < _partPlate.Length; i++)
        {
            _partPlate[i].Stop();
            _partPlate[i].Clear(true);
        }
        for (int i = 0; i < _partSliders.Length; i++)
        {
            _partSliders[i].Stop();
            _partSliders[i].Clear(true);
        }
        for (int i = 0; i < _partLever.Length; i++)
        {
            _partLever[i].Stop();
            _partLever[i].Clear(true);
        }
        if (!lightsAreOn)
        {
            for (int i = 0; i < GlobalLights.Length; i++)
            {
                GlobalLights[i].enabled = false;
                lightLerpTime = 1;
            }
        }

        switch (pLightState)
        {
            case LightState.OFF:
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].StopVideo();
                }
                if (lightsAreOn)
                {
                    lightsAreOn = false;
                    for (int i = 0; i < GlobalLights.Length; i++)
                    {
                        GlobalLights[i].intensity = 0;
                        lightLerpTime = 1;
                        GlobalLights[i].enabled = false;
                    }
                }
                if (DoFakeVersions)
                {
                    for (int i = 0; i < _realSceneObjects.Length; i++)
                    {
                        _realSceneObjects[i].SetActive(false);
                    }
                    for (int i = 0; i < _fakeSceneObjects.Length; i++)
                    {
                        _fakeSceneObjects[i].SetActive(true);
                    }
                }
                break;

            case LightState.JARS:
                if (DoFakeVersions)
                {
                    _fakeJars.SetActive(false);
                    _realJars.SetActive(true);
                }
                _lightJars.SetActive(true);
                //_softLightJars.SetActive(true);
                for (int i = 0; i < _partJars.Length; i++)
                {
                    _partJars[i].Play();
                }
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].PlayVideo((int)LightState.JARS - 1);
                }
                break;

            case LightState.PLATE:
                if (DoFakeVersions)
                {
                    _fakePlate.SetActive(false);
                    _realPlate.SetActive(true);
                }
                _lightPlate.SetActive(true);
                _softLightJars.SetActive(true);
                for (int i = 0; i < _partPlate.Length; i++)
                {
                    _partPlate[i].Play();
                }
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].PlayVideo((int)LightState.PLATE - 1);
                }
                break;

            case LightState.SLIDERS:
                if (DoFakeVersions)
                {
                    _fakeSliders.SetActive(false);
                    _realSliders.SetActive(true);
                }
                _lightSliders.SetActive(true);
                _softLightPlate.SetActive(true);
                for (int i = 0; i < _partSliders.Length; i++)
                {
                    _partSliders[i].Play();
                }
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].PlayVideo((int)LightState.SLIDERS - 1);
                }
                break;

            case LightState.LEVER:
                if (DoFakeVersions)
                {
                    _fakeLever.SetActive(false);
                    _realLever.SetActive(true);
                }
                _lightLever.SetActive(true);
                _softLightSliders.SetActive(true);
                for (int i = 0; i < _partLever.Length; i++)
                {
                    _partLever[i].Play();
                }
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].PlayVideo((int)LightState.LEVER - 1);
                }
                break;

            case LightState.TABLET:
                _lightLever.SetActive(true);
                _softLightLever.SetActive(true);
                for (int i = 0; i < _partLever.Length; i++)
                {
                    _partLever[i].Play(); //NOTE: Tablet uses the same lights and particles as the lever.
                }
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].PlayVideo((int)LightState.TABLET - 1);
                }
                break;

            case LightState.FINISHED:
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].StopVideo();
                }

                _softLightJars.SetActive(false);
                _softLightPlate.SetActive(false);
                _softLightSliders.SetActive(false);
                _softLightLever.SetActive(false);

                if (!lightsAreOn)
                {
                    lightsAreOn = true;
                    for (int i = 0; i < GlobalLights.Length; i++)
                    {
                        GlobalLights[i].intensity = 0;
                        lightLerpTime = 0;
                        GlobalLights[i].enabled = true;
                    }
                }
                if (DoFakeVersions)
                {
                    for (int i = 0; i < _fakeSceneObjects.Length; i++)
                    {
                        _fakeSceneObjects[i].SetActive(false);
                    }
                    for (int i = 0; i < _realSceneObjects.Length; i++)
                    {
                        _realSceneObjects[i].SetActive(true);
                    }
                }

                break;
            default:
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].StopVideo();
                }
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
        if (_lightState >= 0 && (int)_lightState <= (int)LightState.TABLET)
        {
            startRotation1 = StepRotation1[(int)_lightState];
            startRotation2 = StepRotation2[(int)_lightState];
        }
        else //If the current light state is invalid.
        {
            startRotation1 = TargetMoveObject1.localEulerAngles; //NOTE: Could cause issues if dealing with negative rotations. (I.E. A -20 rotation would be read as a 340 rotation.)
            startRotation2 = TargetMoveObject2.localEulerAngles;
        }

        if ((int)pLightState > (int)LightState.SLIDERS) //If the new light state is invalid, turn lights off.
        {
            _lightState = LightState.FINISHED;
        }
        else
        {
            _lightState = pLightState;
        }
        SwitchLights(_lightState);
        FMODUnity.RuntimeManager.PlayOneShot(GLOB.SpotlightSound, GetComponent<Transform>().position);
    }
}
