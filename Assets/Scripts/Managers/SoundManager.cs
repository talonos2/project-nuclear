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
        audioSrc = this.gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string clip)
    {
        audioSrc.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + clip));
    }

    public void PlaySound(AudioClip clip)
    {
        audioSrc.PlayOneShot(clip);
    }
}
