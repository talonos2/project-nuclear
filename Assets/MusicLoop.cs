using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicLoop : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;

    internal AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
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
            yield return new WaitForSeconds(audioSource.clip.length);
        }
        audioSource.clip = loop;
        audioSource.Play();
    }
}