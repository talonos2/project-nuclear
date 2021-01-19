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
    public ShowItemsInMenuController itemSelectionViewerUI;
    private int optionSelected = 0;
    protected float delayReset = .2f;
    protected float delayCounter = .3f;
    private CharacterStats playerStats;
    private bool pickingItem;
    private CharacterStats playerData;
    private InventoryItem rolledItem;

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

        if (FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE))
        {
            ChooseItem();
            //delayBeforePressing = .35f; //So that the next item also has a delay
        }

        if (FWInputManager.Instance.GetKeyDown(InputAction.MENU_RIGHT))
        {
            // if (delayBeforePressing < 0) {
            //optionSelected = 0;
            SelectButton(0);
        }

        if (FWInputManager.Instance.GetKeyDown(InputAction.MENU_LEFT))
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

    public void ChooseItem()
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
                playerData.PushCharacterData();
                itemSelectionViewerUI.ShowSelectedItemAndClose(0);

            }
            else
            {

                SendToTown((Weapon)rolledItem);
                itemSelectionViewerUI.CloseSelectedItemViewer();
            }
        }
        else if (rolledItem is Armor)
        {
            if (optionSelected == 0)
            {

                SendToTown(playerData.armor);
                playerData.setArmor((Armor)rolledItem);
                playerData.PushCharacterData();
                itemSelectionViewerUI.ShowSelectedItemAndClose(1);
            }
            else
            {
                SendToTown((Armor)rolledItem);
                itemSelectionViewerUI.CloseSelectedItemViewer();
            }
        }
        else if (rolledItem is Accessory)
        {
            if (optionSelected == 0)
            {

                SendToTown(playerData.accessory);
                playerData.setAccessory((Accessory)rolledItem);
                playerData.PushCharacterData();
                itemSelectionViewerUI.ShowSelectedItemAndClose(2);
            }
            else
            {

                SendToTown((Accessory)rolledItem);
                itemSelectionViewerUI.CloseSelectedItemViewer();
            }
        }
    }

    public void SetupItemChoiceDisplay(CharacterStats playerData, InventoryItem rolledItem)
    {
        this.playerData = playerData;
        this.rolledItem = rolledItem;
        GameState.isInBattle = true;
        pickingItem = true;
        GameState.pickingItem = true;
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

        if (rolledItem is Weapon)
        {
            foundItemStat = rolledItem.GetComponent<Weapon>().addAttack;
            oldItemStat = playerData.weapon.GetComponent<Weapon>().addAttack;
            if (rolledItem.name == playerData.weapon.name || foundItemStat == oldItemStat)
            {
                SendDuplicateToTown((Weapon)rolledItem);
                //closeItemPickUI();
                return;
            }
            totalStatChange = foundItemStat - oldItemStat;
            if (rolledItem.Rare)
            {
                foundItemText = rolledItem.gameObject.name + "  <color=white><sprite=0> " + foundItemStat + " <size=40>(<color=" + (totalStatChange <= 0 ? "red" : "green") + ">"
    + (totalStatChange > 0 ? "+" : "") + totalStatChange + "</color>)</size></color>";
            }
            else
            {
                foundItemText = "<color=white>" + rolledItem.gameObject.name + "  <sprite=0> " + foundItemStat + " <size=40>(<color=" + (totalStatChange <= 0 ? "red" : "green") + ">"
    + (totalStatChange > 0 ? "+" : "") + totalStatChange + "</color>)</size></color>";
            }

            if (playerData.weapon.Rare)
            {
                oldItemText = "<color=white>Current - </color>" + playerData.weapon.gameObject.name + " <color=white><sprite=0> " + oldItemStat + "</color>";
            }
            else
            {
                oldItemText = "<color=white>Current - " + playerData.weapon.gameObject.name + " <sprite=0> " + oldItemStat + "</color>";
            }


            newItemSprite.sprite = itemIcons[0];
            //oldItemSprite.sprite = itemIcons[0];

        }
        else if (rolledItem is Armor)
        {
            foundItemStat = rolledItem.GetComponent<Armor>().addDefense;
            oldItemStat = playerData.armor.GetComponent<Armor>().addDefense;
            if (rolledItem.name == playerData.armor.name || foundItemStat == oldItemStat)
            {
                SendDuplicateToTown((Armor)rolledItem);
                //closeItemPickUI();
                return;
            }

            totalStatChange = foundItemStat - oldItemStat;

            if (rolledItem.Rare)
            {
                foundItemText = rolledItem.gameObject.name + "  <color=white><sprite=1> " + foundItemStat + " <size=40>(<color=" + (totalStatChange <= 0 ? "red" : "green") + ">"
                + (totalStatChange > 0 ? "+" : "") + totalStatChange + "</color>)</size></color>";
            }
            else
            {
                foundItemText = "<color=white>" + rolledItem.gameObject.name + "  <sprite=1> " + foundItemStat + " <size=40>(<color=" + (totalStatChange <= 0 ? "red" : "green") + ">"
                + (totalStatChange > 0 ? "+" : "") + totalStatChange + "</color>)</size></color>";
            }

            if (playerData.armor.Rare)
            {
                oldItemText = "<color=white>Current - </color>" + playerData.armor.gameObject.name + " <color=white><sprite=" + 1 + "> " + oldItemStat + "</color>";
            }
            else
            {
                oldItemText = "<color=white>Current - " + playerData.armor.gameObject.name + " <sprite=" + 1 + "> " + oldItemStat + "</color>";
            }

            newItemSprite.sprite = itemIcons[1];
            //oldItemSprite.sprite = itemIcons[1];
        }
        else if (rolledItem is Accessory)
        {
            if (rolledItem.name == playerData.accessory.name)
            {
                SendDuplicateToTown((Accessory)rolledItem);
                //closeItemPickUI();
                return;
            }
            Accessory rolledAccItem = (Accessory)rolledItem;
            Accessory oldAccItem = playerData.accessory;

            if (rolledItem.Rare)
            {
                foundItemText = rolledItem.gameObject.name + "<color=white>, " + rolledAccItem.equipmentStatDescription + "</color>";
            }
            else
            {
                foundItemText = "<color=white>" + rolledItem.gameObject.name + ", " + rolledAccItem.equipmentStatDescription + "</color>";
            }

            if (playerData.accessory.Rare)
            {
                oldItemText = "<color=white>Current - </color>" + playerData.accessory.gameObject.name + "<color=white>, " + oldAccItem.equipmentStatDescription + "</color>";
            }
            else
            {
                oldItemText = "<color=white>Current - " + playerData.accessory.gameObject.name + ", " + oldAccItem.equipmentStatDescription + "</color>";
            }

            if (playerData.accessory.name == "No Accessory Equipped") oldItemText = "<color=white>" + playerData.accessory.gameObject.name + "</color>";
            newItemSprite.sprite = itemIcons[2];
            //oldItemSprite.sprite = itemIcons[2];
        }

        this.foundItemText.text = foundItemText;
        currentItemText.text = oldItemText;

        itemSelectionViewerUI.OpenForFoundItemSelection();
        //foundItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
        //newItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
    }

    private void sendGabToTownMessage(string itemSentString, float duration=3f)
    {
        gabTextController.AddItemGabToPlay(itemSentString, duration);
    }


    private void CloseItemPickUI()
    {
        selectedButton = -1;
        SelectButton(1);
        itemSelectionViewerUI.ShowSelectedItemAndClose(-1);
        chooseItemUiCanvas.enabled = false;
        GameState.isInBattle = false;
        GameState.pickingItem = false;
        pickingItem = false;
    }

    private void SendToTown(Weapon i)
    {
        if (i)
        {
            GameData.Instance.itemsFoundThisRun.Add(i);
            if (i.name != "Knife") {
                GameData.Instance.townWeapons.Add(i);
                
            }
            if (i.Rare) sendGabToTownMessage("<sprite=0> " + i.gameObject.name + " <color=white>sent to town.</color>");
            else sendGabToTownMessage("<color=white><sprite=0> " + i.gameObject.name + " sent to town.</color>");
        }
        if (GameData.Instance.timer > 594)
        {
            FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.RETURN_ITEM_ON_TIME);
        }
        CloseItemPickUI();
    }



    private void SendToTown(Armor i)
    {
        if (i)
        {
            GameData.Instance.itemsFoundThisRun.Add(i);
            if (i.name != "Warm Jacket") {
                GameData.Instance.townArmor.Add(i);
                
            }
            if (i.Rare) sendGabToTownMessage("<sprite=1> " + i.gameObject.name + " <color=white>sent to town.</color>");
            else sendGabToTownMessage("<color=white><sprite=1> " + i.gameObject.name + " sent to town.</color>");

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
                if (i.Rare) sendGabToTownMessage("<sprite=2> " + i.gameObject.name + " <color=white>sent to town.</color>");
                else sendGabToTownMessage("<color=white><sprite=2> " + i.gameObject.name + " sent to town.</color>");
            }

        }
        CloseItemPickUI();
    }

    private void SendDuplicateToTown(Weapon i)
    {
        if (i)
        {
            GameData.Instance.itemsFoundThisRun.Add(i);
            GameData.Instance.townWeapons.Add(i);
            if (i.Rare) sendGabToTownMessage("<color=white><size=40><b>Duplicate</b></size> item found that is already equipped.</color> <sprite=0> " + i.gameObject.name + " <color=white>sent to town.</color>", duration: 5f);
            else sendGabToTownMessage("<color=white><size=40><b>Duplicate</b></size> item found that is already equipped. <sprite=0> " + i.gameObject.name + " sent to town.</color>", duration: 5f);
        }
        CloseItemPickUI();
    }

    private void SendDuplicateToTown(Armor i)
    {
        if (i)
        {
            GameData.Instance.itemsFoundThisRun.Add(i);
            GameData.Instance.townArmor.Add(i);
            if (i.Rare) sendGabToTownMessage("<color=white><b>Duplicate</b> item found that is already equipped.</color> <sprite=1> " + i.gameObject.name + " <color=white>sent to town.</color>", duration: 5f);
            else sendGabToTownMessage("<color=white><b>Duplicate</b> item found that is already equipped. <sprite=1> " + i.gameObject.name + " sent to town.</color>", duration: 5f);
            //Debug.Log("r");
        }
        CloseItemPickUI();
    }
    private void SendDuplicateToTown(Accessory i)
    {
        if (i)
        {
            if (i.name != "No Accessory Equipped")
            {
                GameData.Instance.itemsFoundThisRun.Add(i);
                GameData.Instance.townAccessories.Add(i);
                if (i.Rare) sendGabToTownMessage("<color=white><b>Duplicate</b> item found that is already equipped.</color> <sprite=2> " + i.gameObject.name + " <color=white>sent to town.</color>", 5f);
                else sendGabToTownMessage("<color=white><b>Duplicate</b> item found that is already equipped. <sprite=2> " + i.gameObject.name + " sent to town.</color>", duration:5f);
            }

        }
        CloseItemPickUI();
    }

}


