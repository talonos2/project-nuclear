using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData : MonoBehaviour
{

    public bool isMainCharacter = false;    
    public bool isAMonster = false;
    public bool isInteractableObject = false;
    public bool isItem = false;
    public bool isSwitch = false;
    public bool isNPC = false;
    public bool isSpike = false;
    public bool isPassable = false;

    public virtual void ProcessClick(CharacterStats stats)
    { }


}
