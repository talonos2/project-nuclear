using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShortcutCutsceneMap4_4 : ShortcutPlayer
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
            setupCutsceneLocation(new Vector3(56f, -10, 0));
            SceneManager.LoadScene("SC_Map4-4", LoadSceneMode.Additive);
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = 1.5f;
        }
        if (phases[2])
        {
            activateShortcut();
            GameData.Instance.map4_4Shortcut = true;
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
            SceneManager.UnloadSceneAsync("SC_Map4-4");
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
        GameObject.Find("ShortcutController4_4").GetComponent<shortcut4_4controller>().setupShortcut();
    }
}
