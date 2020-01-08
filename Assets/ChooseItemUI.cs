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
            if (optionSelected == 0) { EquipItem(); }
            if (optionSelected == 1) { SendItemToTown(); }
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

    private void SendItemToTown()
    {
        throw new NotImplementedException();
    }

    private void EquipItem()
    {
        throw new NotImplementedException();
    }

    public void setupItemChoiceDisplay(CharacterStats playerData, GameObject rolledItem) {

        //chooseItemUiCanvas.(true);
        pickingItem = true;
        Debug.Log("Do I arrive");
        itemTypeCheck = rolledItem.GetComponent<InventoryItem>();
        SetItemUI(playerData, rolledItem);
        chooseItemUiCanvas.enabled = true;


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

    }

    private void SetItemUI(CharacterStats playerData, GameObject rolledItem)
    {
        /*
          Text foundItemText;
    public Text currentItemText;
    public Text itemKeepText;
    public Text sendHomeText;
    public Image foundItemSprite;
    public Image newItemSprite;*/

        //itemTypeCheck = rolledItem.GetComponent<InventoryItem>();
        //rolledItem
        int foundItemStat;
        int oldItemStat;
        int totalStatChange;
        String foundItem = "";
        String oldItemText = "No Item Equipped";
            

        if (itemTypeCheck.Weapon) {
            foundItemStat = rolledItem.GetComponent<Weapon>().addAttack;
            oldItemStat = playerData.weapon.GetComponent<Weapon>().addAttack;
            totalStatChange = foundItemStat - oldItemStat;
            if (totalStatChange<=0) foundItem = rolledItem.gameObject.name + ", Attack: " + foundItemStat + "(<color=red>" + totalStatChange + "</color>)";
                else foundItem = rolledItem.gameObject.name + ", Attack: " + foundItemStat + "(<color=green>+" + totalStatChange + "</color>)";
            oldItemText = playerData.weapon.gameObject.name + ", Attack: " + oldItemStat;
        }
        if (itemTypeCheck.Armor)
        {
            foundItemStat = rolledItem.GetComponent<Armor>().addDefense;
            oldItemStat = playerData.weapon.GetComponent<Armor>().addDefense;
            totalStatChange = foundItemStat - oldItemStat;
            if (totalStatChange <= 0) foundItem = rolledItem.gameObject.name + ", Defense: " + foundItemStat + "(<color=red>" + totalStatChange + "</color>)";
                else foundItem = rolledItem.gameObject.name + ", Defense: " + foundItemStat + "(<color=green>+" + totalStatChange + "</color>)";
        }
        if (itemTypeCheck.Accessory) { Debug.Log("acc"); }

        foundItemText.text = foundItem;
        foundItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
        newItemSprite.GetComponent<Image>().sprite = rolledItem.GetComponent<InventoryItem>().itemIcon;
        Debug.Log("endlog");
    }
}
