using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseItemUI : MonoBehaviour
{
    public Text foundItemText;
    public Text currentItemText;
    public Text itemKeepText;
    public Text sendHomeText;
    public GameObject foundItemSprite;
    public GameObject newItemSprite;
    public GameObject equipItemButton;
    public GameObject sendItemHomeButton;
    public Canvas chooseItemUiCanvas;
    private int optionSelected = 0;
    protected float delayReset = .14f;
    protected float delayCounter = 0;
    private CharacterStats playerStats;
    private InventoryItem itemTypeCheck;
    private bool pickingItem;
    private CharacterStats playerData;
    private GameObject rolledItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.fullPause || !pickingItem) { return; }
        

        if (Input.GetButton("SelectNext") || Input.GetButton("SelectPrevious"))
        {
            if (delayCounter <= 0)
            {
                delayCounter = delayReset;
                hideButtonSelection();
                if (optionSelected == 1)
                {
                    optionSelected = 0;
                }
                else { optionSelected = 1; }
                showButtonSelection();
            }
            else
            {
                delayCounter -= Time.deltaTime;
            }
        }        

            if (Input.GetButtonDown("Submit"))
        {
             ChooseItem(); 
        }
    }

    private void showButtonSelection()
    {
        if (optionSelected == 0) { equipItemButton.GetComponent<Image>().enabled = true; }
        if (optionSelected == 1) { sendItemHomeButton.GetComponent<Image>().enabled = true; }
    }

    private void hideButtonSelection()
    {
        if (optionSelected == 0) { equipItemButton.GetComponent<Image>().enabled = false; }
        if (optionSelected == 1) { sendItemHomeButton.GetComponent<Image>().enabled = false; }
    }

    private void ChooseItem()
    {
        if (!pickingItem) { return; }

        if (itemTypeCheck.Weapon)
        {
            if (optionSelected == 0)
            {
                if (playerData.weapon != null) GameData.Instance.townWeapons.Add(playerData.weapon);
                sendGabToTownMessage(playerData.weapon);
                playerData.weapon = rolledItem;
                playerData.setWeaponStats(rolledItem);

            }
            else {
                GameData.Instance.townWeapons.Add(rolledItem);
                sendGabToTownMessage(rolledItem);
            }
        }
        if (itemTypeCheck.Armor)
        {
            if (optionSelected == 0)
            {
                if (playerData.armor != null) GameData.Instance.townArmor.Add(playerData.armor);
                sendGabToTownMessage(playerData.armor);
                playerData.armor = rolledItem;
                playerData.setArmorStats(rolledItem);
            }
            else {
                GameData.Instance.townArmor.Add(rolledItem);
                sendGabToTownMessage(rolledItem);
            }
        }
        if (itemTypeCheck.Accessory)
        {
            if (optionSelected == 0)
            {
                if (playerData.accessory != null) GameData.Instance.townAccessories.Add(playerData.accessory);
                sendGabToTownMessage(playerData.accessory);
                playerData.accessory = rolledItem;
                playerData.setAccessoryStats(rolledItem);
            }
            else {
                GameData.Instance.townAccessories.Add(rolledItem);
                sendGabToTownMessage(rolledItem);
            }
        }
        closeItemPickUI(); 
    }

    public void setupItemChoiceDisplay(CharacterStats playerData, GameObject rolledItem) {
        this.playerData = playerData;
        this.rolledItem = rolledItem;
        GameState.isInBattle = true;
        pickingItem = true;
        chooseItemUiCanvas.enabled = true;
        itemTypeCheck = rolledItem.GetComponent<InventoryItem>();
        SetItemUI();
 

    }

    private void SetItemUI()
    {

        int foundItemStat;
        int oldItemStat;
        int totalStatChange;
        String foundItem = "";
        String oldItemText = "No Item Equipped";        

        if (itemTypeCheck.Weapon) {
            foundItemStat = rolledItem.GetComponent<Weapon>().addAttack;
            oldItemStat = playerData.weapon.GetComponent<Weapon>().addAttack;
            if (rolledItem.name == playerData.weapon.name||foundItemStat == oldItemStat) {
                GameData.Instance.townWeapons.Add(playerData.weapon);
                sendGabToTownMessage(playerData.weapon);
                closeItemPickUI();
                return;
            }

            totalStatChange = foundItemStat - oldItemStat;
            if (totalStatChange<=0) foundItem = rolledItem.gameObject.name + ", Attack: " + foundItemStat + "(<color=red>" + totalStatChange + "</color>)";
                else foundItem = rolledItem.gameObject.name + ", Attack: " + foundItemStat + "(<color=green>+" + totalStatChange + "</color>)";
            oldItemText = playerData.weapon.gameObject.name + ", Attack: " + oldItemStat;
            
        }

        if (itemTypeCheck.Armor)
        {
            foundItemStat = rolledItem.GetComponent<Armor>().addDefense;
            oldItemStat = playerData.armor.GetComponent<Armor>().addDefense;
            if (rolledItem.name == playerData.armor.name || foundItemStat== oldItemStat)
            {
                GameData.Instance.townArmor.Add(playerData.armor);
                sendGabToTownMessage(playerData.armor);
                closeItemPickUI();
                return;
            }
         
            totalStatChange = foundItemStat - oldItemStat;
            if (totalStatChange <= 0) foundItem = rolledItem.gameObject.name + ", Defense: " + foundItemStat + "(<color=red>" + totalStatChange + "</color>)";
                else foundItem = rolledItem.gameObject.name + ", Defense: " + foundItemStat + "(<color=green>+" + totalStatChange + "</color>)";
            oldItemText = playerData.armor.gameObject.name + ", Defense: " + oldItemStat;
        }

        if (itemTypeCheck.Accessory) {
            if (rolledItem.name == playerData.accessory.name)
            {
                GameData.Instance.townAccessories.Add(playerData.accessory);
                sendGabToTownMessage(playerData.accessory);
                closeItemPickUI();
                return;
            }
        }

        foundItemText.text = foundItem;
        currentItemText.text = oldItemText;
        foundItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
        newItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
        itemKeepText.text = "Equip " + rolledItem.gameObject.name;
        sendHomeText.text = "Send " + rolledItem.gameObject.name + " Home";

    }

    private void sendGabToTownMessage(GameObject itemSent)
    {
        //throw new NotImplementedException();
    }


    private void closeItemPickUI()
    {

        chooseItemUiCanvas.enabled = false;
        GameState.isInBattle = false;
        pickingItem = false;
    }
}



/*
        if (itemTypeCheck.Weapon)
        {
            if (playerData.weapon != null) GameData.Instance.townWeapons.Add(playerData.weapon);
            playerData.weapon = rolledItem;
            playerData.setWeaponStats(rolledItem);
        }
        else if (itemTypeCheck.Armor)
        {
            if (playerData.armor != null) GameData.Instance.townArmor.Add(playerData.armor);
            playerData.armor = rolledItem;
            playerData.setArmorStats(rolledItem);
        }
        else if (itemTypeCheck.Accessory)
        {
            if (playerData.accessory != null) GameData.Instance.townAccessories.Add(playerData.accessory);
            playerData.accessory = rolledItem;
            playerData.setAccessoryStats(rolledItem);
        }
        */


