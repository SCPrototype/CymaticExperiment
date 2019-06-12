using System.Collections;
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
    public VideoClip[] Videos;
    private int clipIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();

        if (PlayOnAwake)
        {
            PlayNextVideo(LoopVideos);
        }
        if (ContinueOnFinish)
        {
            vp.loopPointReached += EndReached;
        }
    }

    public void StopVideo()
    {
        if (vp.isPlaying)
        {
            vp.Stop();
        }
    }

    public void PlayVideo(int pIndex, bool pLoop = true)
    {
        if (vp.isPlaying)
        {
            vp.Stop();
        }

        vp.isLooping = pLoop;

        if (pIndex < Videos.Length)
        {
            vp.clip = Videos[pIndex];
            vp.Play();
            clipIndex = pIndex;
        }
    }

    public void PlayNextVideo(bool pLoop = true)
    {
        if (vp.isPlaying)
        {
            vp.Stop();
        }

        if (clipIndex + 1 < Videos.Length && clipIndex + 1 >= 0)
        {
            clipIndex++;
            vp.isLooping = pLoop;
            vp.clip = Videos[clipIndex];
            vp.Play();
        }
        else if (ReturnToStart)
        {
            clipIndex = -1;
            PlayNextVideo(pLoop);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayNextVideo(LoopVideos);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (vp.isPlaying)
            {
                vp.Pause();
            }
            else if (vp.isPaused)
            {
                vp.Play();
            }
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer pVidPlayer)
    {
        //If the current clip is not looping
        if (!pVidPlayer.isLooping)
        {
            //Play the next clip
            PlayNextVideo(LoopVideos);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //PlayNextVideo(LoopVideos);
    }
}
