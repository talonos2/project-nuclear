using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMapMusicOnMapStart : MonoBehaviour
{
    public int mapMusic;
    public int combatMusic;
    public bool alwaysOn;
    public bool onVictoryMap;
    

    // Start is called before the first frame update
    void Start()
    {
        if (onVictoryMap) {
            if (GameData.Instance.Worst == 1)
            {
                mapMusic=16;
            }
            else
            {
                mapMusic=17;
            }
        }

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

    public void SetVictoryMusic(int victoryToPlay) {
        //For some reason this wasn't setting the music
        MusicManager.instance.FadeOutMusic(MusicManager.instance.mapMusic, .25f);
        MusicManager.instance.FadeMusic(victoryToPlay, .25f, MusicManager.globalMusicVolume);
        MusicManager.instance.SetMapMusic(victoryToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
