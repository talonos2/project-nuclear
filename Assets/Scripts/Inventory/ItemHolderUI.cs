using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderUI : MonoBehaviour
{

    private InventoryItem itemStored;
    public GameObject itemSpriteHolder;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI itemStatText;
    public Image flashingBackground;
    public InventoryItem empty;
    private string itemDetailsText;
    private Sprite itemSprite;

    private float pulseTime;
    public float pulseWidth = .5f;

    public void Update()
    {
        pulseTime += Time.deltaTime;
        flashingBackground.color = new Color(flashingBackground.color.r, flashingBackground.color.g, flashingBackground.color.b, Mathf.Sin(pulseTime/pulseWidth) * .2f + .65f);
    }

    public void SetItem(InventoryItem itemToSet) {
        if (null==itemToSet)
        {
            itemToSet = empty;
        }
        itemStored = itemToSet;
        itemDetailsText = itemToSet.equipmentDescription;
        itemSpriteHolder.GetComponent<Image>().sprite = itemToSet.itemIcon;
        itemSprite= itemToSet.itemIcon;
        itemText.text = itemToSet.name;
        if (itemToSet.Weapon) {
            Weapon tempwpn = (Weapon)itemToSet;
            itemStatText.text = "+" + tempwpn.addAttack + " ATK";
        }
        else if (itemToSet.Armor) {
            Armor temparm = (Armor)itemToSet;
            itemStatText.text = "+" + temparm.addDefense + " DEF";
        }
        else if (itemToSet.Accessory) {
            itemStatText.text = "Misc Item";
        }
        else
        {
            itemStatText.text = "";
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
