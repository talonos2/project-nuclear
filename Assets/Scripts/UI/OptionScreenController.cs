using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Naninovel;
using TMPro;
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

    public TextMeshProUGUI wKey;
    public TextMeshProUGUI aKey;
    public TextMeshProUGUI sKey;
    public TextMeshProUGUI dKey;
    public TextMeshProUGUI lKey;
    public TextMeshProUGUI rKey;
    public TextMeshProUGUI cKey;
    public TextMeshProUGUI typeKey;

    public GameObject optionFirstButton;

    public Image selectionMarker;

    public Transform[] sliders;
    private TextPrinterManager printerMngr;

    private bool youAreAllowedToBeCalledFromHere = false;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);  //clears first button, then sets it
        EventSystem.current.SetSelectedGameObject(optionFirstButton);

        LoadSavedOptionData();
        RefreshKeys();
    }

    void Update()
    {
        if (FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE))
        {
            //Debug.Log("Hit submit");
            switch (currentMenuOptionSelected)
            {
                case 0: //Keybindings
                    youAreAllowedToBeCalledFromHere = true;
                    this.SwitchKeybinds();
                    youAreAllowedToBeCalledFromHere = false;
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
        if (currentMenuOptionSelected == 0)
        {
            SwitchKeybinds();
            return;
        }
        ChangeValuesOfSliders(currentMenuOptionSelected, amount);
    }

    private void ChangeValuesOfSliders(int optionSelected, float amount)
    {
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
        }
    }

    private void SetValueOfSlider(int optionSelected, float amount)
    {
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
        string savedOptions = "GameOptions";
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
        GameData.Instance.exitPause = false;
        SaveOptions();
        
        SceneManager.UnloadSceneAsync("OptionsScreen");
    }

    private int PrevMenuOption()
    {
        int n = optionsImages.Length;
        return (currentMenuOptionSelected + n - 1) % n;
    }

    private int NextMenuOption()
    {
        int n = optionsImages.Length;
        return (currentMenuOptionSelected + n + 1) % n;
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

    public void SwitchKeybinds()
    {
        if (!youAreAllowedToBeCalledFromHere&& FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE))
        {
            return;
        }
        if (FWInputManager.Instance.IsWASD())
        {
            GameData.Instance.sneakyKeyMap = KeymapType.ARROWS;
            FWInputManager.Instance.SetToArrowKeys();
            RefreshKeys();
        }
        else
        {
            GameData.Instance.sneakyKeyMap = KeymapType.WASD;
            FWInputManager.Instance.SetToWASD();
            RefreshKeys();
        }
    }

    public void RefreshKeys()
    {
        if (FWInputManager.Instance.IsWASD())
        {
            wKey.text = "W";
            wKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(wKey.GetComponent<RectTransform>().anchoredPosition.x, -97);
            aKey.text = "A";
            aKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(aKey.GetComponent<RectTransform>().anchoredPosition.x, -123.2f);
            sKey.text = "S";
            sKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(sKey.GetComponent<RectTransform>().anchoredPosition.x, -123.2f);
            dKey.text = "D";
            dKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(dKey.GetComponent<RectTransform>().anchoredPosition.x, -123.2f);
            lKey.text = "←";
            lKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(lKey.GetComponent<RectTransform>().anchoredPosition.x, -182.3f);
            rKey.text = "→";
            rKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(rKey.GetComponent<RectTransform>().anchoredPosition.x, -182.3f);
            cKey.text = "↑";
            cKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(cKey.GetComponent<RectTransform>().anchoredPosition.x, -182.3f);
            typeKey.text = "WASD";
        }
        else
        {
            wKey.text = "↑";
            wKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(wKey.GetComponent<RectTransform>().anchoredPosition.x, wKey.GetComponent<RectTransform>().anchoredPosition.y + 6);
            aKey.text = "←";
            aKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(aKey.GetComponent<RectTransform>().anchoredPosition.x, aKey.GetComponent<RectTransform>().anchoredPosition.y + 6);
            sKey.text = "↓";
            sKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(sKey.GetComponent<RectTransform>().anchoredPosition.x, sKey.GetComponent<RectTransform>().anchoredPosition.y + 6);
            dKey.text = "→";
            dKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(dKey.GetComponent<RectTransform>().anchoredPosition.x, dKey.GetComponent<RectTransform>().anchoredPosition.y + 6);
            lKey.text = "A";
            lKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(lKey.GetComponent<RectTransform>().anchoredPosition.x, lKey.GetComponent<RectTransform>().anchoredPosition.y - 6);
            rKey.text = "D";
            rKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(rKey.GetComponent<RectTransform>().anchoredPosition.x, rKey.GetComponent<RectTransform>().anchoredPosition.y - 6);
            cKey.text = "C";
            cKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(cKey.GetComponent<RectTransform>().anchoredPosition.x, cKey.GetComponent<RectTransform>().anchoredPosition.y - 6);
            typeKey.text = "Arrows";
        }
    }

}
