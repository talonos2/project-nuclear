using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShortcutCutsceneMap2_3to3_3s : ShortcutPlayer
{
    public GameObject snowballToSpawn;
    public GameObject instantiatedSnowball;
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
        phases.Add(false);//Phase 8

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
            setupCutsceneLocation(new Vector3(47f, 6, 0));
            SceneManager.LoadScene("Sc_Map3-3(s)", LoadSceneMode.Additive);

            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = .5f;
        }
        if (phases[2])
        {
            instantiatedSnowball = Instantiate(snowballToSpawn, new Vector3(47f, 10, 0), Quaternion.identity);

            boulderMover movingBoulder = instantiatedSnowball.GetComponent<boulderMover>();
            instantiatedSnowball.GetComponentInChildren<SpriteShadowLoader>().setOnCutsceneMap();
            instantiatedSnowball.GetComponentInChildren<SpriteShadowLoader>().isOnCutsceneMap=true;
            movingBoulder.isOnCutsceneMap = true;
            movingBoulder.facedDirection = SpriteMovement.DirectionMoved.DOWN;
            movingBoulder.moving = true;
            //Drop ball
            //need splash sound effect here
            SoundManager.Instance.PlaySound("fallingObject_.75_sec", 1);
            waiting = true;
            waitTime = .7f;
        }
        if (phases[3])
        {
            SoundManager.Instance.PlaySound("fuseForBomb", 1);
            SoundManager.Instance.PlaySound("singleSplash", 1);
            waiting = true;
            waitTime = 1.5f;
        }

        if (phases[4])
        {
            fadeInController.enableShortcutFadeOut(.25f);
            waiting = true;
            waitTime = .2f;
        }
        if (phases[5])
        {
            Destroy(instantiatedSnowball);
            GameData.Instance.map3_3Shortcut = true;
            setupNewAfterSnowballMap();
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = 3f; //waiting for fade in And seeing the new map
        }
        if (phases[6])
        {
            GameData.Instance.map1_3toMap2_3Shortcut = false;

            fadeInController.enableShortcutFadeOut(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[7])
        {
            setupBackInDungeon();
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[8])
        {
            GameData.Instance.isCutscene = false;
            playingScene = false;
            GameState.isInBattle = false;
        }
    }

    private void setupNewAfterSnowballMap()
    {
        GameObject.Find("ShortcutController3_3").GetComponent<Map3_3ShortcutController>().setupShortcutForCutscene();
    }
}
