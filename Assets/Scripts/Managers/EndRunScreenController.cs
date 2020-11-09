using System;
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

    private int weaponCutoff;
    private int armorCutoff;

    private bool preventMassSlaughterOfVillagers;

    // Start is called before the first frame update
    void Start()
    {

        switch (GameData.Instance.PowersGained)
        {
            case 0:
                weaponCutoff = 4;
                armorCutoff = 2;
                break;
            case 1:                //Beat Ice Area
                weaponCutoff = 7;
                armorCutoff = 4;
                break;
            case 2:                //Beat Earth Area
                weaponCutoff = 15;
                armorCutoff = 9;
                break;
            case 3:                //Beat Fire Area
                weaponCutoff = 25;
                armorCutoff = 13;
                break;
            case 4:                //Beat Air Area
                weaponCutoff = 34;
                armorCutoff = 17;
                break;
        }
        if (GameData.Instance.deathBoss1)  //Has accessed final map
        {
            weaponCutoff = 39;
            armorCutoff = 20;
        }
        SoundManager.Instance.ChangeEnvironmentVolume(0);
        timeBeforeAnimationStartsStatic = timeBeforeAnimationStarts;
        int y = 0;

        //GameData.Instance.townArmor.Sort();
        //GameData.Instance.townWeapons.Sort();

        for (int x = GameData.Instance.itemsFoundThisRun.Count-1; x >= 0; x--)
        {
            ItemHolderUI newItemHolder = GameObject.Instantiate(templateToCopy);
            newItemHolder.SetItem(GameData.Instance.itemsFoundThisRun[x]);
            newItemHolder.transform.SetParent(content);
            newItemHolder.GetComponent<RectTransform>().localPosition = new Vector3(0, y++ * 25);
            newItemHolder.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            if (newItemHolder.GetItem() is Weapon)
            {
                if ((newItemHolder.GetItem() as Weapon).addAttack < weaponCutoff)
                {
                    newItemHolder.itemText.color = new Color(.6f, .6f, .6f);
                    newItemHolder.itemStatText.color = new Color(.8f, .25f, .25f);
                }
            }
            if (newItemHolder.GetItem() is Armor)
            {
                if ((newItemHolder.GetItem() as Armor).addDefense < armorCutoff)
                {
                    newItemHolder.itemText.color = new Color(.6f, .6f, .6f);
                    newItemHolder.itemStatText.color = new Color(.8f, .25f, .25f);
                }
            }
        }
        content.sizeDelta = new Vector2(0, -25 * GameData.Instance.itemsFoundThisRun.Count);
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
        if (preventMassSlaughterOfVillagers)
        {
            return;
        }
        this.preventMassSlaughterOfVillagers = true;
        SoundManager.Instance.PlayPersistentSound("EndScreenConfirm", 1f);

        float cumulativeTime = 0;
        for (int x = 0; x < 20; x++)
        {
            float timeThisFloor = GameData.Instance.timesThisRun[x] - cumulativeTime;
            if (timeThisFloor < GameData.Instance.bestTimes[x])
            {
                GameData.Instance.bestTimes[x] = timeThisFloor;
            }
            cumulativeTime += timeThisFloor;
        }

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
