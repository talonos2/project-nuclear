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
    protected float delayReset = .2f;
    protected float delayCounter = .3f;
    private CharacterStats playerStats;
    private bool pickingItem;
    private CharacterStats playerData;
    private InventoryItem rolledItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.fullPause || !pickingItem) { return; }
        

        if (Input.GetAxis("Vertical")>.05f )
        {
            hideButtonSelection();
            optionSelected = 0;
            showButtonSelection();
        }

        if (Input.GetAxis("Vertical") < -.05f)
        {
            hideButtonSelection();
            optionSelected = 1;
            showButtonSelection();
        }

        /*
         * 
         * if (delayCounter <= 0)
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
         * */


        /* if (Input.GetButtonDown("Vertical") )
         {

                 delayCounter = delayReset+.16f;
                 hideButtonSelection();
                 if (optionSelected == 1)
                 {
                     optionSelected = 0;
                 }
                 else { optionSelected = 1; }
                 showButtonSelection();


         }*/

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
        SetItemUI();
    }

    private void SetItemUI()
    {

        int foundItemStat;
        int oldItemStat;
        int totalStatChange;
        String foundItem = "";
        String oldItemText = "No Item Equipped";        

        if (rolledItem is Weapon) {
            foundItemStat = rolledItem.GetComponent<Weapon>().addAttack;
            oldItemStat = playerData.weapon.GetComponent<Weapon>().addAttack;
            if (rolledItem.name == playerData.weapon.name||foundItemStat == oldItemStat) {
                return;
            }

            totalStatChange = foundItemStat - oldItemStat;
            if (totalStatChange<=0) foundItem = rolledItem.gameObject.name + ", Attack: " + foundItemStat + "(<color=red>" + totalStatChange + "</color>)";
                else foundItem = rolledItem.gameObject.name + ", Attack: " + foundItemStat + "(<color=green>+" + totalStatChange + "</color>)";
            oldItemText = playerData.weapon.gameObject.name + ", Attack: " + oldItemStat;
            
        }
        else if (rolledItem is Armor)
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
        else if (rolledItem is Accessory) {
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

    private void sendGabToTownMessage(InventoryItem itemSent)
    {
        //throw new NotImplementedException();
    }


    private void closeItemPickUI()
    {

        chooseItemUiCanvas.enabled = false;
        GameState.isInBattle = false;
        pickingItem = false;
    }

    private void SendToTown(Weapon i)
    {
        if (i)
        {
            if (!GameData.bestWeaponFound || GameData.bestWeaponFound < i)
            {
                GameData.bestWeaponFound = i;
            }
            GameData.Instance.townWeapons.Add(i);
            sendGabToTownMessage(i);
        }
        closeItemPickUI();
    }

    private void SendToTown(Armor i)
    {
        if (i)
        {
            if (!GameData.bestArmorFound || (GameData.bestArmorFound < i))
            {
                GameData.bestArmorFound = i;
            }
            GameData.Instance.townArmor.Add(i);
            sendGabToTownMessage(i);
        }
        closeItemPickUI();
    }

    private void SendToTown(Accessory i)
    {
        if (i)
        {
            if (!GameData.bestAccessoryFound || (GameData.bestAccessoryFound < i))
            {
                GameData.bestAccessoryFound = i;
            }
            GameData.Instance.townAccessories.Add(i);
            sendGabToTownMessage(i);
        }
        closeItemPickUI();
    }
}


