using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShortcutCutscene4_1 : ShortcutPlayer
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
            setupCutsceneLocation(new Vector3(26f, 0f, 0));
            SceneManager.LoadScene("SC_Map4-1", LoadSceneMode.Additive);
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = 1.45f;
        }
        if (phases[2])
        {
            activateAlertShortcut();
            waiting = true;
            waitTime = 2.5f;
        }
        if (phases[3])
        {
            activateShortcut();
            SoundManager.Instance.PlaySound("Explosion", 1);
            GameData.Instance.map4_1Shortcut = true;
            //GameData.Instance.map1_3Shortcut = true;
            GameData.Instance.deathBoss2 = true;
            waiting = true;
            waitTime = 2f;
        }
        if (phases[4])
        {
            fadeInController.enableShortcutFadeOut(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[5])
        {
            SceneManager.UnloadSceneAsync("SC_Map4-1");
            setupBackInDungeon();
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[6])
        {
            GameData.Instance.isCutscene = false;
            playingScene = false;
            GameState.isInBattle = false;
        }
    }

    private void activateAlertShortcut()
    {
        GameObject.Find("Map4_1Shortcut_Manager").GetComponent<Map4_1Shortcut>().setupShortcutAlert();

    }

    private void activateShortcut()
    {
        GameObject.Find("Map4_1Shortcut_Manager").GetComponent<Map4_1Shortcut>().setupShortcut();
    }
}
