using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShortcutCutsceneMap1_3to2_3 : ShortcutPlayer
{

    //private bool startShortcutCutscene; //Turn private once shortcut-testing is done
    public GameObject snowballToSpawn;
    public GameObject instantiatedSnowball;



    public void initialiseShortcutCutscene() {
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

    }



    // Update is called once per frame
    void Update()
    {
        if (playDebug) {

            initialiseShortcutCutscene();
            phaseNumber = 0;
            playDebug = false;
        }

        if (!GameData.Instance.isCutscene || playingScene==false) return;

        if (waiting) {
            waitTime -= Time.deltaTime;
            if (waitTime < 0)
            {
                waiting = false;
                phases[phaseNumber] = false;
                phaseNumber++;
                if (phases.Count!=phaseNumber) phases[phaseNumber] = true;
            }
            else { return; }
        }

        if (phases[0]) {
            fadeInController.enableShortcutFadeOut(.5f);
            waiting = true;
            waitTime = .45f;//Note this is slightly less then the fade out time so that 
                            //there isn't 1 frame of the wrong map on the screen
        }
        if (phases[1]) {
            setupCutsceneLocation(new Vector3 (49.336f,2,0));
            SceneManager.LoadScene("Sc_Map2-3", LoadSceneMode.Additive);

            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = .5f;
        }
        if (phases[2]) {
            instantiatedSnowball = Instantiate(snowballToSpawn, new Vector3(57.336f, 5, 0), Quaternion.identity);
            
            boulderMover movingBoulder= instantiatedSnowball.GetComponent<boulderMover>();
            movingBoulder.isOnCutsceneMap = true;
            movingBoulder.facedDirection = SpriteMovement.DirectionMoved.DOWN;
            movingBoulder.moving = true;
            //Drop ball
            //need splash sound effect here
            waiting = true;
            waitTime = 3f;
        }
        if (phases[3]) {
            fadeInController.enableShortcutFadeOut(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[4]) {
            Destroy(instantiatedSnowball);
            GameData.Instance.map1_3toMap2_3Shortcut = true;
            setupNewAfterSnowballMap();
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = 3.75f; //waiting for fade in And seeing the new map
        }
        if (phases[5]) {
            fadeInController.enableShortcutFadeOut(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[6]) {
            setupBackInDungeon();
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[7]) {
            GameData.Instance.isCutscene = false;
            playingScene = false;
        }

    }




    private void setupNewAfterSnowballMap()
    {
        GameObject.Find("Shortcut Controller2_3").GetComponent<map2_3ShortcutController>().setupShortcutForCutscene();
    }


}
