using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : InventoryItem, IComparable
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

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        Weapon other = obj as Weapon;
        if (other != null)
            return this.addAttack.CompareTo(other.addAttack);
        else
            throw new ArgumentException("Object is not a Temperature");
    }

    public override bool Equals(object obj)
    {
        var weapon = obj as Weapon;
        return weapon != null &&
               base.Equals(obj) &&
               addAttack == weapon.addAttack;
    }

    public override int GetHashCode()
    {
        var hashCode = 967190326;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + addAttack.GetHashCode();
        return hashCode;
    }
}
