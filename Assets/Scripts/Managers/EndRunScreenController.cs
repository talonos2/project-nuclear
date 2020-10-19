﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndRunScreenController : MonoBehaviour
{
    public static float timeBeforeAnimationStartsStatic;
    public float timeBeforeAnimationStarts = 2f;

    private List<ItemHolderUI> items = new List<ItemHolderUI>();
    public ItemHolderUI templateToCopy;
    public RectTransform content;

    public Image carryOnButton;
    public Sprite carryOnOn;
    public Sprite carryOnOff;
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.ChangeEnvironmentVolume(0);
        timeBeforeAnimationStartsStatic = timeBeforeAnimationStarts;
        int y = 0;
        for (int x = GameData.Instance.itemsFoundThisRun.Count-1; x >= 0; x--)
        {
            ItemHolderUI newItemHolder = GameObject.Instantiate(templateToCopy);
            newItemHolder.SetItem(GameData.Instance.itemsFoundThisRun[x]);
            newItemHolder.transform.parent = content;
            newItemHolder.transform.position = new Vector3(0, y++ * 25);
        }
        content.sizeDelta = new Vector2(0, 25 * GameData.Instance.itemsFoundThisRun.Count);
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
        SoundManager.Instance.PlayPersistentSound("EndScreenConfirm", 1f);
        if (GameData.Instance.RunNumber == 1) {
            GameData.Instance.isCutscene = true;
            CutsceneLoader.postRun1Cutscene = true;
        }
        CutsceneLoader.runTownBackDialogue = true;
        GameData.Instance.RunNumber += 1;
        GameData.Instance.FloorNumber = 0;
        NewCrystalLevelController.AddCrystalsPostRun();
        if (GameData.Instance.RunNumber < 31) {
            GameData.Instance.autoSaveStats();
        }
        GameData.Instance.SetNextLocation(new Vector2Int(-4,-13), SpriteMovement.DirectionMoved.DOWN);
        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        fadeout.attachToGUI(canvas);
        if (GameData.Instance.RunNumber < 31)
        { fadeout.InitNext("TownMap_1", 2); }          
        else { fadeout.InitNext("TownMap_FinalRun", 2); }
    }

    public void SetButtonLit(bool on)
    {
        if (on)
        {
            SoundManager.Instance.PlaySound("MenuMove", 1f);
            carryOnButton.sprite = carryOnOn;
        }
        else
        {
            carryOnButton.sprite = carryOnOff;
        }
    }
}
