using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Start is called before the first frame update

    

    public int HealthCrystalsGained=0;
    private int HealthCrystalBuff=0;
    public int ManaCrystalsGained=0;
    private int ManaCrystalBuff=0;
    public int AttackCrystalsGained=0;
    private int AttackCrystalBuff=0;
    public int defenseCrystalsGained=0;
    private int defenseCrystalBuff=0;

    private int HealthPerLevel;
    private int ManaPerLevel;
    private float AttackPerLevel;
    private float defensePerLevel;

    public int Level = 1;
    public int MaxHP;
    public int HP;
    public int mana;
    public float attack;
    public float defense;
    public float Experiance=0;
    public int ExpToLevel = 90;

    public bool MageClass;
    public bool FighterClass;
    public bool SurvivorClass;
    public bool ScoutClass;

    //private int 
    void Start()
    {
        SetupDungionRun();
    }

    //Each dungion run this section sets the stats. Main stats to set are the CrystalBuffs and Item buffs. 
    //Also sets class information
    private void SetupDungionRun()
    {

        //HealthCrystalBuff = GetHealthCrystalBuff();
        //ManaCrystalBuff= GetManaCrystalBuff()
        //AttackCrystalBuff = GetAttackCrystalBuff()
        //defenseCrystalBuff = GetdefenseCrystalBuff()
        Level = 1;
        SetExpToNextLevel();
        
        if (MageClass) {
            HealthPerLevel = 10;
            ManaPerLevel = 11;
            AttackPerLevel = 2f;
            defensePerLevel = 1;

            MaxHP = 140+ HealthPerLevel*Level+ HealthCrystalBuff;
            mana=95+ ManaPerLevel*Level;
            attack=10+(int)AttackPerLevel*Level;
            defense=(int)defensePerLevel*Level;

}
        if (FighterClass) {
            HealthPerLevel = 10;
            ManaPerLevel = 10;
            AttackPerLevel = 2.2f;
            defensePerLevel = 1;

            MaxHP = 140 + HealthPerLevel * Level;
            mana = 90 + ManaPerLevel * Level;
            attack = 11 + (int)AttackPerLevel * Level;
            defense = (int)defensePerLevel * Level;


        }
        if (SurvivorClass)
        {
            HealthPerLevel = 10;
            ManaPerLevel = 10;
            AttackPerLevel = 2;
            defensePerLevel = 1.1f;

            MaxHP = 140 + HealthPerLevel * Level;
            mana = 90 + ManaPerLevel * Level;
            attack = 10 + (int)AttackPerLevel * Level;
            defense = 1+(int)defensePerLevel * Level;

        }
        if (ScoutClass) {
            HealthPerLevel = 11;
            ManaPerLevel = 10;
            AttackPerLevel = 2;
            defensePerLevel = 1;

            MaxHP = 154 + HealthPerLevel * Level;
            mana = 90 + ManaPerLevel * Level;
            attack = 10 + (int)AttackPerLevel * Level;
            defense = (int)defensePerLevel * Level;

        }
        HP = MaxHP;
        HP = HP + HealthCrystalBuff;
        mana = mana + ManaCrystalBuff;
        attack = attack + AttackCrystalBuff;
        defense = defense + defenseCrystalBuff; 

    }

    internal void AddExp(int expGiven)
    {
        Experiance += expGiven;
        while (Experiance >= ExpToLevel) {
            Experiance -= ExpToLevel;
            Level += 1;
            LevelUp();
            SetExpToNextLevel();
        }
    }

    private void LevelUp()
    {
        MaxHP += HealthPerLevel;
        HP += HealthPerLevel;
        mana += ManaPerLevel;
        attack += AttackPerLevel;
        defense += defensePerLevel;
    }

    private void SetExpToNextLevel()
    {
        ExpToLevel = Level * 45 + Level * Level * 45;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
