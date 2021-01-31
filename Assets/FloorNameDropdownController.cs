using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorNameDropdownController : MonoBehaviour
{
    public bool inDungeonUi;
    public float animateSpeed = 10;
    private float delayBeforeClose = 0f;
    public bool animateOpen;
    public bool animateClose;
    public bool inPauseMenu;
    public TextMeshProUGUI nameHolder;
    public GameObject panelToDropDown;
    private string floorName;
    private float speedToAnimate;
    void Start()
    {
        UpdateFloorText();
        animateOpen = true;
    }

    private void UpdateFloorText()
    {
        //NOTE: if you edit this list also edit the list in FloorDropoffPrefab
        switch (GameData.Instance.FloorNumber) {
            case 1:
                floorName = "Icy Portcullis";
            break;
            case 2:
                floorName = "Bleak Pit";
                break;
            case 3:
                floorName = "Dispatch Channel";
                break;
            case 4:
                floorName = "Stone Lung";
                break;
            case 5:
                floorName = "Sifting Claws";
                break;
            case 6:
                floorName = "Tangled Corridor";
                break;
            case 7:
                floorName = "Fickle Stanchion";
                break;
            case 8:
                floorName = "Trundle Gyre";
                break;
            case 9:
                floorName = "Slag Canal";
                break;
            case 10:
                floorName = "Reclamation Pool";
                break;
            case 11:
                floorName = "Sleeping Citadel";
                break;
            case 12:
                floorName = "Toothed Tower";
                break;
            case 13:
                floorName = "Fractured Theater";
                break;
            case 14:
                floorName = "Bellows";
                break;
            case 15:
                floorName = "Seeping Drain";
                break;
            case 16:
                floorName = "Precarious Bridge";
                break;
            case 17:
                floorName = "Residuary";
                break;
            case 18:
                floorName = "Vigilant Bypass";
                break;
            case 19:
                floorName = "Warded Hall";
                break;
            case 20:
                floorName = "Control Room";
                break;


        }


        nameHolder.text = floorName;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.getFullPauseStatus() || GameState.isInBattle || GameData.Instance.isInDialogue || GameData.Instance.isCutscene)
        {
            return;
        }
        speedToAnimate = animateSpeed * Time.deltaTime * 17;
        HandleAnimateOpen();
        HandleAnimateClose();
    }

    private void HandleAnimateOpen()
    {
        //Open is 307
        if (animateOpen)
        {
            panelToDropDown.transform.localPosition = new Vector3(panelToDropDown.transform.localPosition.x, panelToDropDown.transform.localPosition.y + speedToAnimate, panelToDropDown.transform.localPosition.z);
            if (panelToDropDown.transform.localPosition.y > -236)
            {
                panelToDropDown.transform.localPosition = new Vector3(panelToDropDown.transform.localPosition.x, -236, panelToDropDown.transform.localPosition.z);
                animateOpen = false;
                delayBeforeClose = 2.5f;
            }
        }
    }

    private void HandleAnimateClose()
    {
        //closed is 373
        if (delayBeforeClose > 0)
        {
            delayBeforeClose -= Time.deltaTime;
            if (delayBeforeClose <= 0)
            {
                animateClose = true;
            }
            return;
        }
        if (animateClose)
        {
            panelToDropDown.transform.localPosition = new Vector3(panelToDropDown.transform.localPosition.x, panelToDropDown.transform.localPosition.y - speedToAnimate, panelToDropDown.transform.localPosition.z);
            if (panelToDropDown.transform.localPosition.y <-350 )
            {
                panelToDropDown.transform.localPosition = new Vector3(panelToDropDown.transform.localPosition.x, -350, panelToDropDown.transform.localPosition.z);
                animateClose = false;
            }
        }
    }
}
