using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShortcutCutsceneMap2_2stoMap3_2 : ShortcutPlayer
{
    public override void initialiseShortcutCutscene()
    {
        phases.Add(true);//Phase 0
        phases.Add(false);//Phase 1
        phases.Add(false);//Phase 2
        phases.Add(false);//Phase 3
        phases.Add(false);//Phase 4
        phases.Add(false);//Phase 5
        phases.Add(false);//Phase 6
        phases.Add(false);//Phase 7

        // startShortcutCutscene = true;
        setupPlayerObject();
        playingScene = true;
        GameData.Instance.isCutscene = true;
        GameState.isInBattle = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (playDebug)
        {

            initialiseShortcutCutscene();
            phaseNumber = 0;
            playDebug = false;
        }

        if (!GameData.Instance.isCutscene || playingScene == false) return;

        if (waiting)
        {
            waitTime -= Time.deltaTime;
            if (waitTime < 0)
            {
                waiting = false;
                phases[phaseNumber] = false;
                phaseNumber++;
                if (phases.Count != phaseNumber) phases[phaseNumber] = true;
            }
            else { return; }
        }

        //Begin of actual cutscene phases
        if (phases[0])
        {
            fadeInController.enableShortcutFadeOut(.5f);
            waiting = true;
            waitTime = .45f;//Note this is slightly less then the fade out time so that 
                            //there isn't 1 frame of the wrong map on the screen
        }
        if (phases[1])
        {
            setupCutsceneLocation(new Vector3(53f, .5f, 0));
            SceneManager.LoadScene("SC_Map3-2", LoadSceneMode.Additive);
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = 1.5f;
        }
        if (phases[2])
        {
            activateShortcut();
            GameData.Instance.map3_2Shortcut = true;
            waiting = true;
            waitTime = 3f;
        }
        if (phases[3])
        {
            fadeInController.enableShortcutFadeOut(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[4])
        {
            //SceneManager.UnloadSceneAsync("SC_Map3-2");
            setupBackInDungeon();
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[5])
        {
            GameData.Instance.isCutscene = false;
            playingScene = false;
            GameState.isInBattle = false;
        }
    }

    private void activateShortcut()
    {
        GameObject.Find("ShortcutController3_2").GetComponent<DoorShortcutController>().setupShortcut();
    }

}
