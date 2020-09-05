using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{


    public LoadSaveController loadSaveController;

    public Image selectionMarker;
    public Image[] menuOptions;
    public Sprite[] onImages;
    public Sprite[] offImages;

    internal bool inSaveMenu;

    private int currentMenuOptionSelected = -1;

    void Start()
    {
        RefreshSelectedOption(0); //TODO: If save game, start on continue.
    }

    void Update()
    {
        if (Array.IndexOf<Scene>(SceneManager.GetAllScenes(), SceneManager.GetSceneByName("OptionsScreen"))>-1)
        {
            return;
        }
        if (inSaveMenu) return;

        if (Input.GetButtonDown("Submit"))
        {
            //Debug.Log("Hit submit");
            switch(currentMenuOptionSelected)
            {
                case 0:
                    //SoundManager.Instance.PlaySound("MenuOkay", 1f);
                    //StartNewGame();
                    break;
                case 1:
                    SoundManager.Instance.PlaySound("MenuOkay", 1f);
                    //loadSaveController.activateLoad(this);
                    //inSaveMenu = true;
                    break;
                    //SoundManager.Instance.PlaySound("MenuOkay", 1f);
                    //loadSaveController.LoadGame(0);
                    //break;
                case 2:
                    OpenOptionsMenu();
                    break;
                case 3:
                    
                    break;
                case 4:
                    
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
        SoundManager.Instance.PlaySound("MenuOkay", 1f);
        SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
    }

    private int PrevMenuOption()
    {
        return (currentMenuOptionSelected + 5)%6;  //TODO: Skip continue and load game if there's no game to load or continue.
    }

    private int NextMenuOption()
    {
        return (currentMenuOptionSelected + 7)%6;
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
 


    

    internal void ReActivate()
    {
        inSaveMenu = false;

    }

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
}
