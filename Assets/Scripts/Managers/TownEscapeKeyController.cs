using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownEscapeKeyController : MonoBehaviour
{
    protected bool currentlyEscaped;
    protected int buttonSelected = 0;
    protected float delayReset = .15f;
    protected float delayCounter = 0;
    public GameObject canvas;
    public GameObject[] selected;
    public LoadSaveController loadSaveController;
    public bool inOtherMenu;


    void Update()
    {
        if (GameData.Instance.isCutscene || inOtherMenu) return;

        if (FWInputManager.Instance.GetKeyDown(InputAction.GO_BACK))
        {

            if (currentlyEscaped)
            {


                    if (GameData.Instance.exitPause)
                    {
                        GameData.Instance.exitPause = false;
                    }
                    else ClosePauseMenu();

                
                
                //canvas.SetActive(false);
            }
            else if (!currentlyEscaped && GameState.fullPause != true)
            {
                SoundManager.Instance.PlaySound("MenuOpen", 1f);
                GameState.fullPause = true;
                currentlyEscaped = true;
                SoundManager.Instance.PlaySound("MenuOkay", 1f);
                SceneManager.LoadScene("PauseScreenTown", LoadSceneMode.Additive);
                //canvas.SetActive(true);
            }



        }
        
    }

    public void ClosePauseMenu() {
        SoundManager.Instance.PlaySound("MenuNope", 1f);
        GameState.fullPause = false;
        GameData.Instance.inPauseMenu = false;
        currentlyEscaped = false;
        SceneManager.UnloadSceneAsync("PauseScreenTown");

    }


    void OnEnable()
    {
        hideButtonSelection();
        buttonSelected = 0;
        showButtonSelection();

    }

    private void showButtonSelection()
    {
        selected[buttonSelected].GetComponent<Image>().enabled = true;
    }
    private void hideButtonSelection()
    {
        selected[buttonSelected].GetComponent<Image>().enabled = false;
    }


    public void StartRunButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 0;
        showButtonSelection();
        GameState.fullPause = false;
        CutsceneLoader.LoadCutsceneAndFade(canvas.GetComponent<Canvas>(), .5f);
        //StartDungeonRun.StartRun();
        //Load 'load game' ui screen
    }

    private void SaveGameButtonClicked()
    {
        GameData.Instance.exitPause = true;
        hideButtonSelection();
        buttonSelected = 1;
        showButtonSelection();
        inOtherMenu = true;
        loadSaveController.ActivateLoad(this, true);
    }
    public void LoadGameButtonClicked()
    {
        GameData.Instance.exitPause = true;
        hideButtonSelection();
        buttonSelected = 2;
        showButtonSelection();
        inOtherMenu = true;
        canvas.SetActive(false);
        loadSaveController.ActivateLoad(this, false);
        //Load 'load game' ui screen
    }
    public void optionButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 3;
        showButtonSelection();
        loadSaveController.newGameController.OpenOptionsMenu();
        //Load 'options' ui screen
    }
    public void MenuButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 4;
        showButtonSelection();
        GameState.fullPause = false;
        SceneManager.LoadScene("TitleScreen");
        //load scene
    }
    public void ExitGameButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 5;
        showButtonSelection();
        GameState.fullPause = false;
        Application.Quit();
    }


    internal void ReActivate()
    {
        inOtherMenu = false;
        canvas.SetActive(true);
    }
}