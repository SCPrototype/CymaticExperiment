using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoScreen : MonoBehaviour
{
    private VideoPlayer vp;
    public bool PlayOnAwake = false;
    public bool ReturnToStart = true;
    public bool ContinueOnFinish = true;
    public VideoClip[] Videos;
    private int clipIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();

        if (PlayOnAwake)
        {
            PlayNextVideo(true);
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
            PlayNextVideo(true);
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

        if (ContinueOnFinish && (ulong)vp.frame == vp.frameCount)
        {
            PlayNextVideo(true);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        PlayNextVideo(true);
    }
}
