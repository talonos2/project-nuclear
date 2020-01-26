using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderUI : MonoBehaviour
{

    private InventoryItem itemStored;
    public GameObject itemSpriteHolder;
    public Text itemText;
    public Text itemStatText;
    private String itemDetailsText;
    private Sprite itemSprite;

    public void SetItem(InventoryItem itemToSet) {
        itemDetailsText = itemToSet.equipmentDescription;
        itemSpriteHolder.GetComponent<Image>().sprite = itemToSet.itemIcon;
        itemSprite= itemToSet.itemIcon;
        itemText.text = itemToSet.name;
        if (itemToSet.Weapon) {
            Weapon tempwpn = (Weapon)itemToSet;
            itemStatText.text = "+" + tempwpn.addAttack + " ATK";
        }
        if (itemToSet.Armor) {
            Armor temparm = (Armor)itemToSet;
            itemStatText.text = "+" + temparm.addDefense + " DEF";
        }
        if (itemToSet.Accessory) {
            itemStatText.text = "Misc Item";
        }

    }

    public String getItemDetails() {
        return itemDetailsText;
    }
    public Sprite GetItemSprite() {
        return itemSprite;
    }
    public InventoryItem GetItem() {
        return itemStored;
    }

}
