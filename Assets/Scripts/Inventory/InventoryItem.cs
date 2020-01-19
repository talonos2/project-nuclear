using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public string equipmentDescription = "Describe me please";
    public string floorFoundOn;
    public bool Weapon;
    public bool Armor;
    public bool Accessory;
    public bool Rare;
    public Sprite itemIcon;

    public void OnAfterDeserialize()
    {
        throw new NotImplementedException();
    }

    public void OnBeforeSerialize()
    {
        throw new NotImplementedException();
    }
}
