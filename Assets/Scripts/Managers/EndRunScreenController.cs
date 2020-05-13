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

    public ItemHolderUI bestWeapon;
    public ItemHolderUI bestArmor;
    public ItemHolderUI bestAccessory;

    public Image carryOnButton;
    public Sprite carryOnOn;
    public Sprite carryOnOff;

    // Start is called before the first frame update
    void Start()
    {
        timeBeforeAnimationStartsStatic = timeBeforeAnimationStarts;
        bestWeapon.SetItem(GameData.bestWeaponFound);
        bestArmor.SetItem(GameData.bestArmorFound);
        bestAccessory.SetItem(GameData.bestAccessoryFound);
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
        SoundManager.Instance.PlaySound("MenuOkay");
        if (GameData.Instance.RunNumber == 1) {
            GameData.Instance.isCutscene = true;
            CutsceneLoader.postRun1Cutscene = true;
        }
        CutsceneLoader.runTownBackDialogue = true;
        GameData.Instance.RunNumber += 1;
        GameData.Instance.FloorNumber = 0;
        NewCrystalLevelController.AddCrystalsPostRun();
        GameData.Instance.autoSaveStats();
        GameData.Instance.SetNextLocation(new Vector2Int(-4,-13), SpriteMovement.DirectionMoved.DOWN);
        SceneManager.LoadScene("TownMap_1");
    }

    public void SetButtonLit(bool on)
    {
        if (on)
        {
            SoundManager.Instance.PlaySound("MenuMove");
            carryOnButton.sprite = carryOnOn;
        }
        else
        {
            carryOnButton.sprite = carryOnOff;
        }
    }
}
