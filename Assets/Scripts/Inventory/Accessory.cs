using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Accessory : InventoryItem
{
    public string equipmentStatDescription = "StatsPlease";
    public EffectType effectType;
    public EffectType effectType1;
    public EffectType effectType2;
    public EffectType effectType3;
    public EffectType effectType4;
    public float effectStrength;
    public float effectStrength1;
    public float effectStrength2;
    public float effectStrength3;
    public float effectStrength4;
    public enum EffectType
    {
        None, Health,Mana,Defense,Attack,Ice,Earth,Fire,Air,HPVamp,MPVamp,Crit,XP,Dodge, AttackPercent
    }
}
