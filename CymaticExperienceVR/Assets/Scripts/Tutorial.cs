using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpotlightHandler))]
public class Tutorial : MonoBehaviour
{
    private SpotlightHandler _spotLightHandler;
    private int _currentStage = 0;

    public float StartDelay = 5.0f;
    //public float NextStageDelay = 0.5f;
    public float CompleteDelay = 0.5f;
    private float stageSwitchTime;
    private bool isSwitchingStage = false;

    private float _startupTime;
    private float _delayOnStart = 5.0f;
    private bool _sceneStarting = false;
    private FMODUnity.StudioEventEmitter _chladniTalkBoard;
    public GameObject chladniSoundEmitter;
    private bool _soundShouldChange = false;
    private string _soundTargetString = "";
    private bool _welcomeSoundHasPlayed = false;
    private bool _fsliderSoundHasPlayed = false;
    private bool _sandMoveSoundHasPlayed = false;
    private bool _sliderMoveSoundHasPlayed = false;
    public VideoScreen[] videos;
    private string[] _tutorialSounds;

    void Awake()
    {
        _tutorialSounds = new string[8];
        switch(GLOB.LanguageSelected)
        {
            case GLOB.Language.Dutch:
                _tutorialSounds[0] = GLOB.TutorialStartDutchSound;
                _tutorialSounds[1] = GLOB.TutorialPickingUpDutchSound;
                _tutorialSounds[2] = GLOB.TutorialShakeDutchSound;
                _tutorialSounds[3] = GLOB.TutorialSandMoveDutchSound;
                _tutorialSounds[4] = GLOB.TutorialFslidersDutchSound;
                _tutorialSounds[5] = GLOB.TutorialSliderMoveDutchSound;
                _tutorialSounds[6] = GLOB.TutorialAslidersDutchSound;                
                _tutorialSounds[7] = GLOB.TutorialEndingDutchSound;
                break;
            case GLOB.Language.German:
                _tutorialSounds[0] = GLOB.TutorialStartGermanSound;
                _tutorialSounds[1] = GLOB.TutorialPickingUpGermanSound;
                _tutorialSounds[2] = GLOB.TutorialShakeGermanSound;
                _tutorialSounds[3] = GLOB.TutorialSandMoveGermanSound;
                _tutorialSounds[4] = GLOB.TutorialFslidersGermanSound;
                _tutorialSounds[5] = GLOB.TutorialSliderMoveGermanSound;
                _tutorialSounds[6] = GLOB.TutorialAslidersGermanSound;
                _tutorialSounds[7] = GLOB.TutorialEndingGermanSound;
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _spotLightHandler = this.GetComponent<SpotlightHandler>();
        _chladniTalkBoard = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _chladniTalkBoard.Event = _tutorialSounds[0];
        _chladniTalkBoard.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(chladniSoundEmitter.transform));
    }

    // Update is called once per frame
    void Update()
    {
        if (_chladniTalkBoard.Event == _tutorialSounds[0] && !_chladniTalkBoard.IsPlaying() && _welcomeSoundHasPlayed)
        {
            _soundTargetString = _tutorialSounds[1];
            _soundShouldChange = true;
        }
        if (_chladniTalkBoard.Event == _tutorialSounds[3] && !_chladniTalkBoard.IsPlaying() && _sandMoveSoundHasPlayed)
        {
            _soundTargetString = _tutorialSounds[4];
            _soundShouldChange = true;
        }
        if(_chladniTalkBoard.Event == _tutorialSounds[4] && !_chladniTalkBoard.IsPlaying() && _fsliderSoundHasPlayed)
        {
            _soundTargetString = _tutorialSounds[5];
            _soundShouldChange = true;
        }
        if(_chladniTalkBoard.Event == _tutorialSounds[5] && !_chladniTalkBoard.IsPlaying() && _sliderMoveSoundHasPlayed)
        {
            _soundTargetString = _tutorialSounds[6];
            _soundShouldChange = true;
        }
        if (_currentStage <= 0)
        {
            if (Time.time >= StartDelay)
            {
                ResetTutorial();
            }
            else
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
        if(_soundShouldChange)
        {
            ChangeSounds(_soundTargetString);
        }
    }

    public void CompleteStage(int pStage)
    {
        if (_currentStage == pStage && !isSwitchingStage)
        {
            _soundShouldChange = true;
            switch (pStage)
            {
                case 1:
                    _soundTargetString = _tutorialSounds[2];
                    break;
                case 2:
                    _soundTargetString = _tutorialSounds[3];
                    break;
                case 3:
                     _soundTargetString = _tutorialSounds[7];
                    break;
            }
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
        ChangeSounds(_tutorialSounds[0]);
        _chladniTalkBoard.Play();
        _welcomeSoundHasPlayed = true;
        _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
    }

    private void ChangeSounds(string pGlobName, bool pBoolInterrupt = true)
    {

        if (_chladniTalkBoard.IsPlaying())
        {
            if (pBoolInterrupt)
            {
                _chladniTalkBoard.Stop();
            }
            //Fade out.
        } else
        {
            for (int i = 0; i < videos.Length; i++)
            {
                videos[i].PlayChladniVideo(Array.IndexOf(_tutorialSounds, pGlobName));
            }
            Debug.Log(Array.IndexOf(_tutorialSounds, pGlobName));
            _chladniTalkBoard.ChangeEvent(pGlobName);
            _chladniTalkBoard.Play();
            _soundShouldChange = false;
            if(pGlobName == _tutorialSounds[4])
            {
                _fsliderSoundHasPlayed = true;
            }
            if(pGlobName == _tutorialSounds[3])
            {
                _sandMoveSoundHasPlayed = true;
            }
            if(pGlobName == _tutorialSounds[5])
            {
                _sliderMoveSoundHasPlayed = true;
            }
        }
    }

}
