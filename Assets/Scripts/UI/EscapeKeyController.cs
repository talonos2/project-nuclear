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
                SoundManager.Instance.PlaySound("MenuNope", 1f);
                GameState.fullPause = false;
                currentlyEscaped = false;
                CloseOptionsMenu();
                //canvas.SetActive(false);
            }
            else if (!currentlyEscaped && GameState.fullPause != true)
            {
                SoundManager.Instance.PlaySound("MenuOpen", 1f);
                GameState.fullPause = true;
                currentlyEscaped = true;
                OpenPauseMenu();
                //canvas.SetActive(true);
            }

        }
        /*
        if (currentlyEscaped)
        {
            
            if (Input.GetButtonDown("Submit"))
            {
                //save
                Debug.Log(buttonSelected);
                if (buttonSelected == 1) { LoadGameButtonClicked(); } //load
                if (buttonSelected == 2) { /*optionButtonClicked();*/ /* buttonSelected = buttonSelected; } //options
                if (buttonSelected == 3) { MenuButtonClicked(); } //main menu
                                                                  //abandon
                if (buttonSelected == 5) { ExitGameButtonClicked(); }//exit
            }
            if (Input.GetButtonDown("SelectNext"))
            {
                SoundManager.Instance.PlaySound("MenuMove", 1f);
                delayCounter = delayReset + .3f;
                hideButtonSelection();
                buttonSelected += 1;
                if (buttonSelected >= selected.Length) { buttonSelected = 0; }
                showButtonSelection();


            }
            if (Input.GetButtonDown("SelectPrevious"))
            {
                SoundManager.Instance.PlaySound("MenuMove", 1f);
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
        */
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

/*
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
    */
    public void LoadGameButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 1;
        //showButtonSelection();
        //canvas.SetActive(false);
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
        GameState.fullPause = false;
        //Debug.Log("title screen runs for some reason");
        SceneManager.LoadScene("TitleScreen");
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

    internal void ReActivate()
    {
        inOtherMenu = false;
        canvas.SetActive(true);
    }
}