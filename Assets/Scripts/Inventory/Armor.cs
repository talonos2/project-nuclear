using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armor : InventoryItem{
    public int addDefense = 0;

    public static bool operator >(Armor w1, Armor w2)
    {
        return w1.addDefense > w2.addDefense;
    }
    public static bool operator <(Armor w1, Armor w2)
    {
        return w1.addDefense < w2.addDefense;
    }
    public static bool operator >=(Armor w1, Armor w2)
    {
        return w1.addDefense >= w2.addDefense;
    }
    public static bool operator <=(Armor w1, Armor w2)
    {
        return w1.addDefense <= w2.addDefense;
    }
    public static bool operator ==(Armor w1, Armor w2)
    {
        return w1.addDefense == w2.addDefense;
    }
    public static bool operator !=(Armor w1, Armor w2)
    {
        return w1.addDefense != w2.addDefense;
    }
}
