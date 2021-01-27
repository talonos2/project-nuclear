using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Naninovel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionScreenController : MonoBehaviour
{
    private int currentMenuOptionSelected = 0;
    PauseMenuController pauseMenuController;

    public Image[] optionsImages;
    public Sprite[] onSprites;
    public Sprite[] offSprites;

    public GameObject optionFirstButton;

    public Image selectionMarker;

    public Transform[] sliders;
    private TextPrinterManager printerMngr;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);  //clears first button, then sets it
        EventSystem.current.SetSelectedGameObject(optionFirstButton);
        GameData.Instance.exitPause = true;
        
        LoadSavedOptionData();

    }

    void Update()
    {
        if (FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE))
        {
            //Debug.Log("Hit submit");
            switch (currentMenuOptionSelected)
            {
                case 0: //Keybindings
                    break;
                case 1: //Music
                    break;
                case 2: //SFX
                    break;
                case 3: //Exit Options
                    CloseOptionsMenu();
                    break;
            }
        }

        //Selecting Up
        if (FWInputManager.Instance.GetKeyDown(InputAction.MENU_UP))
        {
            RefreshSelectedOption(PrevMenuOption());
        }

        //SelectingDownOption
        if (FWInputManager.Instance.GetKeyDown(InputAction.MENU_DOWN))
        {
            RefreshSelectedOption(NextMenuOption());
        }

        if (FWInputManager.Instance.GetKeyDown(InputAction.MENU_LEFT))
        {
            ChangeSelected(-.1f);
        }

        if (FWInputManager.Instance.GetKeyDown(InputAction.MENU_RIGHT))
        {
            ChangeSelected(.1f);
        }

        if (FWInputManager.Instance.GetKeyDown(InputAction.GO_BACK))
        {
            CloseOptionsMenu();
        }
    }

    private void ChangeSelected(float amount)
    {

        changeValuesOfSliders(currentMenuOptionSelected, amount);       
    }

    private void changeValuesOfSliders(int optionSelected, float amount) {
        float oldSliderValue;
        float newSliderValue;
        switch (optionSelected)
        {
            case 1:
                oldSliderValue = MusicManager.instance.GetMusicVolume();
                newSliderValue = Mathf.Clamp(oldSliderValue + amount, .001f, 1f);
                MusicManager.instance.ChangeMusicVolume(newSliderValue);
                sliders[0].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (MusicManager.instance.GetMusicVolume() * 380));
                break;
            case 2:
                oldSliderValue = SoundManager.Instance.soundEffectVolume;
                newSliderValue = Mathf.Clamp(oldSliderValue + amount, .001f, 1f);
                SoundManager.Instance.soundEffectVolume = newSliderValue;
                sliders[1].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (SoundManager.Instance.soundEffectVolume * 380));
                break;
           /* case 3:
                oldSliderValue = printerMngr.PrintSpeed;
                newSliderValue = Mathf.Clamp(oldSliderValue + amount, .001f, 1f);
                printerMngr.SetPrintSpeed(newSliderValue);
                sliders[2].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (printerMngr.PrintSpeed * 380));
                break;*/
        }
    }

    private void SetValueOfSlider(int optionSelected, float amount) {
        switch (optionSelected)
        {
            case 1:
                MusicManager.instance.ChangeMusicVolume(amount);
                sliders[0].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (MusicManager.instance.GetMusicVolume() * 380));
                break;
            case 2:
                SoundManager.Instance.soundEffectVolume = amount;
                sliders[1].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (SoundManager.Instance.soundEffectVolume * 380));
                break;
                /* case 3:
                     oldSliderValue = printerMngr.PrintSpeed;
                     newSliderValue = Mathf.Clamp(oldSliderValue + amount, .001f, 1f);
                     printerMngr.SetPrintSpeed(newSliderValue);
                     sliders[2].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (printerMngr.PrintSpeed * 380));
                     break;*/
        }
    }

    internal void SaveOptions()
    {

        string appPath = Application.dataPath;
        string savedOptions="GameOptions";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;
        OptionSaveManager optionSaver = new OptionSaveManager();
        optionSaver.SetupOptionsSave(SoundManager.Instance.soundEffectVolume, MusicManager.instance.GetMusicVolume());

        if (!Directory.Exists(appPath + "/Saves")) { Directory.CreateDirectory(appPath + "/Saves"); }
        if (!File.Exists(appPath + "/Saves/" + savedOptions + ".binary"))
        {
            saveFile = File.Create(appPath + "/Saves/" + savedOptions + ".binary");
            formatter.Serialize(saveFile, optionSaver);
            saveFile.Close();
        }
        else
        {
            saveFile = File.OpenWrite(appPath + "/Saves/" + savedOptions + ".binary");
            formatter.Serialize(saveFile, optionSaver);
            saveFile.Close();
        }
        GameData.Instance.optionsState = optionSaver;

    }

    private void LoadSavedOptionData()
    {
        string appPath = Application.dataPath;
        if (!Directory.Exists(appPath + "/Saves")) { Directory.CreateDirectory(appPath + "/Saves"); }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;
        OptionSaveManager optionSaver = new OptionSaveManager();
        string savedOptions = "GameOptions";

        if (!File.Exists(appPath + "/Saves/" + savedOptions + ".binary"))
        {

            FileStream saveFile2 = File.Create(appPath + "/Saves/" + savedOptions + ".binary");
            OptionSaveManager optionSaver2 = new OptionSaveManager();
            optionSaver2.SetupOptionsSave(SoundManager.Instance.soundEffectVolume, MusicManager.instance.GetMusicVolume());
            formatter.Serialize(saveFile2, optionSaver2);
            saveFile2.Close();
        }

        saveFile = File.Open(appPath + "/Saves/" + savedOptions + ".binary", FileMode.Open);
        optionSaver = (OptionSaveManager)formatter.Deserialize(saveFile);
        GameData.Instance.optionsState = optionSaver;
        LoadValuesIntoOptions(optionSaver);
        saveFile.Close();

    }

    private void LoadValuesIntoOptions(OptionSaveManager optionSaver)
    {
        SetValueOfSlider(1, optionSaver.musicVolume);
        SetValueOfSlider(2, optionSaver.soundVolume);
    }

    public void CloseOptionsMenu()
    {

        // pauseMenuController.OptionsReturn();
        GameData.Instance.exitPause = false;
        SaveOptions();
        SceneManager.UnloadSceneAsync("OptionsScreen");
    }

    private int PrevMenuOption()
    {
        //%5 is the number of options (n), 4 and 6 is just 1 above or below the number of options (n-1, n+1)
        int n = optionsImages.Length;
        return (currentMenuOptionSelected + n-1) % n;
    }

    private int NextMenuOption()
    {
        int n = optionsImages.Length;
        return (currentMenuOptionSelected + n+1) % n;
    }

    public void RefreshSelectedOption(int menuOptionSelected)
    {
        if (menuOptionSelected != currentMenuOptionSelected && currentMenuOptionSelected != -1)
        {
            SoundManager.Instance.PlaySound("MenuMove", 1f);
        }
        currentMenuOptionSelected = menuOptionSelected;

        selectionMarker.GetComponent<RectTransform>().localPosition = new Vector3(-190, optionsImages[menuOptionSelected].transform.localPosition.y, 0);

        for (int x = 0; x < optionsImages.Length; x++)
        {
            optionsImages[x].sprite = offSprites[x];
        }

        optionsImages[menuOptionSelected].sprite = onSprites[menuOptionSelected];
    }


    public void DragStuff(BaseEventData data)
    {
        PointerEventData pedata = (PointerEventData)data;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), pedata.position, null, out Vector2 currentPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), pedata.pressPosition, null, out Vector2 startPos);

        int boundSlider;
        if (startPos.y < 40)
        {
            boundSlider = 1;
        }
        else
        {
            boundSlider = 0;
        }

        float newSliderValue = Mathf.Clamp((currentPos.x - 60) / 190, .001f, 1f);

        sliders[boundSlider].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (newSliderValue * 380));

        switch (boundSlider)
        {
            case 0:
                MusicManager.instance.ChangeMusicVolume(newSliderValue);
                break;
            case 1:
                SoundManager.Instance.soundEffectVolume = newSliderValue;
                break;
        }
    }

}
