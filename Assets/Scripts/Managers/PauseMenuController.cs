using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{

    public LoadSaveController loadSaveController;


    //    public Image selectionMarker;
    //    public Image[] menuOptions;
    //    public Sprite[] onImages;
    //    public Sprite[] offImages;

    internal bool inSaveMenu;
    internal Canvas canvas;
    public bool inOtherMenu;

//    private int currentMenuOptionSelected = -1;

    public GameObject dungeonFirstButton, townFirstButton, optionsReturn;

    public Button[] buttons;
    public Button abandon, Dungeon, save, load, options, main, exit; 

    Text inOrOut;

    

    void Start()
    {
        //RefreshSelectedOption(0); //TODO: If save game, start on continue.

        

        Scene scene = SceneManager.GetActiveScene();
        canvas = this.gameObject.GetComponent<Canvas>();

        EventSystem.current.SetSelectedGameObject(null);  //clears first button, then sets it

//        inOrOut = Dungeon.GetComponentInChildren<Text>();

        // Debug.Log("basic " + inOrOut + " with .text = " + inOrOut.text);

        if (scene.name == "TownMap_1" || scene.name == "TownInterior_Pub_1" || scene.name == "TownInterior_Church_1"
        || scene.name == "TownInterior_Manor_1" || scene.name == "TownInterior_SeersCottage_1")
        {

            //Image toOff = abandon.gameObject.GetComponent(typeof(Image)) as Image;
            //toOff.enabled = false;
            //abandon.interactable = false;
            //abandon.enabled = false;
            //enter.transform.Translate(-1000, 0, 0);

//            inOrOut.text = "Enter Dungeon";

            EventSystem.current.SetSelectedGameObject(townFirstButton);
        }
        else
        {
//            Image toOff = save.gameObject.GetComponent(typeof(Image)) as Image;
//            toOff.enabled = false;
//            save.enabled = false;
            //toOff = enter.gameObject.GetComponent(typeof(Image)) as Image;
            //toOff.enabled = false;
            //abandon.transform.Translate(-1000, 0, 0);
            //enter.enabled = false;
 //           inOrOut.text = "Abandon Run";
            EventSystem.current.SetSelectedGameObject(dungeonFirstButton);
        }
        

        

    }


    void Update()
    {
        if (Array.IndexOf<Scene>(SceneManager.GetAllScenes(), SceneManager.GetSceneByName("OptionsScreen"))>-1)
        {
            return;
        }
        if (inSaveMenu) return;

        if (EventSystem.current.currentSelectedGameObject==null)
        EventSystem.current.SetSelectedGameObject(optionsReturn);
        /*
                if (Input.GetButtonDown("Submit"))
                {
                    //Debug.Log("Hit submit");
                    switch(currentMenuOptionSelected)
                    {
                        case 0: //save
                            SoundManager.Instance.PlaySound("MenuOkay", 1f);
                            //StartNewGame();
                            break;
                        case 1: //load
                            SoundManager.Instance.PlaySound("MenuOkay", 1f);
                            //loadSaveController.activateLoad(this);
                            inSaveMenu = true;
                            break;
                            SoundManager.Instance.PlaySound("MenuOkay", 1f);
                            loadSaveController.LoadGame(0);
                            break;
                        case 2: //options
                            SoundManager.Instance.PlaySound("MenuOkay", 1f);
                            OpenOptionsMenu();
                            break;
                        case 3: //main menu
                            SoundManager.Instance.PlaySound("MenuOkay", 1f);
                            break;
                        case 4: //abandon run
                            SoundManager.Instance.PlaySound("MenuOkay", 1f);
                            break;
                        case 5: // exit
                            SoundManager.Instance.PlaySound("MenuOkay", 1f);
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

            public void OpenOptionsMenu()
            {
                //SoundManager.Instance.PlaySound("MenuOkay", 1f);
                SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
            }

            private int PrevMenuOption()
            {

                if (currentMenuOptionSelected == 5)
                    currentMenuOptionSelected = (currentMenuOptionSelected + 5) % 6;

                return (currentMenuOptionSelected + 5)%6;  //TODO: Skip continue and load game if there's no game to load or continue.
            }

            private int NextMenuOption()
            {
                currentMenuOptionSelected = (currentMenuOptionSelected + 7) % 6;
                //        if (inDungeon == true && currentMenuOptionSelected == 5)
                if (currentMenuOptionSelected == 5)
                {
                    Debug.Log("if goes off");
                    currentMenuOptionSelected = (currentMenuOptionSelected + 7) % 6;
                }
                return currentMenuOptionSelected;
            }

            public void RefreshSelectedOption(int menuOptionSelected)
            {
                Debug.Log(menuOptionSelected);
                if (menuOptionSelected != currentMenuOptionSelected && currentMenuOptionSelected != -1)
                {
                    SoundManager.Instance.PlaySound("MenuMove", 1f);
                }
                //TODO: Skip continue and load game if there's no game to load or continue. (Repeat check here because of mouse.)
                currentMenuOptionSelected = menuOptionSelected;
                selectionMarker.GetComponent<RectTransform>().localPosition = new Vector3(-170, 100 - 40 * menuOptionSelected, 0);

                for (int x = 0; x < menuOptions.Length; x++)
                {
                    menuOptions[x].sprite = offImages[x];
                }
                menuOptions[menuOptionSelected].sprite = onImages[menuOptionSelected];
                */
    }
 


    
    /*
    internal void ReActivate()
    {
        inSaveMenu = false;

    }*/

    public void Quit()
    {
//        Debug.Log("Trying to Quit");
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    private void OnApplicationQuit()
    {
        if (!Application.isEditor)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }

    public void ExitGameButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 5;
        //showButtonSelection();
        GameState.fullPause = false;
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SaveGameButtonClicked()
    {
     //   hideButtonSelection();
     //   buttonSelected = 1;
     //   showButtonSelection();
     //   inOtherMenu = true;
        foreach (Button flip in buttons)
            flip.enabled = false;
        loadSaveController.ActivateLoad(this, true);
    }

    public void LoadGameButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 1;
        //showButtonSelection();
        //canvas.SetActive(false);

        //loadSaveController.LoadGame(0);
        foreach (Button flip in buttons)
            flip.enabled = false;
        loadSaveController.ActivateLoad(this, false);

    }
    public void optionButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 2;
        //showButtonSelection();
        SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsReturn);
        //Load 'options' ui screen
    }
    public void MenuButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 3;
        //showButtonSelection();
        GameState.fullPause = false;
        //Debug.Log("title screen runs for some reason");
        SceneManager.LoadScene("TitleScreen");
    }

    public void ReActivate()
    {
        foreach (Button flip in buttons)
            flip.enabled = true;
    }

    public void OptionsReturn()
    {
//        Scene scene = SceneManager.GetSceneByName("PauseScreen");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsReturn);
    }

