using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShortcutCutsceneMap3_1to3_4 : ShortcutPlayer
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

        // startShortcutCutscene = true;
        setupPlayerObject();
        playingScene = true;
        GameData.Instance.isCutscene = true;

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
            setupCutsceneLocation(new Vector3(49f, -3, 0));
            SceneManager.LoadScene("SC_Map3-4", LoadSceneMode.Additive);

            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = .5f;
        }
        if (phases[2])
        {
            instantiatedSnowball = Instantiate(snowballToSpawn, new Vector3(49f, 2, 0), Quaternion.identity);

            boulderMover movingBoulder = instantiatedSnowball.GetComponent<boulderMover>();
            instantiatedSnowball.GetComponentInChildren<SpriteShadowLoader>().setOnCutsceneMap();
            instantiatedSnowball.GetComponentInChildren<SpriteShadowLoader>().isOnCutsceneMap = true;
            instantiatedSnowball.GetComponentInChildren<Renderer>().material.SetInt("_HasEmissive", 1);
            movingBoulder.isOnCutsceneMap = true;
            movingBoulder.facedDirection = SpriteMovement.DirectionMoved.DOWN;
            movingBoulder.moving = true;
            //Drop ball. wind sound affect too?
            //need splash sound effect here
            waiting = true;
            waitTime = 1.75f;
        }
        if (phases[3])
        {
            fadeInController.enableShortcutFadeOut(.25f);
            waiting = true;
            waitTime = .2f;
        }
        if (phases[4])
        {
            Destroy(instantiatedSnowball);
            GameData.Instance.map3_4Shortcut = true;
            setupNewAfterSnowballMap();
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = 3f; //waiting for fade in And seeing the new map
        }
        if (phases[5])
        {
            fadeInController.enableShortcutFadeOut(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[6])
        {
            setupBackInDungeon();
            fadeInController.enableShortcutFadeIn(.5f);
            waiting = true;
            waitTime = .45f;
        }
        if (phases[7])
        {
            GameData.Instance.isCutscene = false;
            playingScene = false;
        }
    }

    private void setupNewAfterSnowballMap()
    {
        GameObject.Find("ShortcutController 3_4").GetComponent<shortcut3_4controller>().SetShortcut();
    }
}
