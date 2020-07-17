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

    void Update()
    {
        if (GameData.Instance.isCutscene ) return;

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
                if (buttonSelected == 1) { LoadGameButtonClicked(); }
                if (buttonSelected == 2) { optionButtonClicked(); }
                if (buttonSelected == 3) { MenuButtonClicked(); }
                if (buttonSelected == 4) { ExitGameButtonClicked(); }
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
        CutsceneLoader.LoadCutscene();        
        //StartDungeonRun.StartRun();
        //Load 'load game' ui screen
    }
    public void LoadGameButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 1;
        showButtonSelection();
        //Load 'load game' ui screen
    }
    public void optionButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 2;
        showButtonSelection();
        //Load 'options' ui screen
    }
    public void MenuButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 3;
        showButtonSelection();
        GameState.fullPause = false;
        SceneManager.LoadScene("TitleScreen");
        //load scene
    }
    public void ExitGameButtonClicked()
    {
        hideButtonSelection();
        buttonSelected = 4;
        showButtonSelection();
        GameState.fullPause = false;
        Application.Quit();
    }
}