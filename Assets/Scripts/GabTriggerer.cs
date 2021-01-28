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
    //public static bool[] gabNumbers = new bool[32];
    private double timeRemaining;
    public string sound;
    public bool forcePause;
    public VideoClip clipToPlayForTutorial = null;
    public double time;
    public ActivationRequirement activationRequirement = ActivationRequirement.NONE;
    public bool changesBasedOnControlSceme = false;
    public string gabText2;
    //public bool firstTownBackGab;

    public enum ActivationRequirement { NONE, NEEDS_ICE_POWER }

    public new void Start()
    {
        base.Start();
        
    }


    public void TriggerGab()
    {
        if (!GameData.Instance.gabNumbers[gabNumber]&&RequirementsMet())
        {
            if (sound != null)
            {
                SoundManager.Instance.PlaySound(sound, 1f);
            }
            if (forcePause)
            {
                timeRemaining = time;
                GameState.setFullPause(true);
            }
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            if (uiController==null) //We must be in town.
            {
                uiController = GameObject.Find("TownMenuUi");
            }
            if (changesBasedOnControlSceme && FWInputManager.Instance.IsWASD())
            {
                uiController.GetComponent<GabTextController>().AddGabToPlay(new GabTextController.Gab(gabText2, true, (float)time, true, true, clipToPlayForTutorial));
            }
            else
            {
                uiController.GetComponent<GabTextController>().AddGabToPlay(new GabTextController.Gab(gabText, true, (float)time, true, true, clipToPlayForTutorial));
            }
            GameData.Instance.gabNumbers[gabNumber] = true;
        }
        GameObject.Destroy(this.gameObject);
    }

    private bool RequirementsMet()
    {
        switch (activationRequirement)
        {
            case ActivationRequirement.NEEDS_ICE_POWER:
                if (GameData.Instance.PowersGained<1)
                {
                    return false;
                }
                return true;
        }
        return true;
    }

    void Update()
    {
//        if (GameData.Instance.isCutscene || GameData.Instance.isInDialogue || GameState.fullPause) return;



        
      //  if (firstTownBackGab) TriggerGab();
    }
}
