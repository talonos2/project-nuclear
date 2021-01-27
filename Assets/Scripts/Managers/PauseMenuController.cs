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

    private bool inOptionsMenu;

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

            EventSystem.current.SetSelectedGameObject(townFirstButton);
        }
        else
        {

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
        if (inOptionsMenu) {
            inOptionsMenu = false;
            ReActivate();
        }

        if (EventSystem.current.currentSelectedGameObject==null)
        EventSystem.current.SetSelectedGameObject(optionsReturn);
       
    }


    public void DeActivateButtons() {
        foreach (Button flip in buttons)
            flip.enabled = false;
    }
    
    /*
    internal void ReActivate()
    {
        inSaveMenu = false;

    }*/

    public void Quit()
    {
        //        Debug.Log("Trying to Quit");
        DeActivateButtons();
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

        DeActivateButtons();
        GameState.fullPause = false;
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void continueButtonClicked() {
        DeActivateButtons();
        GameObject escMenu=GameObject.Find("EscapeMenuUi");
        if (escMenu) {
            escMenu.GetComponent<EscapeKeyController>().ClosePauseMenuCallback();
        }
    }
    public void SaveGameButtonClicked()
    {
        //   hideButtonSelection();
        //   buttonSelected = 1;
        //   showButtonSelection();
        //   inOtherMenu = true;
        DeActivateButtons();
        GameData.Instance.exitPause = true;
        loadSaveController.ActivateLoad(this, true);
    }

    public void LoadGameButtonClicked()
    {
        DeActivateButtons();
        GameData.Instance.exitPause = true;
        loadSaveController.ActivateLoad(this, false);

    }
    public void optionButtonClicked()
    {

        DeActivateButtons();
        SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
        inOptionsMenu = true;
        GameData.Instance.exitPause = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsReturn);
        //Load 'options' ui screen
    }
    public void MenuButtonClicked()
    {
        //hideButtonSelection();
        //buttonSelected = 3;
        //showButtonSelection();
        //DeActivateButtons();
        GameState.fullPause = false;
        GameData.Instance.inDungeon = false;
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

    


    public void AbandonButtonClick()
    {
        DeActivateButtons();
        GameState.fullPause = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().MurderPlayer();
        GameObject.Find("EscapeMenuUi").GetComponent<EscapeKeyController>().ClosePauseMenuCallback();
    }



    public void EnterButtonClick()
    {
        GameData.Instance.inDungeon = true;
        DeActivateButtons();
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

