using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Stats
{

    public string enemyName = "Name me please";
    public int ExpGiven;
    public ElementalPower weakness = ElementalPower.NULL;
    public CrystalType crystalType = CrystalType.NULL;
    public int crystalDropAmount;

    public int powerGiven;

    public Sprite[] combatSprites;
    public AttackAnimation attackAnimation = AttackAnimation.HOP;

 
}
