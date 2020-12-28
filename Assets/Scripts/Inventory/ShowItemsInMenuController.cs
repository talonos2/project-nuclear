using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowItemsInMenuController : MonoBehaviour
{
    public ItemHolderUI weaponUIPrefab;
    public ItemHolderUI armorUIPrefab;
    public ItemHolderUI accessoryUIPrefab;
    public TextMeshProUGUI itemDetailsDescription;
    public GameObject picToMove;
    public GameObject extraTextToMove;
    private bool openForItemSelection;
    private bool flashFinished;
    private CharacterStats savedStats;
    public bool inDungeonUi;
    public float animateSpeed=10;
    private float delayBeforeClose = 0f;
    private bool animateOpen;
    private bool animateClose;
    public bool inPauseMenu;
    private bool animatingDropOpen;
    private bool animatingDropClosed;
    private float speedToAnimate;

    // Start is called before the first frame update
    void Start()
    {
        savedStats = GameObject.Find("GameStateData").GetComponent<CharacterStats>();
        weaponUIPrefab.SetItem(savedStats.weapon, false);
        armorUIPrefab.SetItem(savedStats.armor, false);
        accessoryUIPrefab.SetItem(savedStats.accessory, false);


       
    }

    public void setDescriptionText(int slotNum) {
        //Debug.Log("Am I entering "+slotNum);
        if (!inPauseMenu) { return; }
        weaponUIPrefab.flashingBackground.enabled = false;
        armorUIPrefab.flashingBackground.enabled = false;
        accessoryUIPrefab.flashingBackground.enabled = false;

        if (slotNum == 0) {
            itemDetailsDescription.text = weaponUIPrefab.getItemDetails()+" "+weaponUIPrefab.itemStatText.text;
            weaponUIPrefab.flashingBackground.enabled = true;
        }
        if (slotNum == 1) {
            itemDetailsDescription.text = armorUIPrefab.getItemDetails() + " " + armorUIPrefab.itemStatText.text;
            armorUIPrefab.flashingBackground.enabled = true;
        }
        if (slotNum == 2)
        {
            itemDetailsDescription.text = accessoryUIPrefab.getItemDetails() + " " + accessoryUIPrefab.itemStatText.text;
            accessoryUIPrefab.flashingBackground.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!inDungeonUi) {
            return;
        }

        HandleChangedItemFlashing();
        if (openForItemSelection) { return; }
        speedToAnimate = animateSpeed * Time.deltaTime * 30;
        HandleAnimateOpen();
        HandleAnimateClose();
        HandleDropdownClose();
        HandleDropdownOpen();       
        
    }

    private void HandleChangedItemFlashing()
    {
        if (delayBeforeClose > 0)
        {
            delayBeforeClose -= Time.deltaTime;
            if (delayBeforeClose < .25 && !flashFinished)
            {
                flashFinished = true;
            }
            if (delayBeforeClose < 0)
            {
                weaponUIPrefab.flashingBackground.enabled = false;
                armorUIPrefab.flashingBackground.enabled = false;
                accessoryUIPrefab.flashingBackground.enabled = false;
                openForItemSelection = false;
            }

        }
    }

    private void HandleAnimateOpen()
    {
                //-610 to -435 (open)
        if (animateOpen)
        {
            picToMove.transform.localPosition = new Vector3(picToMove.transform.localPosition.x + speedToAnimate, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
            if (picToMove.transform.localPosition.x > -435)
            {
                picToMove.transform.localPosition = new Vector3(-435, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
                animateOpen = false;
                if (inPauseMenu) { animatingDropOpen = true; }
            }
        }
    }

    private void HandleAnimateClose()
    {
        if (animateClose)
        {
            picToMove.transform.localPosition = new Vector3(picToMove.transform.localPosition.x - speedToAnimate*1.5f, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
            if (picToMove.transform.localPosition.x < -610)
            {
                picToMove.transform.localPosition = new Vector3(-610, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
                animateClose = false;
                weaponUIPrefab.flashingBackground.enabled = false;
                armorUIPrefab.flashingBackground.enabled = false;
                accessoryUIPrefab.flashingBackground.enabled = false;
            }
        }
    }

    private void HandleDropdownClose()
    {
        if (animatingDropClosed)
        {
            extraTextToMove.transform.localPosition = new Vector3(extraTextToMove.transform.localPosition.x, extraTextToMove.transform.localPosition.y + speedToAnimate*1.5f, extraTextToMove.transform.localPosition.z);
            if (extraTextToMove.transform.localPosition.y > 0)
            {
                extraTextToMove.transform.localPosition = new Vector3(extraTextToMove.transform.localPosition.x, 0, extraTextToMove.transform.localPosition.z);
                animatingDropClosed = false;
                animateClose = true;
            }
        }
    }

    private void HandleDropdownOpen()
    {
        if (inPauseMenu)
        {
            if (animatingDropOpen)
            {
                extraTextToMove.transform.localPosition = new Vector3(extraTextToMove.transform.localPosition.x, extraTextToMove.transform.localPosition.y - speedToAnimate, extraTextToMove.transform.localPosition.z);
                if (extraTextToMove.transform.localPosition.y < -120f)
                {
                    extraTextToMove.transform.localPosition = new Vector3(extraTextToMove.transform.localPosition.x, -120f, extraTextToMove.transform.localPosition.z);
                    animatingDropOpen = false;
                }
            }
        }
    }

    public void SetToAnimateOpen() {
        //onMouseEnter
        if (inPauseMenu) { return; }
        animateOpen = true;
            animateClose = false;       

    }
    public void SetToAnimateClose()
    {
        //onMouseExit
        if (inPauseMenu) { return; }
        animateClose = true;
        animateOpen = false;

    }

    internal void SetPauseAnimateOpen()
    {
        animateOpen = true;
        animateClose = false;
        inPauseMenu = true;
        setDescriptionText(0);

    }

    internal void SetPauseAnimateClose()
    {
        animatingDropClosed = true;
        animateOpen = false;
        inPauseMenu = false;
        weaponUIPrefab.flashingBackground.enabled = false;
        armorUIPrefab.flashingBackground.enabled = false;
        accessoryUIPrefab.flashingBackground.enabled = false;
    }


    public void OpenForFoundItemSelection() {
        picToMove.transform.localPosition = new Vector3(-435, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
        openForItemSelection = true;
    }
    public void ShowSelectedItemAndClose(int itemSelected) {

        if (itemSelected!=-1) delayBeforeClose = .75f;
        flashFinished=false;
        animateClose = true;
        if (itemSelected ==0) {
            weaponUIPrefab.SetItem(savedStats.weapon, false);
            weaponUIPrefab.flashingBackground.enabled = true;
        }
        if (itemSelected == 1) {
            armorUIPrefab.SetItem(savedStats.armor, false);
            armorUIPrefab.flashingBackground.enabled = true;
        }
        if (itemSelected == 2)
        {
            accessoryUIPrefab.SetItem(savedStats.accessory, false);
            accessoryUIPrefab.flashingBackground.enabled = true;
        }

    }


}
