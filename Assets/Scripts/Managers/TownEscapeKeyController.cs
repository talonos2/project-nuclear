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

            if (Input.GetButtonDown("Cancel"))
        {
            if (currentlyEscaped)
            {
                GameState.fullPause = false;
                currentlyEscaped = false;
                canvas.SetActive(false);
            }
            else if (!currentlyEscaped)
            {
                GameState.fullPause = true;
                currentlyEscaped = true;
                canvas.SetActive(true);
            }

        }

        if (currentlyEscaped)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (buttonSelected == 0) { StartRunButtonClicked(); }
                if (buttonSelected == 1) { SaveGameButtonClicked(); }
                if (buttonSelected == 2) { LoadGameButtonClicked(); }
                if (buttonSelected == 3) { optionButtonClicked(); }
                if (buttonSelected == 4) { MenuButtonClicked(); }
                if (buttonSelected == 5) { ExitGameButtonClicked(); }
            }
            if (Input.GetButtonDown("SelectNext"))
            {

                delayCounter = delayReset + .3f;
                hideButtonSelection();
                buttonSelected += 1;
                if (buttonSelected >= selected.Length) { buttonSelected = 0; }
                showButtonSelection();


            }
            if (Input.GetButtonDown("SelectPrevious"))
            {

                delayCounter = delayReset + .3f;
                hideButtonSelection();
                buttonSelected -= 1;
                if (buttonSelected < 0) { buttonSelected = selected.Length - 1; }
                showButtonSelection();


            }
            if (Input.GetButton("SelectNext"))
            {
                if (delayCounter <= 0)
                {
                    delayCounter = delayReset;
                    hideButtonSelection();
                    buttonSelected += 1;
                    if (buttonSelected >= selected.Length) { buttonSelected = 0; }
                    showButtonSelection();
                }
                else
                {
                    delayCounter -= Time.deltaTime;
                }
            }
            if (Input.GetButton("SelectPrevious"))
            {
                if (delayCounter <= 0)
                {
                    delayCounter = delayReset;
                    hideButtonSelection();
                    buttonSelected -= 1;
                    if (buttonSelected < 0) { buttonSelected = selected.Length - 1; }
                    showButtonSelection();
                }
                else
                {
                    delayCounter -= Time.deltaTime;
                }
            }
        }
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
        CutsceneLoader.LoadCutsceneAndFade(this.GetComponent<Canvas>(), .5f);        
        //StartDungeonRun.StartRun();
        //Load 'load game' ui screen
    }

    private void SaveGameButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 1;
        showButtonSelection();
        inOtherMenu = true;
        loadSaveController.activateLoad(this, true);
    }
    public void LoadGameButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 2;
        showButtonSelection();
        inOtherMenu = true;
        canvas.SetActive(false);
        loadSaveController.activateLoad(this, false);
        //Load 'load game' ui screen
    }
    public void optionButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 3;
        showButtonSelection();
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