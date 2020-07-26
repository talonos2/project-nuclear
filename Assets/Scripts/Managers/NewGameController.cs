using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameController : MonoBehaviour
{

    public GameObject RunNumberTextField;
    public GameObject dropDown;

    public GameObject PerfeToggle;
    public GameObject WorstToggle;
    public GameObject DouglassToggle;
    public GameObject SaraToggle;
    public GameObject McDermitToggle;
    public GameObject ToddToggle;
    public GameObject NormaToggle;
    public GameObject DerringerToggle;
    public GameObject MelvardiusToggle;
    public GameObject MaraToggle;
    public GameObject DevonToggle;
    public GameObject PendletonToggle;

    public Vector2Int Map1EntrancePoint;
    public Vector2Int TownSpawnPosition;
    public SpriteMovement.DirectionMoved TownSpwanFacing;
    public SpriteMovement.DirectionMoved Map1Facing;
    public LoadSaveController loadSaveController;

    public Image selectionMarker;
    public Image[] menuOptions;
    public Sprite[] onImages;
    public Sprite[] offImages;

    private int currentMenuOptionSelected = -1;

    void Start()
    {
        RefreshSelectedOption(0); //TODO: If save game, start on continue.
    }

    void Update()
    {

        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("Hit submit");
            switch(currentMenuOptionSelected)
            {
                case 0:
                    StartNewGame();
                    break;
                case 1:
                    //TODO: This
                    break;
                case 2:
                    SoundManager.Instance.PlaySound("MenuOkay", 1f);
                    loadSaveController.activateLoad();
                    break;
                case 3:
                    //TODO: This
                    break;
                case 4:
                    //TODO: This
                    break;
            }
        }

        //Selecting Up
        if (Input.GetButtonDown("SelectUp"))
        {
            RefreshSelectedOption(PrevMenuOption());
        }


        //SelectingDownOption
        if (Input.GetButtonDown("SelectDown"))
        {
            RefreshSelectedOption(NextMenuOption());
        }
    }

    private int PrevMenuOption()
    {
        return (currentMenuOptionSelected + 4)%5;  //TODO: Skip continue and load game if there's no game to load or continue.
    }

    private int NextMenuOption()
    {
        return (currentMenuOptionSelected + 6)%5;
    }

    public void RefreshSelectedOption(int menuOptionSelected)
    {
        if (menuOptionSelected != currentMenuOptionSelected && currentMenuOptionSelected != -1)
        {
            SoundManager.Instance.PlaySound("MenuMove", 1f);
        }
        //TODO: Skip continue and load game if there's no game to load or continue. (Repeat check here because of mouse.)
        currentMenuOptionSelected = menuOptionSelected;
        selectionMarker.GetComponent<RectTransform>().localPosition = new Vector3(-70, -110 - 20 * menuOptionSelected, 0);

        for (int x = 0; x < menuOptions.Length; x++)
        {
            menuOptions[x].sprite = offImages[x];
        }
        menuOptions[menuOptionSelected].sprite = onImages[menuOptionSelected];
    }

    public void StartNewGameActual() {
        if (GameData.Instance.RunNumber==1) {
            CutsceneLoader.introCutscene = true;
            GameData.Instance.FloorNumber = 0;
            CutsceneLoader.LoadCutscene();
        }
        if (GameData.Instance.RunNumber > 1 && GameData.Instance.RunNumber <= 30) {
            GameData.Instance.SetNextLocation(TownSpawnPosition, TownSpwanFacing);
            GameData.Instance.FloorNumber = 0;
            FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
            fadeout.InitNext("TownMap_1", 1);
        }


    }
    public void StartNewGame (){
        SoundManager.Instance.PlaySound("MenuOkay", 1f);
        Text runVariable=RunNumberTextField.GetComponent <Text> ();
        Dropdown runType = dropDown.GetComponent<Dropdown>();
        

        if (runType.value == 0)
        {
            if (runVariable.text == "" || Convert.ToInt32(runVariable.text) == 1)
            {
                GameData.Instance.RunNumber = 1;
                CutsceneLoader.introCutscene = true;
                GameData.Instance.FloorNumber = 0;
                CutsceneLoader.LoadCutscene();
            }
            else
            {
                GameData.Instance.SetNextLocation(TownSpawnPosition, TownSpwanFacing);
                GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);
                GameData.Instance.FloorNumber = 0;
                SceneManager.LoadScene("TownMap_1");
            }
        }
        if (runType.value == 1) {
            if (runVariable.text == "" || Convert.ToInt32(runVariable.text)==0 )
            {
                GameData.Instance.RunNumber = 1;
                CutsceneLoader.introCutscene = true;
            }
            else {
                //load cutscene runvar-1
                GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);
            }
            CutsceneLoader.LoadCutscene();
            
        }
        if (runType.value == 2) {
            if (runVariable.text != "")
            {
                GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);
            }
            else {
                GameData.Instance.RunNumber = 1;
            }
            GameData.Instance.SetNextLocation(Map1EntrancePoint, Map1Facing);
            GameData.Instance.FloorNumber = 1;
            SceneManager.LoadScene("Map1-1");

        }
        if (runType.value == 3){
            CutsceneLoader.endCutscene = true;

            if (runVariable.text == "" || Convert.ToInt32(runVariable.text) == 0)
            {
                GameData.Instance.RunNumber = 1;
                CutsceneLoader.introCutscene = true;
            }
            else
            {
                
                //load cutscene runvar-1
                GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);

                GameData.Instance.Perfect = Convert.ToInt32(PerfeToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Worst = Convert.ToInt32(WorstToggle.GetComponent<Toggle>().isOn);

                GameData.Instance.Douglass = Convert.ToInt32(DouglassToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Sara = Convert.ToInt32(SaraToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.McDermit = Convert.ToInt32(McDermitToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Todd = Convert.ToInt32(ToddToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Norma = Convert.ToInt32(NormaToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Derringer = Convert.ToInt32(DerringerToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Melvardius = Convert.ToInt32(MelvardiusToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Mara = Convert.ToInt32(MaraToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Devon = Convert.ToInt32(DevonToggle.GetComponent<Toggle>().isOn);
                GameData.Instance.Pendleton = Convert.ToInt32(PendletonToggle.GetComponent<Toggle>().isOn);

            }
            CutsceneLoader.LoadEnding();

        }


    }


}
