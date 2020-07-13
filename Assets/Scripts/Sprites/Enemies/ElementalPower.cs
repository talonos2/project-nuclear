using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  ElementalPower
{ NULL, ICE, EARTH, FIRE, AIR }

public static class EleExtensions
{
    public static Color EleColor(this ElementalPower ele)
    {
        switch (ele)
        {
            case ElementalPower.NULL:
                return Color.gray;
            case ElementalPower.ICE:
                return Color.cyan;
            case ElementalPower.EARTH:
                return Color.green;
            case ElementalPower.FIRE:
                return Color.red;
            case ElementalPower.AIR:
                return Color.yellow;
        }
        return Color.black;
    }

    public static string EleName(this ElementalPower ele)
    {
        switch (ele)
        {
            case ElementalPower.NULL:
                return "None";
            case ElementalPower.ICE:
                return "ICE";
            case ElementalPower.EARTH:
                return "EARTH";
            case ElementalPower.FIRE:
                return "FIRE";
            case ElementalPower.AIR:
                return "AIR";
        }
        return "Bad Type!";
    }
}