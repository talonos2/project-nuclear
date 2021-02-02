using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideoPlayer : MonoBehaviour
{
    public VideoClip videoClip;

    void Start()
    {
        VideoPlayer videoPlayer = gameObject.AddComponent<VideoPlayer>();
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        videoPlayer.playOnAwake = false;
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = "_MainTex";
        videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            var vp = GetComponent<UnityEngine.Video.VideoPlayer>();

            if (vp.isPlaying)
            {
                vp.Pause();
            }
            else
            {
                vp.Play();
            }
        }
    }
}