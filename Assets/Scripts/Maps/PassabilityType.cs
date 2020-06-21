using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassabilityType
{ NORMAL, MONSTER, AIR, WALL, ERROR }
public enum GroundMaterialType
{ SNOW, GRASS, DIRT, STONE, METAL, WALL, ERROR }
public enum AmbientSoundType
{ NONE, WATER, LAVA, ERROR }

public static class GridTypeExtensions
{
    public static string GetName(this GroundMaterialType inMat)
    {
        switch (inMat)
        {
            case GroundMaterialType.SNOW:
                return "Snow";
            case GroundMaterialType.GRASS:
                return "Grass";
            case GroundMaterialType.DIRT:
                return "Dirt";
            case GroundMaterialType.STONE:
                return "Stone";
            case GroundMaterialType.METAL:
                return "Metal";
            default:
                return "ERROR";
        }
    }
}