using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossSpawnController : MonoBehaviour
{
    public BossSpawnAnimator bossSpawnController1;
    public BossSpawnAnimator bossSpawnController2;
    public BossSpawnAnimator bossSpawnController3;
    public BossSpawnAnimator bossSpawnController4;
    public BossSpawnAnimator finalBossSpawner;
    public GameObject cutscenePlayerPrefab;
    private GameObject instantiatedCutscenePlayer;
    internal int bossCount = 0;
    private GameObject playerObject;
    private Camera mainCamera;
    public float cutsceneDelay=3;
    private float cutsceneTimer;


    protected int phaseNumber = 0;
    public FadeIn fadeInController;
    protected bool waiting;
    protected float waitTime = 0;
    public List<bool> phases = new List<bool>();
    internal bool playingScene;
    public bool debugMode;

    // Start is called before the first frame update
    void Start()
    {
        playerObject= GameObject.FindGameObjectWithTag("Player");
        mainCamera = playerObject.GetComponentInChildren<Camera>();
        phases.Add(true);//Phase 0
        phases.Add(false);//Phase 1
        phases.Add(false);//Phase 2
        phases.Add(false);//Phase 3
        phases.Add(false);//Phase 4
        phases.Add(false);//Phase 5
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            bossCount = 4;
            playingScene = true;
            GameData.Instance.isInDialogue = true;
            GameData.Instance.isCutscene = true;
            debugMode = false;
        }
        if (playingScene == false) { return; }



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

        if (phases[0])
        {
            fadeInController.enableShortcutFadeOut(.4f);
            waiting = true;
            waitTime = .35f;//Note this is slightly less then the fade out time so that 
                            //there isn't 1 frame of the wrong map on the screen
        }
        if (phases[1])
        {
            mainCamera.enabled = false;
            switch (bossCount)
            {
                case 0:
                    instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController1.transform.parent.position + new Vector3(3.5f, 0, 0), Quaternion.identity);
                    break;
                case 1:
                    instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController2.transform.parent.position + new Vector3(-6.5f, 0, 0), Quaternion.identity);
                    break;
                case 2:
                    instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController3.transform.parent.position + new Vector3(-3.5f, 0, 0), Quaternion.identity);
                    break;
                case 3:
                    instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController4.transform.parent.position + new Vector3(6.5f, 0, 0), Quaternion.identity);
                    break;
                case 4:
                    instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, finalBossSpawner.transform.parent.position + new Vector3(0, 2, 0), Quaternion.identity);
                    break;
            }

            fadeInController.enableShortcutFadeIn(.4f);
            waiting = true;
            waitTime = .4f;
        }
        if (phases[2])
        {

            switch (bossCount)
            {
                case 0:
                    bossSpawnController1.spawnBoss = true;
                    break;
                case 1:
                    bossSpawnController2.spawnBoss = true;
                    break;
                case 2:
                    bossSpawnController3.spawnBoss = true;
                    break;
                case 3:
                    bossSpawnController4.spawnBoss = true;
                    break;
                case 4:
                    finalBossSpawner.spawnBoss = true;
                    //Destroy(slimeOnCrystalBase);
                    break;
            }

            waiting = true;
            waitTime = 2.8f;
            if (bossCount == 4) { waitTime = 11.3f; }
        }
        if (phases[3])
        {
            fadeInController.enableShortcutFadeOut(.4f);
            waiting = true;
            waitTime = .35f;
        }
        if (phases[4])
        {
            Destroy(instantiatedCutscenePlayer);
            mainCamera.enabled = true;
            fadeInController.enableShortcutFadeIn(.4f);
            waiting = true;
            waitTime = .4f;
        }
        if (phases[5])
        {
            GameData.Instance.isCutscene = false;
            playingScene = false;
            GameState.isInBattle = false;
            GameData.Instance.isInDialogue = false;
            phases[0]=true;//Phase 0
            phases[1] = false;//Phase 1
            phases[2] = false; ;//Phase 2
            phases[3] = false; ;//Phase 3
            phases[4] = false; ;//Phase 4
            phases[5] = false; ;//Phase 5
            phaseNumber = 0;
            bossCount += 1;
        }

        /*
                cutsceneTimer -= Time.deltaTime;
                if (cutsceneTimer <= 0) {
                    inCutscene = false;
                    GameData.Instance.isInDialogue = false;
                    mainCamera.enabled = true;
                    Destroy(instantiatedCutscenePlayer);
                }
                */
    }

    internal void spawnNextBoss()
    {
        playingScene = true;
        GameData.Instance.isInDialogue = true;
        GameData.Instance.isCutscene = true;
        /*
                switch (bossCount) {
                    case 0:
                        cutsceneTimer = cutsceneDelay;
                        bossSpawnController1.spawnBoss = true;
                        GameData.Instance.isInDialogue = true;
                        inCutscene = true;
                        mainCamera.enabled = false;
                        instantiatedCutscenePlayer=Instantiate(cutscenePlayerPrefab, bossSpawnController1.transform.parent.position+new Vector3(3.5f,0,0), Quaternion.identity);
                        break;
                    case 1:
                        cutsceneTimer = cutsceneDelay;
                        bossSpawnController2.spawnBoss = true;
                        GameData.Instance.isInDialogue = true;
                        inCutscene = true;
                        mainCamera.enabled = false;
                        instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController2.transform.parent.position + new Vector3(-6.5f, 0, 0), Quaternion.identity);
                        break;
                    case 2:
                        cutsceneTimer = cutsceneDelay;
                        bossSpawnController3.spawnBoss = true;
                        GameData.Instance.isInDialogue = true;
                        inCutscene = true;
                        mainCamera.enabled = false;
                        instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController3.transform.parent.position + new Vector3(-3.5f, 0, 0), Quaternion.identity);
                        break;
                    case 3:
                        cutsceneTimer = cutsceneDelay;
                        bossSpawnController4.spawnBoss = true;
                        GameData.Instance.isInDialogue = true;
                        inCutscene = true;
                        mainCamera.enabled = false;
                        instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController4.transform.parent.position + new Vector3(6.5f, 0, 0), Quaternion.identity);
                        break;
                    case 4:
                        cutsceneTimer = cutsceneDelay;
                        finalBossSpawner.spawnBoss = true;
                        GameData.Instance.isInDialogue = true;
                        inCutscene = true;
                        mainCamera.enabled = false;
                        instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, finalBossSpawner.transform.parent.position, Quaternion.identity);
                        break;

                }*/



    }
}
