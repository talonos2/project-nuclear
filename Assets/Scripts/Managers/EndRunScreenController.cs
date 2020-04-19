using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRunScreenController : MonoBehaviour
{
    public static float timeBeforeAnimationStartsStatic;
    public float timeBeforeAnimationStarts = 2f;

    // Start is called before the first frame update
    void Start()
    {
        timeBeforeAnimationStartsStatic = timeBeforeAnimationStarts;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            EndRunAndLoadTown();
        }
    }

    public void EndRunAndLoadTown()
    {
        if (GameData.Instance.RunNumber == 1) {
            GameData.Instance.isCutscene = true;
            CutsceneLoader.postRun1Cutscene = true;
        }
        CutsceneLoader.runTownBackDialogue = true;
        GameData.Instance.RunNumber += 1;
        GameData.Instance.FloorNumber = 0;
        GameData.Instance.autoSaveStats();
        NewCrystalLevelController.SetCrystalBuffs();
        GameData.Instance.SetNextLocation(new Vector2Int(-4,-13), SpriteMovement.DirectionMoved.DOWN);
        SceneManager.LoadScene("TownMap_1");

    }
}
