using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GabTriggerer : DoodadData
{
    // Start is called before the first frame update


    public string gabText;
    public int gabNumber;
    public static bool[] gabNumbers = new bool[32];
    private double timeRemaining;
    public string sound;
    public bool forcePause;
    public VideoClip clipToPlayForTutorial = null;
    public double time;

    public new void Start()
    {
        base.Start();
    }


    public void TriggerGab()
    {
        if (!gabNumbers[gabNumber])
        {
            if (sound != null)
            {
                SoundManager.Instance.PlaySound(sound, 1f);
            }
            if (forcePause)
            {
                timeRemaining = time;
                GameState.fullPause = true;
            }
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<GabTextController>().AddGabToPlay(new GabTextController.Gab(gabText,true, (float)time, true,true, clipToPlayForTutorial));
            gabNumbers[gabNumber] = true;
        }
        GameObject.Destroy(this.gameObject);
    }

    void Update()
    {

    }
}
