using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicLoop : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;
    public float overrideIntroOffset;

    internal AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartIntro()
    {
        StartCoroutine(BeginLoop());
    }

    IEnumerator BeginLoop()
    {
        if (intro != null)
        {
            audioSource.clip = intro;
            audioSource.Play();
            while (audioSource.time<(audioSource.clip.length-Time.deltaTime))
            {
                Debug.Log("Here yielding: " + audioSource.time + ", " + audioSource.isPlaying);
                yield return null;
            }
        }
        audioSource.loop = true;
        Debug.Log("Done waiting.");
        audioSource.clip = loop;
        audioSource.Play();
    }
}