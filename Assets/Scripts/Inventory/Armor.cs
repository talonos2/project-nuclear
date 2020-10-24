using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armor : InventoryItem, IComparable
{
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

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        Armor other = obj as Armor;
        if (other != null)
            return this.addDefense.CompareTo(other.addDefense);
        else
            throw new ArgumentException("Object is not a Temperature");
    }

    public override bool Equals(object obj)
    {
        var armor = obj as Armor;
        return armor != null &&
               base.Equals(obj) &&
               addDefense == armor.addDefense;
    }

    public override int GetHashCode()
    {
        var hashCode = -1496245746;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + addDefense.GetHashCode();
        return hashCode;
    }
}
