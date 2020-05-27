using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public static AudioClip potBreakSound, rockAttackStrongSound, rockAttackWeakSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
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
}
