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

    public static bool operator >(Accessory w1, Accessory w2)
    {
        return CompareItem(w1,w2)>0;
    }
    public static bool operator <(Accessory w1, Accessory w2)
    {
        return CompareItem(w1, w2)<0;
    }
    public static bool operator >=(Accessory w1, Accessory w2)
    {
        return CompareItem(w1, w2) >= 0;
    }
    public static bool operator <=(Accessory w1, Accessory w2)
    {
        return CompareItem(w1, w2) <= 0;
    }
    public static bool operator ==(Accessory w1, Accessory w2)
    {
        return CompareItem(w1, w2) == 0;
    }
    public static bool operator !=(Accessory w1, Accessory w2)
    {
        return CompareItem(w1, w2) != 0;
    }

    public int getLowestFloor()
    {
        string[] Floors = floorFoundOn.Split(' ');
        if (Floors[0] == null) return -1;
        return    System.Convert.ToInt32(Floors[0]);

    }
    public static int CompareItem(Accessory x, Accessory y)
    {

        int xFloor = 0;
        int yFloor = 0;

        string[] Floors = x.floorFoundOn.Split(' ');

        foreach (string floor in Floors)
        {
            xFloor = System.Convert.ToInt32(floor);
        }

        Floors = y.floorFoundOn.Split(' ');

        foreach (string floor in Floors)
        {
            yFloor = System.Convert.ToInt32(floor);
        }

        if (xFloor > yFloor) { return -1; }
        else if (xFloor < yFloor) { return 1; }
        else { return 0; }
    }
}
