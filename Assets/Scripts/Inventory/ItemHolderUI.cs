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
        itemStored = itemToSet;
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
