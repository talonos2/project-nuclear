using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OptionSaveManager
{
    public float musicVolume;
    public float soundVolume;


    internal void SetupOptionsSave(float soundEffectVolume, float musicVolume)
    {
        this.musicVolume = musicVolume;
        this.soundVolume = soundEffectVolume;

    }
}
