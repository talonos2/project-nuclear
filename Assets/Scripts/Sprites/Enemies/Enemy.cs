﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Stats
{

    public string enemyName = "Name me please";
    public int ExpGiven;
    public ElementalPower weakness = ElementalPower.NULL;
    public CrystalType crystalType = CrystalType.NULL;
    public int crystalDropAmount;
    public Vector2 hitsplatOffset = new Vector2(0, 0);
    public Vector2 combatOffset;



    public Sprite[] combatSprites;
    public AttackAnimation attackAnimation = AttackAnimation.HOP;

    public AudioClip goodHit;
    public AudioClip badHit;
    public AudioClip goodBlock;
    public AudioClip badBlock;
    public PowerupEffect powerupEffect;





    public bool iceBoss;
    public bool earthBoss;
    public bool earthBoss2;
    public bool fireBoss;
    public bool fireBoss2;
    public bool airBoss;
    public bool airBoss2;
    public bool deathBoss;
    public bool deathBoss2;
    public bool finalBoss;
    public bool finalBossForm1;

    public bool testMonster;
    public bool hideCrystalSounds;

    public void Start()
    {
        if (iceBoss && GameData.Instance.iceBoss1) { Destroy(this.gameObject); }
        if (earthBoss && GameData.Instance.earthBoss1) { Destroy(this.gameObject); }
        if (fireBoss && GameData.Instance.fireBoss1) { Destroy(this.gameObject); }
        if (airBoss && GameData.Instance.airBoss1) { Destroy(this.gameObject); }
        if (deathBoss && GameData.Instance.deathBoss1) { Destroy(this.gameObject); }
        if (earthBoss2 && !GameData.Instance.earthBoss1) { Destroy(this.gameObject); }
        if (fireBoss2 && GameData.Instance.fireBoss2 || fireBoss2 && !GameData.Instance.fireBoss1) { Destroy(this.gameObject); }
        if (airBoss2 && GameData.Instance.airBoss2 || airBoss2 && !GameData.Instance.airBoss1) { Destroy(this.gameObject); }
        if (deathBoss2 && GameData.Instance.deathBoss2 || deathBoss2 && !GameData.Instance.deathBoss1) { Destroy(this.gameObject); }
    }

    public virtual void doUponDeath()
    {
        GameData.Instance.monstersKilledInThisGame += 1;
        FinalWinterAchievementManager.Instance.SetStatAndGiveAchievement(FWStatAchievement.KILL_LOTS_OF_MONSTERS, GameData.Instance.monstersKilledInThisGame);

        GameData.Instance.monstersKilledInThisRun += 1;
    }

}
