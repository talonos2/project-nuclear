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
                return new Color(1, .4f, .4f);
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

    //<sprite index =[index] >

    public static string TempEleIconString(this ElementalPower ele)
    {
        switch (ele)
        {
            case ElementalPower.NULL:
                return "<sprite index=3>";
            case ElementalPower.ICE:
                return "<sprite index=1>";
            case ElementalPower.EARTH:
                return "<sprite index=4>";
            case ElementalPower.FIRE:
                return "<sprite index=0>";
            case ElementalPower.AIR:
                return "<sprite index=2>";
        }
        return "Bad Type!";
    }
}