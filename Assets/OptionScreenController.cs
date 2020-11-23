using System;
using System.Collections;
using System.Collections.Generic;
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

        sliders[0].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (MusicManager.instance.GetMusicVolume() * 380));
        sliders[1].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (SoundManager.Instance.soundEffectVolume * 380));
        //sliders[2].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (printerMngr.PrintSpeed * 380));

//        Debug.Log("Initializing to "+MusicManager.instance.GetMusicVolume() + ", " + SoundManager.Instance.soundEffectVolume + ", " + printerMngr.PrintSpeed);
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
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
                case 3: //Text Speed
                    break;
                case 4: //Resolution
                    break;
                case 5: //Exit
                    CloseOptionsMenu();
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

        if (Input.GetButtonDown("SelectLeft"))
        {
            ChangeSelected(-.1f);
        }

        if (Input.GetButtonDown("SelectRight"))
        {
            ChangeSelected(.1f);
        }

        if (Input.GetButtonDown("Cancel")) {
            CloseOptionsMenu();
        }
    }

    private void ChangeSelected(float amount)
    {
        float oldSliderValue;
        float newSliderValue;
        switch (currentMenuOptionSelected)
        {
            case 1:
                oldSliderValue = MusicManager.instance.GetMusicVolume();
                newSliderValue = Mathf.Clamp(oldSliderValue+amount, .001f, 1f);
                MusicManager.instance.ChangeMusicVolume(newSliderValue);
                sliders[0].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (MusicManager.instance.GetMusicVolume() * 380));
                break;
            case 2:
                oldSliderValue = SoundManager.Instance.soundEffectVolume;
                newSliderValue = Mathf.Clamp(oldSliderValue + amount, .001f, 1f);
                SoundManager.Instance.soundEffectVolume = newSliderValue;
                sliders[1].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (SoundManager.Instance.soundEffectVolume * 380));
                break;
            case 3:
                oldSliderValue = printerMngr.PrintSpeed;
                newSliderValue = Mathf.Clamp(oldSliderValue + amount, .001f, 1f);
                printerMngr.SetPrintSpeed(newSliderValue);
                sliders[2].GetComponent<RectTransform>().localPosition = new Vector2(0, 190 - (printerMngr.PrintSpeed * 380));
                break;
        }
    }

    public void CloseOptionsMenu()
    {

       // pauseMenuController.OptionsReturn();
        SceneManager.UnloadSceneAsync("OptionsScreen");
    }

    private int PrevMenuOption()
    {
        return (currentMenuOptionSelected + 5) % 6;
    }

    private int NextMenuOption()
    {
        return (currentMenuOptionSelected + 7) % 6;
    }

    public void RefreshSelectedOption(int menuOptionSelected)
    {
        if (menuOptionSelected != currentMenuOptionSelected && currentMenuOptionSelected != -1)
        {
            SoundManager.Instance.PlaySound("MenuMove", 1f);
        }
        currentMenuOptionSelected = menuOptionSelected;

        selectionMarker.GetComponent<RectTransform>().localPosition =  new Vector3(-190, optionsImages[menuOptionSelected].transform.localPosition.y, 0);

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
        if (startPos.y < 0)
        {
            boundSlider = 2;
        }
        else if (startPos.y < 40)
        {
            boundSlider = 1;
        }
        else
        {
            boundSlider = 0;
        }

        float newSliderValue = Mathf.Clamp((currentPos.x - 60) / 190, .001f, 1f);

        sliders[boundSlider].GetComponent<RectTransform>().localPosition = new Vector2(0, 190-(newSliderValue * 380));

        switch (boundSlider)
        {
            case 0:
                MusicManager.instance.ChangeMusicVolume(newSliderValue);
                break;
            case 1:
                SoundManager.Instance.soundEffectVolume = newSliderValue;
                break;
            case 2:
                printerMngr.SetPrintSpeed(newSliderValue);
                break;
        }
    }

}
