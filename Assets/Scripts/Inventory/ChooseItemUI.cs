using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseItemUI : MonoBehaviour
{
    public TextMeshProUGUI foundItemText;
    public TextMeshProUGUI currentItemText;
    public Image oldItemSprite;
    public Image newItemSprite;
    public Image equipItemButton;
    public Image sendItemHomeButton;
    public Canvas chooseItemUiCanvas;
    private int optionSelected = 0;
    protected float delayReset = .2f;
    protected float delayCounter = .3f;
    private CharacterStats playerStats;
    private bool pickingItem;
    private CharacterStats playerData;
    private InventoryItem rolledItem;
    private float delayBeforePressing = .35f;

    public int selectedButton = -1;
    public Sprite equipOn;
    public Sprite equipOff;
    public Sprite sendOn;
    public Sprite sendOff;
    public Sprite[] itemIcons;

    private GabTextController gabTextController;

    // Start is called before the first frame update
    void Start()
    {
        gabTextController = this.gameObject.GetComponent<GabTextController>();

        SelectButton(1);
        //optionSelected = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.fullPause || !pickingItem) { return; }
        //delayBeforePressing -= Time.deltaTime;

        if (Input.GetButtonDown("Submit"))
        {
            ChooseItem();
            //delayBeforePressing = .35f; //So that the next item also has a delay
        }

        if (Input.GetButtonDown("SelectRight") )
        {
           // if (delayBeforePressing < 0) {
                //optionSelected = 0;
                SelectButton(0);


        }

        if (Input.GetButtonDown("SelectLeft"))
        {
            //optionSelected = 1;
            SelectButton(1);
        }


    }

    public void SelectButton(int buttonToSelect)
    {
        optionSelected = buttonToSelect;
        if (buttonToSelect != selectedButton && selectedButton != -1)
        {
            SoundManager.Instance.PlaySound("MenuMove", 1f);
        }

        switch (buttonToSelect)
        {
            case 0:
                equipItemButton.sprite = equipOn;
                equipItemButton.rectTransform.localScale = Vector3.one * 1.1f;
                sendItemHomeButton.sprite = sendOff;
                sendItemHomeButton.rectTransform.localScale = Vector3.one;
                return;
            case 1:
                equipItemButton.sprite = equipOff;
                equipItemButton.rectTransform.localScale = Vector3.one;
                sendItemHomeButton.sprite = sendOn;
                sendItemHomeButton.rectTransform.localScale = Vector3.one * 1.1f;
                return;
        }
    }

    private void ChooseItem()
    {
        //Debug.Log("What did I pick "+optionSelected);
        if (!pickingItem) { return; }
        if (optionSelected == 0)
        {
            SoundManager.Instance.PlaySound("MenuNope", 1f);
        }
        else
        {
            SoundManager.Instance.PlaySound("MenuOkay", 1f);
        }

        if (rolledItem is Weapon)
        {
            if (optionSelected == 0)
            {

                
                    SendToTown(playerData.weapon);
                playerData.setWeapon((Weapon)rolledItem);

            }
            else
            {
                
                    SendToTown((Weapon)rolledItem);
            }
        }
        else if (rolledItem is Armor)
        {
            if (optionSelected == 0)
            {
                
                SendToTown(playerData.armor);
                playerData.setArmor((Armor)rolledItem);
            }
            else
            {
                    SendToTown((Armor)rolledItem);
            }
        }
        else if (rolledItem is Accessory)
        {
            if (optionSelected == 0)
            {
                
                SendToTown(playerData.accessory);
                playerData.setAccessory((Accessory)rolledItem);
            }
            else {

                    SendToTown((Accessory)rolledItem);
            }
        }
    }

    public void setupItemChoiceDisplay(CharacterStats playerData, InventoryItem rolledItem) {
        this.playerData = playerData;
        this.rolledItem = rolledItem;
        GameState.isInBattle = true;
        pickingItem = true;
        chooseItemUiCanvas.enabled = true;
        SelectButton(1);
        SetItemUI();
    }

    private void SetItemUI()
    {

        int foundItemStat;
        int oldItemStat;
        int totalStatChange;
        String foundItemText = "";
        String oldItemText = "No Item Equipped";        

        if (rolledItem is Weapon) {
            foundItemStat = rolledItem.GetComponent<Weapon>().addAttack;
            oldItemStat = playerData.weapon.GetComponent<Weapon>().addAttack;
            if (rolledItem.name == playerData.weapon.name||foundItemStat == oldItemStat) {
                SendToTown((Weapon)rolledItem);
                //closeItemPickUI();
                return;
            }
            totalStatChange = foundItemStat - oldItemStat;
            foundItemText = rolledItem.gameObject.name + "  <sprite=0> " + foundItemStat + " <size=40>(<color=" + (totalStatChange <= 0 ? "red" : "green") + ">"
                + (totalStatChange > 0 ? "+":"") + totalStatChange + "</color>)</size>";
            oldItemText = "Current - " + playerData.weapon.gameObject.name + " <sprite=0> " + oldItemStat;
            newItemSprite.sprite = itemIcons[0];
            //oldItemSprite.sprite = itemIcons[0];

        }
        else if (rolledItem is Armor)
        {
            foundItemStat = rolledItem.GetComponent<Armor>().addDefense;
            oldItemStat = playerData.armor.GetComponent<Armor>().addDefense;
            if (rolledItem.name == playerData.armor.name || foundItemStat== oldItemStat)
            {
                SendToTown((Armor)rolledItem);
                //closeItemPickUI();
                return;
            }

            totalStatChange = foundItemStat - oldItemStat;
            foundItemText = rolledItem.gameObject.name + "  <sprite=1> " + foundItemStat + " <size=40>(<color=" + (totalStatChange <= 0 ? "red" : "green") +">"
                + (totalStatChange > 0 ? "+":"")+totalStatChange +"</color>)</size>";
            oldItemText = "Current - "+playerData.armor.gameObject.name + " <sprite=" + 1 + "> " + oldItemStat;
            newItemSprite.sprite = itemIcons[1];
            //oldItemSprite.sprite = itemIcons[1];
        }
        else if (rolledItem is Accessory)
        {
            if (rolledItem.name == playerData.accessory.name)
            {
                SendToTown((Accessory)rolledItem);
                //closeItemPickUI();
                return;
            }
            Accessory rolledAccItem = (Accessory)rolledItem;
            Accessory oldAccItem = playerData.accessory;
            foundItemText = rolledItem.gameObject.name+", " + rolledAccItem.equipmentStatDescription; 
            oldItemText = "Current - " + playerData.accessory.gameObject.name + ", " + oldAccItem.equipmentStatDescription;
            if (playerData.accessory.name == "No Accessory Equipped") oldItemText = playerData.accessory.gameObject.name;
                newItemSprite.sprite = itemIcons[2];
            //oldItemSprite.sprite = itemIcons[2];
        }

        this.foundItemText.text = foundItemText;
        currentItemText.text = oldItemText;
        //foundItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
        //newItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
    }

    private void sendGabToTownMessage(string itemSentString)
    {
        gabTextController.AddGabToPlay(itemSentString);
    }


    private void CloseItemPickUI()
    {
        selectedButton = -1;
        SelectButton(1);
        chooseItemUiCanvas.enabled = false;
        GameState.isInBattle = false;
        pickingItem = false;
    }

    private void SendToTown(Weapon i)
    {
        if (i)
        {
            GameData.Instance.itemsFoundThisRun.Add(i);
            if (playerData.weapon.name != "Knife")
                GameData.Instance.townWeapons.Add(i);
            sendGabToTownMessage("<sprite=0> " + i.gameObject.name + " sent to town.");
        }
        CloseItemPickUI();
    }

    private void SendToTown(Armor i)
    {
        if (i)
        {
            GameData.Instance.itemsFoundThisRun.Add(i);
            if (playerData.armor.name != "Warm Jacket")
                GameData.Instance.townArmor.Add(i);
            sendGabToTownMessage("<sprite=1> " + i.gameObject.name + " sent to town.");
        }
        CloseItemPickUI();
    }

    private void SendToTown(Accessory i)
    {
        if (i)
        {
            if (i.name != "No Accessory Equipped")
            {
                GameData.Instance.itemsFoundThisRun.Add(i);
                GameData.Instance.townAccessories.Add(i);
                sendGabToTownMessage("<sprite=2> " + i.gameObject.name + " sent to town.");
            }

        }
        CloseItemPickUI();
    }
}


