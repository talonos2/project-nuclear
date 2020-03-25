using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMapMusicOnMapStart : MonoBehaviour
{
    public int mapMusic;
    public int combatMusic;

    // Start is called before the first frame update
    void Start()
    {
        if (mapMusic != MusicManager.instance.mapMusic)
        { 
            MusicManager.instance.FadeOutMusic(MusicManager.instance.mapMusic, .25f);
            MusicManager.instance.FadeMusic(mapMusic, .25f, MusicManager.globalMusicVolume);
            MusicManager.instance.SetMapMusic(mapMusic);
        }
        if (combatMusic != MusicManager.instance.combatMusic)
        {
            MusicManager.instance.FadeOutMusic(MusicManager.instance.combatMusic, .25f);
            MusicManager.instance.FadeMusic(combatMusic, .25f, .01f);
            MusicManager.instance.SetCombatMusic(combatMusic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
