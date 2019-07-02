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
    private float SceneStartTime;
    //public float NextStageDelay = 0.5f;
    public float CompleteDelay = 0.5f;
    private float stageSwitchTime;
    private bool isSwitchingStage = false;
    public float NextStageDelay = 0.5f;
    private float stageStartTime;
    private bool readyToComplete = false;

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
    private FMODUnity.StudioEventEmitter _soundAfterTutorial;
    private bool _goodbyeHasPlayed = false;

    void Awake()
    {
        SceneStartTime = Time.time;

        _tutorialSounds = new string[8];
        switch (GLOB.LanguageSelected)
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
        _soundAfterTutorial = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _soundAfterTutorial.Event = GLOB.BackgroundSound;
        _soundAfterTutorial.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("Complete tutorial");
            _currentStage = 6;
            readyToComplete = true;
            isSwitchingStage = true;
            CompleteStage(6);
            videos[0].PlayChladniVideo(7);
            videos[1].PlayChladniVideo(7);
            videos[0].clipIndex = 7;
            videos[1].clipIndex = 7;
            _spotLightHandler.SwitchLights(SpotlightHandler.LightState.JARS);
            _spotLightHandler.SwitchLights(SpotlightHandler.LightState.PLATE);
            _spotLightHandler.SwitchLights(SpotlightHandler.LightState.SLIDERA);
            _spotLightHandler.SwitchLights(SpotlightHandler.LightState.SLIDERF);
            _spotLightHandler.SwitchLights(SpotlightHandler.LightState.FINISHED);

        }
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
        if (_chladniTalkBoard.Event == _tutorialSounds[4] && !_chladniTalkBoard.IsPlaying() && _fsliderSoundHasPlayed)
        {
            _soundTargetString = _tutorialSounds[5];
            _soundShouldChange = true;
        }
        if(_chladniTalkBoard.Event == _tutorialSounds[7] && !_chladniTalkBoard.IsPlaying() && _goodbyeHasPlayed && !_soundAfterTutorial.IsPlaying())
        {
            _soundAfterTutorial.Play();
        }
        if (_currentStage <= 0)
        {
            if (Time.time - SceneStartTime >= StartDelay)
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
            if (Time.time - stageSwitchTime >= NextStageDelay)
            {
                _currentStage++;
                if (_currentStage == 3)
                {
                    _spotLightHandler.ChangeLight((SpotlightHandler.LightState)_currentStage, 10);
                }
                else if (_currentStage == 4)
                {
                    _spotLightHandler.ChangeLight((SpotlightHandler.LightState)_currentStage, 4);
                    CompleteDelay = 5.5f;
                }
                else
                {
                    _spotLightHandler.ChangeLight((SpotlightHandler.LightState)_currentStage);
                }
                isSwitchingStage = false;
                if (CompleteDelay > 0)
                {

                    stageStartTime = Time.time;
                    readyToComplete = false;
                }
            }
        }
        if (!readyToComplete)
        {
            if (Time.time - stageStartTime >= CompleteDelay)
            {
                readyToComplete = true;
            }
        }

        if (!_sceneStarting)
        {
            if (Time.time > _startupTime + _delayOnStart)
            {
                _sceneStarting = true;
            }
        }
        if (_soundShouldChange)
        {
            ChangeSounds(_soundTargetString, true);
            _soundAfterTutorial.Stop();
            //For some reason it justs randomly starts.
        }
    }

    public void CompleteStage(int pStage)
    {
        
        if (_currentStage == pStage && !isSwitchingStage && readyToComplete)
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
                    _soundTargetString = _tutorialSounds[6];
                    break;
                case 4:
                    _soundTargetString = _tutorialSounds[7];
                    break;
            }
            if (NextStageDelay > 0)
            {
                stageSwitchTime = Time.time;
                isSwitchingStage = true;
            }
            else
            {
                _currentStage++;
               
                _spotLightHandler.ChangeLight((SpotlightHandler.LightState)_currentStage);
                if (CompleteDelay > 0)
                {
                    stageStartTime = Time.time;
                    readyToComplete = false;
                }
            }
        }
    }

    public void ResetTutorial()
    {
        _currentStage = 1;
        ChangeSounds(_tutorialSounds[0], true);
        _chladniTalkBoard.Play();
        _welcomeSoundHasPlayed = true;
        int filelength = 14;
        _spotLightHandler.ChangeLight((SpotlightHandler.LightState)_currentStage, filelength);
        if (CompleteDelay > 0)
        {
            stageStartTime = Time.time;
            readyToComplete = false;
        }
    }

    private void ChangeSounds(string pGlobName, bool pBoolInterrupt = true)
    {
        if (_chladniTalkBoard.IsPlaying())
        {
            if (pBoolInterrupt)
            {
                _chladniTalkBoard.Stop();
            }
        }
        else
        {
            for (int i = 0; i < videos.Length; i++)
            {
                videos[i].PlayChladniVideo(Array.IndexOf(_tutorialSounds, pGlobName));
            }
            _chladniTalkBoard.ChangeEvent(pGlobName);
            _chladniTalkBoard.Play();
            _soundShouldChange = false;
            if (pGlobName == _tutorialSounds[4])
            {
                _fsliderSoundHasPlayed = true;
            }
            if (pGlobName == _tutorialSounds[3])
            {
                _sandMoveSoundHasPlayed = true;
            }
            if (pGlobName == _tutorialSounds[5])
            {
                _sliderMoveSoundHasPlayed = true;
            }
            if(pGlobName == _tutorialSounds[7])
            {
                Debug.Log("Yes hello");
                _goodbyeHasPlayed = true;
            }
        }
    }

    public string[] getTutorialVideoNames()
    {
        return _tutorialSounds;
    }

    
}