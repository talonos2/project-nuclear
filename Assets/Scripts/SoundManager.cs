using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    public static AudioClip potBreakSound, rockAttackStrongSound, rockAttackWeakSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        /*potBreakSound = Resources.Load<AudioClip>("potBreak");
        rockAttackStrongSound = Resources.Load<AudioClip>("rockAttackStrong");
        rockAttackWeakSound = Resources.Load<AudioClip>("rockAttackWeak");*/

        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        AudioClip clipToPlay=Resources.Load<AudioClip>("Sounds/" + clip);
  
        
        audioSrc.PlayOneShot(clipToPlay);

        /*
        switch (clip)
        {
            case "potBreak":
                audioSrc.PlayOneShot(potBreakSound);
                break;
            case "rockAttackStrong":
                audioSrc.PlayOneShot(rockAttackStrongSound);
                break;
            case "rockAttackWeak":
                audioSrc.PlayOneShot(rockAttackWeakSound);
                break;


        }*/



    }



}
