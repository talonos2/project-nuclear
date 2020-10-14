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
    public bool inOtherMenu;

//    private int currentMenuOptionSelected = -1;

    public GameObject pauseFirstButton;


    void Start()
    {
        //RefreshSelectedOption(0); //TODO: If save game, start on continue.
        
        EventSystem.current.SetSelectedGameObject(null);  //clears first button, then sets it
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    void Update()
    {
        if (Array.IndexOf<Scene>(SceneManager.GetAllScenes(), SceneManager.GetSceneByName("OptionsScreen"))>-1)
        {
            return;
        }
        if (inSaveMenu) return;
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
        Debug.Log("Trying to Quit");
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

    private void SaveGameButtonClicked()
    {
     //   hideButtonSelection();
     //   buttonSelected = 1;
     //   showButtonSelection();
        inOtherMenu = true;
//        loadSaveController.activateLoad(this, true);
    }

    public void LoadGameButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 1;
        //showButtonSelection();
        //canvas.SetActive(false);

        //loadSaveController.LoadGame(0);
//        loadSaveController.activateLoad(this, false);

    }
    public void optionButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 2;
        //showButtonSelection();
        SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
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



}
