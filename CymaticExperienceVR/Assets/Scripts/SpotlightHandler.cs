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
        TABLET = 5
    };

    public VideoScreen[] _videoScreens; //NOTE: The video players cause frame rate drops when switching videos.

    public GameObject _lightJars;
    public ParticleSystem[] _partJars;
    public GameObject _lightPlate;
    public ParticleSystem[] _partPlate;
    public GameObject _lightSliders;
    public ParticleSystem[] _partSliders;
    public GameObject _lightLever;
    public ParticleSystem[] _partLever;

    public Transform TargetMoveObject1;
    private Vector3 startRotation1;
    public Vector3[] StepRotation1;
    public Transform TargetMoveObject2;
    private Vector3 startRotation2;
    public Vector3[] StepRotation2;
    [Range(0.0f, 1.0f)]
    public float RotationSpeed = 0.1f;
    private float lerpTime = 0.0f;

    private LightState _lightState;
    private FMOD.Studio.EventInstance _spotLightSound;
    // Start is called before the first frame update
    public void Start()
    {
        _spotLightSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayArea/LeverRelease");
        startRotation1 = TargetMoveObject1.localEulerAngles;
        startRotation2 = TargetMoveObject2.localEulerAngles;
    }

    public void Update()
    {
        if (lerpTime <= 1)
        {
            if (TargetMoveObject1.localEulerAngles != StepRotation1[(int)_lightState])
            {
                TargetMoveObject1.localEulerAngles = Vector3.LerpUnclamped(startRotation1, StepRotation1[(int)_lightState], lerpTime);
                //TargetMoveObject1.localEulerAngles += (StepRotation1[(int)_lightState] - TargetMoveObject1.localEulerAngles) * RotationSpeed;
            }
            if (TargetMoveObject2.localEulerAngles != StepRotation2[(int)_lightState])
            {
                TargetMoveObject2.localEulerAngles = Vector3.LerpUnclamped(startRotation2, StepRotation2[(int)_lightState], lerpTime);
                //TargetMoveObject2.localEulerAngles += (StepRotation2[(int)_lightState] - TargetMoveObject2.localEulerAngles) * RotationSpeed;
            }
            lerpTime = Mathf.Clamp(lerpTime + RotationSpeed, 0.0f, 1.0f);
        }
    }

    private void SwitchLights(LightState pLightState)
    {
        lerpTime = 0;
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
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].StopVideo();
                }
                break;
            case LightState.JARS:
                _lightJars.SetActive(true);
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
                _lightPlate.SetActive(true);
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
                _lightSliders.SetActive(true);
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
                _lightLever.SetActive(true);
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
                for (int i = 0; i < _partLever.Length; i++)
                {
                    _partLever[i].Play(); //NOTE: Tablet uses the same lights and particles as the lever.
                }
                for (int i = 0; i < _videoScreens.Length; i++)
                {
                    _videoScreens[i].PlayVideo((int)LightState.TABLET - 1);
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

        if ((int)pLightState > (int)LightState.TABLET) //If the new light state is invalid, turn lights off.
        {
            _lightState = LightState.OFF;
        }
        else
        {
            _lightState = pLightState;
        }
        SwitchLights(_lightState);
        FMODUnity.RuntimeManager.PlayOneShot(GLOB.SpotlightSound, GetComponent<Transform>().position);
    }
}
