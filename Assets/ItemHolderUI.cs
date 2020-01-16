using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderUI : MonoBehaviour
{

    private GameObject itemStored;
    public GameObject itemSpriteHolder;
    public Text itemText;

    public void SetItem(GameObject itemToSet) {
        itemStored = itemToSet;
        InventoryItem ItemDetails = itemStored.GetComponent<InventoryItem>();
        itemSpriteHolder.GetComponent<Image>().sprite = ItemDetails.itemIcon;
        itemText.text = ItemDetails.name;
    }
    public GameObject GetItem() {
        return itemStored;
    }

}
