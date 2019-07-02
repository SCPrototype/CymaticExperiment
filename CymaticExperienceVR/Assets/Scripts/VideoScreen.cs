﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoScreen : MonoBehaviour
{
    private VideoPlayer vp;
    [Tooltip("Start playing videos on awake?")]
    public bool PlayOnAwake = false;
    [Tooltip("Should clips restart when finished?")]
    public bool LoopVideos = true;
    [Tooltip("Should we return to the start of the clip array if we reached the end?")]
    public bool ReturnToStart = true;
    [Tooltip("Should the next clip start playing when the previous one is finished?")]
    public bool ContinueOnFinish = true;
    public VideoClip[] VideosRepeating;
    public VideoClip[] VideosCatniFace;
    public VideoClip[] VideosEndingDutch;
    public VideoClip[] VideosEndingGerman;
    public int clipIndex = 0;
    private FMODUnity.StudioEventEmitter _monitorTurningOn;
    private FMODUnity.StudioEventEmitter _monitorSwitchClip;
    private int _indexEndClip = 0;
    private string[] clipList;
    private bool _videothingiesended = false;
    public enum ChladniAnimations
    {
        START = 0,
        PICKINGUP = 1,
        SHAKE = 2,
        SANDMOVE = 3,
        FSLIDER = 4,
        SLIDERMOVE = 5,
        ASLIDER = 6,
        END = 7,
    };

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        _monitorTurningOn = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _monitorTurningOn.Event = GLOB.MonitorTurnOnSound;
        _monitorTurningOn.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        _monitorSwitchClip = this.gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _monitorSwitchClip.Event = GLOB.MonitorSwitchSound;
        _monitorSwitchClip.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject.transform));
        vp.loopPointReached += EndReached;
        clipList = GameObject.Find("LightHolders").GetComponent<Tutorial>().getTutorialVideoNames();
    }

    public void StopVideo()
    {
        if (vp != null && vp.isPlaying)
        {
            vp.Stop();
        }
    }

    public void StartUpMonitor()
    {
        _monitorTurningOn.Play();
    }

    public void PlayVideo(int pIndex, bool pLoop = false)
    {
     
    }

    public void PlayChladniVideo(int pIndex = 90)
    {
        vp.Stop();
        if (pIndex == 90)
        {
            pIndex = clipIndex;
        } else
        {
            clipIndex = pIndex;
        }
        vp.clip = VideosCatniFace[pIndex];
        vp.isLooping = false;
        vp.Play();
    }

    public void PlayRepeatingVideo(int pIndex)
    {
        vp.isLooping = true;
        if (!_monitorSwitchClip.IsPlaying())
        {
            _monitorSwitchClip.Play();
        }
        if (vp == null)
        {
            return;
        }

        if (vp.isPlaying)
        {
            vp.Stop();
        }
        vp.clip = VideosRepeating[pIndex];
        vp.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void EndReached(UnityEngine.Video.VideoPlayer pVidPlayer)
    {
        if (!_videothingiesended)
        {
            //If the current clip is not looping
            if (!pVidPlayer.isLooping && clipIndex != 0 && clipIndex != 7 && clipIndex != 3)
            {
                if (pVidPlayer.clip.name == clipList[1])
                {
                    PlayRepeatingVideo(1);
                }
                else
                {
                    PlayRepeatingVideo(clipIndex);
                }
            }
            if (clipIndex == 7)
            {
                vp.Stop();
                _videothingiesended = true;
                EndReached(pVidPlayer);
            }
        } else
        {
            PlayEndVideo(_indexEndClip);
            if (_indexEndClip == 0)
            {
                _indexEndClip = 1;
            }
            else
            {
                _indexEndClip = 0;
            }
        }
    }

    public void ChangeClipIndex(int pIndex)
    {
        //clipIndex = pIndex;
    }

    public void OnCollisionEnter(Collision collision)
    {
    }

    public void PlayEndVideo(int pIndex)
    {
        if (GLOB.LanguageSelected == GLOB.Language.Dutch)
        {
            vp.clip = VideosEndingDutch[pIndex];
        } else
        {
            vp.clip = VideosEndingGerman[pIndex];
        }
        vp.isLooping = false;
        vp.Play();
    }
}
