using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeKeyController : MonoBehaviour
{
    protected bool currentlyEscaped;
    protected int buttonSelected = 0;
    protected float delayReset = .15f;
    protected float delayCounter = 0;
    public GameObject canvas;
    public GameObject[] selected;
    public LoadSaveController loadSaveController;
    public PauseMenuController pauseMenuController;
    public ShowItemsInMenuController ItemsEquippedUI;
    public bool inOtherMenu;




    // Update is called once per frame
    void Update()
    {
        if (FWInputManager.Instance.GetKeyDown(InputAction.GO_BACK))
        {

            if (currentlyEscaped)
            {

                    if (GameData.Instance.exitPause) {
                        GameData.Instance.exitPause = false;
                        
                    }
                    else ClosePauseMenuCallback();
               
                //canvas.SetActive(false);
            }
            else if (!currentlyEscaped && GameState.getFullPauseStatus() != true)
            {
                SoundManager.Instance.PlaySound("MenuOpen", 1f);
                GameState.setFullPause(true);
                currentlyEscaped = true;
                OpenPauseMenu();
                //canvas.SetActive(true);
            }

        }
 
    }

    public void ClosePauseMenuCallback() {
        SoundManager.Instance.PlaySound("MenuNope", 1f);
        GameState.setFullPause(false);
        GameData.Instance.inPauseMenu = false;
        currentlyEscaped = false;
        CloseOptionsMenu();
    }

    public void OpenPauseMenu()
    {
        SoundManager.Instance.PlaySound("MenuOkay", 1f);
        SceneManager.LoadScene("PauseScreenDungeon", LoadSceneMode.Additive);
        ItemsEquippedUI.SetPauseAnimateOpen();
    }

    public void CloseOptionsMenu()
    {
        SceneManager.UnloadSceneAsync("PauseScreenDungeon");
        ItemsEquippedUI.SetPauseAnimateClose();
    }


    public void LoadGameButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 1;
        //showButtonSelection();
        //canvas.SetActive(false);
        GameData.Instance.exitPause = true;
        loadSaveController.ActivateLoad(this);

    }
    public void optionButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 2;
        //showButtonSelection();
        //SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
        //Load 'options' ui screen
    }
    public void MenuButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 3;
        //showButtonSelection();
        GameState.setFullPause (false);
        //Debug.Log("title screen runs for some reason");
        SceneManager.LoadScene("TitleScreen");
    }


    public void ExitGameButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 5;
        //showButtonSelection();
        GameState.setFullPause(false);
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    internal void ReActivate()
    {
        inOtherMenu = false;
        canvas.SetActive(true);
    }

}