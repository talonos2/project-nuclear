using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : InventoryItem
{
    public int addAttack = 0;

    public static bool operator > (Weapon w1, Weapon w2)
    {
        return w1.addAttack > w2.addAttack;
    }
    public static bool operator <(Weapon w1, Weapon w2)
    {
        return w1.addAttack < w2.addAttack;
    }
    public static bool operator >=(Weapon w1, Weapon w2)
    {
        return w1.addAttack >= w2.addAttack;
    }
    public static bool operator <=(Weapon w1, Weapon w2)
    {
        return w1.addAttack <= w2.addAttack;
    }
    public static bool operator ==(Weapon w1, Weapon w2)
    {
        return w1.addAttack == w2.addAttack;
    }
    public static bool operator !=(Weapon w1, Weapon w2)
    {
        return w1.addAttack != w2.addAttack;
    }
}
