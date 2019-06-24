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
    private float stageSwitchTime;
    private bool isSwitchingStage = false;
    public float CompleteDelay = 0.5f;
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

    // Start is called before the first frame update
    void Start()
    {
        _spotLightHandler = this.GetComponent<SpotlightHandler>();
        _chladniTalkBoard = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _chladniTalkBoard.Event = GLOB.TutorialStartSound;
        _chladniTalkBoard.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(chladniSoundEmitter.transform));
    }

    // Update is called once per frame
    void Update()
    {
        if (_chladniTalkBoard.Event == GLOB.TutorialStartSound && !_chladniTalkBoard.IsPlaying() && _welcomeSoundHasPlayed)
        {
            _soundTargetString = GLOB.TutorialPickingUpSound;
            _soundShouldChange = true;
        }
        if (_chladniTalkBoard.Event == GLOB.TutorialSandMoveSound && !_chladniTalkBoard.IsPlaying() && _sandMoveSoundHasPlayed)
        {
            _soundTargetString = GLOB.TutorialFslidersSound;
            _soundShouldChange = true;
        }
        if(_chladniTalkBoard.Event == GLOB.TutorialFslidersSound && !_chladniTalkBoard.IsPlaying() && _fsliderSoundHasPlayed)
        {
            _soundTargetString = GLOB.TutorialAslidersSound;
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
            if (Time.time - stageSwitchTime >= NextStageDelay)
            {
                _currentStage++;
                _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
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
        if(_soundShouldChange)
        {
            ChangeSounds(_soundTargetString);
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
                    _soundTargetString = GLOB.TutorialShakeSound;
                    break;
                case 2:
                    _soundTargetString = GLOB.TutorialSandMoveSound;
                    break;
                case 3:
                     _soundTargetString = GLOB.TutorialEndingSound;
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
                _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
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
        ChangeSounds(GLOB.TutorialStartSound);
        _chladniTalkBoard.Play();
        _welcomeSoundHasPlayed = true;
        _spotLightHandler.SetLightState((SpotlightHandler.LightState)_currentStage);
        if (CompleteDelay > 0)
        {
            stageStartTime = Time.time;
            readyToComplete = false;
        }
    }

    private void ChangeSounds(string pGlobName)
    {
        if (_chladniTalkBoard.IsPlaying())
        {
            _chladniTalkBoard.Stop();
            //Fade out.
        } else
        {
            _chladniTalkBoard.ChangeEvent(pGlobName);
            _chladniTalkBoard.Play();
            _soundShouldChange = false;
            if(pGlobName == GLOB.TutorialFslidersSound)
            {
                _fsliderSoundHasPlayed = true;
            }
            if(pGlobName == GLOB.TutorialSandMoveSound)
            {
                _sandMoveSoundHasPlayed = true;
            }
        }
    }

}
