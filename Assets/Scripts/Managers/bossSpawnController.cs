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
    private bool inCutscene;
    public float cutsceneDelay=3;
    private float cutsceneTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerObject= GameObject.FindGameObjectWithTag("Player");
        mainCamera = playerObject.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCutscene) { return; }

        cutsceneTimer -= Time.deltaTime;
        if (cutsceneTimer <= 0) {
            inCutscene = false;
            GameData.Instance.isInDialogue = false;
            mainCamera.enabled = true;
            Destroy(instantiatedCutscenePlayer);
        }

    }

    internal void spawnNextBoss()
    {

        switch (bossCount) {
            case 0:
                cutsceneTimer = cutsceneDelay;
                bossSpawnController1.spawnBoss = true;
                GameData.Instance.isInDialogue = true;
                inCutscene = true;
                mainCamera.enabled = false;
                instantiatedCutscenePlayer=Instantiate(cutscenePlayerPrefab, bossSpawnController1.transform.parent.position, Quaternion.identity);
                break;
            case 1:
                cutsceneTimer = cutsceneDelay;
                bossSpawnController2.spawnBoss = true;
                GameData.Instance.isInDialogue = true;
                inCutscene = true;
                mainCamera.enabled = false;
                instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController2.transform.parent.position, Quaternion.identity);
                break;
            case 2:
                cutsceneTimer = cutsceneDelay;
                bossSpawnController3.spawnBoss = true;
                GameData.Instance.isInDialogue = true;
                inCutscene = true;
                mainCamera.enabled = false;
                instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController3.transform.parent.position, Quaternion.identity);
                break;
            case 3:
                cutsceneTimer = cutsceneDelay;
                bossSpawnController4.spawnBoss = true;
                GameData.Instance.isInDialogue = true;
                inCutscene = true;
                mainCamera.enabled = false;
                instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, bossSpawnController4.transform.parent.position, Quaternion.identity);
                break;
            case 4:
                cutsceneTimer = cutsceneDelay;
                finalBossSpawner.spawnBoss = true;
                GameData.Instance.isInDialogue = true;
                inCutscene = true;
                mainCamera.enabled = false;
                instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, finalBossSpawner.transform.parent.position, Quaternion.identity);
                break;

        }

        bossCount += 1;
    }
}