/*    public void DungeonButtonClick()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TownMap_1" || scene.name == "TownInterior_Pub_1" || scene.name == "TownInterior_Church_1"
        || scene.name == "TownInterior_Manor_1" || scene.name == "TownInterior_SeersCottage_1")
        {
            //needs to start cutscene instead
            GameState.fullPause = false;
            CutsceneLoader.LoadCutsceneAndFade(canvas.GetComponent<Canvas>(), .5f);
            //StartDungeonRun.StartRun();
        }
        else
        {
            //endrun code here
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().MurderPlayer();
            //characterController.MurderPlayer();
        }
    }*/

    public void AbandonButtonClick()
    {
        GameState.fullPause = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().MurderPlayer();
    }



    public void EnterButtonClick()
    {
        GameState.fullPause = false;
        CutsceneLoader.LoadCutsceneAndFade(canvas.GetComponent<Canvas>(), .5f);
    }


    public void PlayButtonHoverSound() {
        SoundManager.Instance.PlaySound("MenuMove", 1f);
    }

    public void PlayButtonSelectSound() {
        SoundManager.Instance.PlaySound("MenuOkay", 1f);
    }

}

/*
public void ClosePauseMenuScene() {
    Debug.Log("Did I call close pause scene");
    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("PauseScreen"));
}*/

