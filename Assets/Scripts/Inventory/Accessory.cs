using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Accessory : InventoryItem
{
    public EffectType effectType;
    public float effectStrength;

    public enum EffectType
    {
        Health,Mana,defense,Attack,Ice,Earth,Fire,Air,HPVamp,MPVamp,Crit,XP,Dodge
    }
}
