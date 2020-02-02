using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnAwake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.instance.music[0].Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
