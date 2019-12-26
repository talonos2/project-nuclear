using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBuffManager : MonoBehaviour
{
    
    //Setup Crystal Buffs
    private static int[] crystalTiers = { 50, 150, 350, 650, 1075, 1625, 2300, 3100, 4025, 5075, 6250, 7550, 8975, 10525, 12200, 14000, 15925, 17975, 20150, 22450, 31451 };
    private static int[] crystalUpgrades = {50, 100, 200, 300, 425, 550, 675, 800, 925, 1050, 1175, 1300, 1425, 1550, 1675, 1800, 1925, 2050, 2175, 2300, 9001};



    public static void SetCrystalBuffs()
    {
        SetHealthBuff();
        SetManaBuff();
        SetAttackBuff();
        SetDefenseBuff();
    }



    private static void SetDefenseBuff()
    {
       
        int i = 0;
        while (i < crystalTiers.Length) {
            if (GameData.Instance.DefenseCrystalTotal< crystalTiers[i])
            {
                break;
            }
            i++;
        }
        GameData.Instance.DefenseCrystalBonus = 2 * i;
    }

    private static void SetAttackBuff()
    {
        int i = 0;
        while (i < crystalTiers.Length)
        {
            if (GameData.Instance.AttackCrystalTotal < crystalTiers[i])
            {
                break;
            }
            i++;
        }
        GameData.Instance.AttackCrystalBonus = 4 * i;

    }

    private static void SetManaBuff()
    {
        int i = 0;
        while (i < crystalTiers.Length)
        {
            if (GameData.Instance.ManaCrystalTotal < crystalTiers[i])
            {
                break;
            }
            i++;
        }
        GameData.Instance.ManaCrystalBonus = 20 * i;
    }

    private static void SetHealthBuff()
    {
        int i = 0;
        while (i < crystalTiers.Length)
        {
            if (GameData.Instance.HealhCrystalTotal < crystalTiers[i])
            {
                break;
            }
            i++;
        }
        GameData.Instance.HealhCrystalBonus = 20 * i;
    }
}
