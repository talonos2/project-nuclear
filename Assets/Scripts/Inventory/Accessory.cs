using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Accessory : MonoBehaviour
{
    public EffectType effectType;
    public float effectStrength;

    public enum EffectType
    {
        SAMPLE,EFFECT,TYPES,TO,BE,REPLACED,LATER
    }
}
