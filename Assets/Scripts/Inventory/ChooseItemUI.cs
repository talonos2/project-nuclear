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
    private float delayBeforePressing = .3f;

    public int selectedButton = -1;
    public Sprite equipOn;
    public Sprite equipOff;
    public Sprite sendOn;
    public Sprite sendOff;
    public Sprite[] itemIcons;

    // Start is called before the first frame update
    void Start()
    {
        SelectButton(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.fullPause || !pickingItem) { return; }
        delayBeforePressing -= Time.deltaTime;

        if (Input.GetButtonDown("Submit"))
        {
            delayBeforePressing = .3f;
            ChooseItem();
        }

        if (Input.GetAxis("Horizontal")>.05f )
        {
            if (delayBeforePressing < 0) {
                optionSelected = 0;
                SelectButton(0);
            }

        }

        if (Input.GetAxis("Horizontal") < -.05f)
        {
            optionSelected = 1;
            SelectButton(1);
        }


    }

    public void SelectButton(int buttonToSelect)
    {
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

                if (playerData.weapon.name != "Knife")
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
                if (playerData.armor.name != "Warm Jacket")
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
                if (playerData.accessory.name != "No Accessory Equipped")
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
                GameData.Instance.townWeapons.Add((Weapon)rolledItem);
                sendGabToTownMessage(rolledItem);
                closeItemPickUI();
                return;
            }
            totalStatChange = foundItemStat - oldItemStat;
            foundItemText = rolledItem.gameObject.name + "   (<sprite="+0+"><color="+(totalStatChange<=0? "red" : "green") +">+"+foundItemStat+"</color>)";
            oldItemText = playerData.weapon.gameObject.name + "  " + oldItemStat;
            newItemSprite.sprite = itemIcons[0];
            //oldItemSprite.sprite = itemIcons[0];

        }
        else if (rolledItem is Armor)
        {
            foundItemStat = rolledItem.GetComponent<Armor>().addDefense;
            oldItemStat = playerData.armor.GetComponent<Armor>().addDefense;
            if (rolledItem.name == playerData.armor.name || foundItemStat== oldItemStat)
            {
                GameData.Instance.townArmor.Add((Armor)rolledItem);
                sendGabToTownMessage(rolledItem);
                closeItemPickUI();
                return;
            }

            totalStatChange = foundItemStat - oldItemStat;
            foundItemText = rolledItem.gameObject.name + "   (<sprite=" + 1 + "><color=" + (totalStatChange <= 0 ? "red" : "green") + ">+" + foundItemStat + "</color>)";
            oldItemText = playerData.armor.gameObject.name + ", Defense: " + oldItemStat;
            newItemSprite.sprite = itemIcons[1];
            //oldItemSprite.sprite = itemIcons[1];
        }
        else if (rolledItem is Accessory)
        {
            if (rolledItem.name == playerData.accessory.name)
            {
                GameData.Instance.townAccessories.Add((Accessory)rolledItem);
                sendGabToTownMessage(rolledItem);
                closeItemPickUI();
                return;
            }
            foundItemText = rolledItem.gameObject.name + " TODO: Brief Desc.";
            oldItemText = playerData.accessory.gameObject.name + " TODO: Brief Desc.";
            newItemSprite.sprite = itemIcons[2];
            //oldItemSprite.sprite = itemIcons[2];
        }

        this.foundItemText.text = foundItemText;
        currentItemText.text = oldItemText;
        //foundItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
        //newItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
    }

    private void sendGabToTownMessage(InventoryItem itemSent)
    {
        //throw new NotImplementedException();
    }


    private void closeItemPickUI()
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


