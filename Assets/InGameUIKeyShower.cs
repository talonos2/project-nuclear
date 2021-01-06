﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIKeyShower : MonoBehaviour
{
    // Start is called before the first frame update
    private bool setupKeys;
    private string togglLeftKeyToDisplay;
    private string togglRightKeyToDisplay;
    private string ActivateKeyToDisplay;
    private string healKeyToDisplay;
    public bool toggleLeftKey;
    public bool toggleRightKey;
    public bool activateKey;
    public bool healKey;
    public TextMeshPro keyToShow;
    public GameObject hidePowerKey;
    private bool UiLocalOn;

    void Start()
    {
        if (GameData.Instance.sneakyKeyMap == KeymapType.UNDEFINED) {
            setupKeys = false;
            return;
        }
        if (GameData.Instance.sneakyKeyMap == KeymapType.ARROWS) {
            setupArrowKeyText();
            displayKeys();
            setupKeys = true;
        }
        if (GameData.Instance.sneakyKeyMap == KeymapType.WASD)
        {
            setupWASDKeyText();
            displayKeys();
            setupKeys = true;
        }

    }

    private void displayKeys()
    {
        if (!healKey&&GameData.Instance.PowersGained>0) { hidePowerKey.GetComponent<SpriteRenderer>().enabled = true;
            keyToShow.enabled = true;
        }
        if (healKey) {
            hidePowerKey.GetComponent<SpriteRenderer>().enabled = true;
            keyToShow.enabled = true;
            keyToShow.text = healKeyToDisplay;
             }
        if (toggleLeftKey) { keyToShow.text = togglLeftKeyToDisplay; }
        if (toggleRightKey) { keyToShow.text = togglRightKeyToDisplay; }
        if (activateKey) { keyToShow.text = ActivateKeyToDisplay; }
        
        UiLocalOn = true;
    }
    private void hideKeys() {
        hidePowerKey.GetComponent<SpriteRenderer>().enabled = false;
        keyToShow.enabled = false;
        UiLocalOn = false;
    }

    private void setupWASDKeyText()
    {
        togglLeftKeyToDisplay="<-";
        togglRightKeyToDisplay="->";
        ActivateKeyToDisplay="up";
        healKeyToDisplay="c";
}

    private void setupArrowKeyText()
    {
        togglLeftKeyToDisplay = "a";
        togglRightKeyToDisplay = "d";
        ActivateKeyToDisplay = "c";
        healKeyToDisplay = "s";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.UI_On&& !UiLocalOn) {
            displayKeys();
        }

        if (GameData.Instance.UI_On == false && UiLocalOn) {
            hideKeys();
        }

        if (!setupKeys) {
            if (GameData.Instance.sneakyKeyMap == KeymapType.UNDEFINED)
            {
                setupKeys = false;
                return;
            }
            if (GameData.Instance.sneakyKeyMap == KeymapType.ARROWS)
            {
                setupArrowKeyText();
                displayKeys();
                setupKeys = true;
            }
            if (GameData.Instance.sneakyKeyMap == KeymapType.WASD)
            {
                setupWASDKeyText();
                displayKeys();
                setupKeys = true;
            }
        }

    }
}