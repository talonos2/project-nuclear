using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public static AudioClip potBreakSound, rockAttackStrongSound, rockAttackWeakSound;
    static AudioSource audioSrc;
    AudioSource environmentalSound;
    internal string currentlyPlayingEnvTrack = "";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (!environmentalSound)
        {
            CreateEnvironmentalSound();
        }
    }

    private void CreateEnvironmentalSound()
    {
        GameObject tempGO = new GameObject();
        tempGO.name = "EnvironmentalSound";
        tempGO.transform.parent = this.transform;
        environmentalSound = tempGO.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string clip, float vol)
    {
        if (!audioSrc)
        {
            audioSrc = this.gameObject.AddComponent<AudioSource>();
        }
        audioSrc.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + clip), vol);
    }

    public void PlaySound(AudioClip clip)
    {
        if (!audioSrc)
        {
            audioSrc = this.gameObject.AddComponent<AudioSource>();
        }
        audioSrc.PlayOneShot(clip);
    }

    public void PlayPersistentSound(string clip)
    {
        audioSrc.clip = Resources.Load<AudioClip>("Sounds/" + clip);
        audioSrc.Play();
    }

    public void ChangeEnvironmentTrack(string clip)
    {
        if (!environmentalSound)
        {
            CreateEnvironmentalSound();
        }

        environmentalSound.clip = Resources.Load<AudioClip>("Sounds/Environment/" + clip);
        environmentalSound.loop = true;
        currentlyPlayingEnvTrack = clip;
        environmentalSound.Play();
    }

    public void ChangeEnvironmentTrack()
    {
        currentlyPlayingEnvTrack = "";
        if (environmentalSound)
        {
            environmentalSound.Stop();
        }
    }

    public void ChangeEnvironmentVolume(float vol)
    {
        Debug.Log("Changing volume: " + vol);
        environmentalSound.volume = vol / 9.0f;
    }
}
