﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderUI : MonoBehaviour
{

    private InventoryItem itemStored;
    public GameObject itemSpriteHolder;
    public Text itemText;
    private String itemDetailsText;
    private Sprite itemSprite;

    public void SetItem(InventoryItem itemToSet) {
        itemStored = itemToSet;
        InventoryItem ItemDetails = itemStored.GetComponent<InventoryItem>();
        itemDetailsText = ItemDetails.equipmentDescription;
        itemSpriteHolder.GetComponent<Image>().sprite = ItemDetails.itemIcon;
        itemSprite= ItemDetails.itemIcon;
        itemText.text = ItemDetails.name;
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
