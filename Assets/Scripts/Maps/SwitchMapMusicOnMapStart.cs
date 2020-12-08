using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMapMusicOnMapStart : MonoBehaviour
{
    public int mapMusic;
    public int combatMusic;
    public bool alwaysOn;

    // Start is called before the first frame update
    void Start()
    {
        if (mapMusic != MusicManager.instance.mapMusic)
        { 
            MusicManager.instance.FadeOutMusic(MusicManager.instance.mapMusic, .25f);
            MusicManager.instance.FadeMusic(mapMusic, .25f, MusicManager.globalMusicVolume);
            MusicManager.instance.SetMapMusic(mapMusic);
        }
        if (combatMusic != MusicManager.instance.combatMusic&&!alwaysOn)
        {
            MusicManager.instance.FadeOutMusic(MusicManager.instance.combatMusic, .25f);
            MusicManager.instance.FadeMusic(combatMusic, .25f, .0001f);
            MusicManager.instance.SetCombatMusic(combatMusic);
        }
        if (alwaysOn)
        {
            MusicManager.instance.FadeOutMusic(MusicManager.instance.combatMusic, .25f);
            MusicManager.instance.FadeMusic(combatMusic, .25f, MusicManager.globalMusicVolume);
            MusicManager.instance.SetCombatMusic(combatMusic);
            MusicManager.instance.SyncCombatToPreexistingMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
