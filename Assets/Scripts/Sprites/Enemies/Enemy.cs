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

    

    public Sprite[] combatSprites;
    public AttackAnimation attackAnimation = AttackAnimation.HOP;

    public AudioClip goodHit;
    public AudioClip badHit;
    public AudioClip goodBlock;
    public AudioClip badBlock;





    public bool iceBoss;
    public bool earthBoss;
    public bool fireBoss;
    public bool airBoss;
    public bool deathBoss;

    public void Start()
    {
        if (iceBoss && GameData.Instance.iceBoss1) { Destroy(this.gameObject); }
        if (earthBoss && GameData.Instance.earthBoss1) { Destroy(this.gameObject); }
        if (fireBoss && GameData.Instance.fireBoss1) { Destroy(this.gameObject); }
        if (airBoss && GameData.Instance.airBoss1) { Destroy(this.gameObject); }
        if (deathBoss && GameData.Instance.deathBoss) { Destroy(this.gameObject); }
    }


}
