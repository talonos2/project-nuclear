using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Stats
{
    public int HealthCrystalsGained = 0;
    private int HealthCrystalBuff = 0;
    public int ManaCrystalsGained = 0;
    private int ManaCrystalBuff = 0;
    public int AttackCrystalsGained = 0;
    private int AttackCrystalBuff = 0;
    public int defenseCrystalsGained = 0;
    private int defenseCrystalBuff = 0;

    private int HealthPerLevel;
    private int ManaPerLevel;
    private float AttackPerLevel;
    private float DefensePerLevel;

    public int Level = 1;
    public int MaxMana;
    public int mana;

    public int experience=0;
    public int expToLevel = 90;

    public Sprite[] combatSprites;

    public bool MageClass;
    public bool FighterClass;
    public bool SurvivorClass;
    public bool ScoutClass;

    public GameObject weapon;
    public GameObject armor;
    public GameObject accessory;

    public int armorBonusDefense;
    public int weaponBonusAttack;
    public int accessoryAttack;
    public int accessoryDefense;
    public int accessoryHealth;
    public int accessoryMana;
    public int accessoryCritChance;
    public int accessoryExpBonus;
    public int accessoryIceBonus;
    public int accessoryEarthBonus;
    public int accessoryFireBonus;
    public int accessoryAirBonus;
    public int accessoryHPVamp;
    public int accessoryMPVamp;
    public int accessoryDodgeBonus;
    public int accessoryAttackPercent;

    public int powersGained = 0;
    public int currentPower = 0;

    private GameObject gameStateData;
    private GameData gameData;
    private CharacterStats SavedStats;
    private GameObject equipmentData;
    private EquipmentData itemData;
    private float regenerationCounter;

    void Start()
    {

        
        gameStateData = GameObject.Find("GameStateData");
        equipmentData = GameObject.Find("EquipmentData");
        gameData = gameStateData.GetComponent <GameData> ();
        itemData = equipmentData.GetComponent<EquipmentData>();
        SavedStats = gameStateData.GetComponent<CharacterStats>();
        if (gameData.FloorNumber==1)
        {            
            SetupBaseStats();
            PushCharacterData();
        }
        else {
            SetupBaseStats();            
            PullCharacterData(); }              
    }

    private void PullCharacterData()
    {
        this.Level = SavedStats.Level;
        this.MaxHP=SavedStats.MaxHP;
        this.HP = SavedStats.HP;
        this.MaxMana = SavedStats.MaxMana;
        this.mana = SavedStats.mana;
        this.attack = SavedStats.attack;
        this.defense = SavedStats.defense;
        this.experience = SavedStats.experience;
        this.expToLevel = SavedStats.expToLevel;
        this.powersGained = gameData.PowersGained;
        this.currentPower = SavedStats.currentPower;

        this.HealthCrystalsGained = SavedStats.HealthCrystalsGained;
        this.ManaCrystalsGained = SavedStats.ManaCrystalsGained;
        this.AttackCrystalsGained = SavedStats.AttackCrystalsGained;
        this.defenseCrystalsGained = SavedStats.defenseCrystalsGained;

        this.weapon = SavedStats.weapon;
        this.armor = SavedStats.armor;
        this.accessory = SavedStats.accessory;
        setWeaponStats(weapon);
        setArmorStats(armor);
        setAccessoryStats(accessory);

}

    //Called at the end of every combat and every time an item is gained. 
    public void PushCharacterData() {
        SavedStats.Level=this.Level;
        SavedStats.MaxHP=this.MaxHP;
        SavedStats.HP=this.HP;
        SavedStats.MaxMana = this.MaxMana;
        SavedStats.mana=this.mana;
        SavedStats.attack=this.attack;
        SavedStats.defense= this.defense;
        SavedStats.experience = this.experience;
        SavedStats.expToLevel = this.expToLevel;
        SavedStats.powersGained = this.powersGained;
        SavedStats.currentPower = this.currentPower;
        gameData.PowersGained = this.powersGained;

        SavedStats.HealthCrystalsGained=this.HealthCrystalsGained;
        SavedStats.ManaCrystalsGained=this.ManaCrystalsGained;
        SavedStats.AttackCrystalsGained= this.AttackCrystalsGained;
        SavedStats.defenseCrystalsGained= this.defenseCrystalsGained;

        SavedStats.weapon = this.weapon;
        SavedStats.armor = this.armor;
        SavedStats.accessory = this.accessory;
    }

    //Each dungion run this section sets the stats. Main stats to set are the CrystalBuffs and Item buffs. 
    //Also sets class information
    private void SetupBaseStats()
    {

        HealthCrystalBuff = GameData.Instance.HealhCrystalBonus;
        ManaCrystalBuff = GameData.Instance.ManaCrystalBonus;
        AttackCrystalBuff = GameData.Instance.AttackCrystalBonus;
        defenseCrystalBuff = GameData.Instance.DefenseCrystalBonus;
        Level = 1;
        SetExpToNextLevel();
        
        if (MageClass) {
            HealthPerLevel = 10;
            ManaPerLevel = 11;
            AttackPerLevel = 2f;
            DefensePerLevel = 1;

            MaxHP = 140+ HealthPerLevel*Level;
            MaxMana=95+ ManaPerLevel*Level;

            attack=10+(int)AttackPerLevel*Level;
            defense=(int)DefensePerLevel*Level;

}
        if (FighterClass) {
            HealthPerLevel = 10;
            ManaPerLevel = 10;
            AttackPerLevel = 2.2f;
            DefensePerLevel = 1;

            MaxHP = 140 + HealthPerLevel * Level;
            MaxMana = 90 + ManaPerLevel * Level;
            attack = 11 + (int)AttackPerLevel * Level;
            defense = (int)DefensePerLevel * Level;


        }
        if (SurvivorClass)
        {
            HealthPerLevel = 10;
            ManaPerLevel = 10;
            AttackPerLevel = 2;
            DefensePerLevel = 1.1f;

            MaxHP = 140 + HealthPerLevel * Level;
            MaxMana = 90 + ManaPerLevel * Level;
            attack = 10 + (int)AttackPerLevel * Level;
            defense = 1+(int)DefensePerLevel * Level;

        }
        if (ScoutClass) {
            HealthPerLevel = 11;
            ManaPerLevel = 10;
            AttackPerLevel = 2;
            DefensePerLevel = 1;

            MaxHP = 154 + HealthPerLevel * Level;
            MaxMana = 90 + ManaPerLevel * Level;
            attack = 10 + (int)AttackPerLevel * Level;
            defense = (int)DefensePerLevel * Level;

        }

        MaxMana += ManaCrystalBuff;
        MaxHP += HealthCrystalBuff;
        HP = MaxHP;
        mana = MaxMana;
        attack = attack + AttackCrystalBuff;
        defense = defense + defenseCrystalBuff;

        if (gameData.RunNumber == 1) {
            weapon = itemData.getRunOneWeapon();
           // attack += weapon.GetComponent<Weapon>().addAttack;
        }
        else {
            weapon = itemData.getRunTwoWeapon();
        }
        armor = itemData.getDefaultArmor();
        accessory = itemData.getEmptyAccessory();
        setWeaponStats(weapon);
        setArmorStats(armor);
        setAccessoryStats(accessory);
    }

    internal void AddExp(int expGiven)
    {
        experience += expGiven;
        while (experience >= expToLevel) {
            experience -= expToLevel;
            Level += 1;
            LevelUp();
            SetExpToNextLevel();
        }
    }

    public void setWeaponStats(GameObject weaponChanged) {
        weaponBonusAttack = weaponChanged.GetComponent<Weapon>().addAttack;
    }

    public void setArmorStats(GameObject armorChanged) {
        armorBonusDefense = armorChanged.GetComponent<Armor>().addDefense;
    }

    public void setAccessoryStats(GameObject accessoryChanged) {
        Debug.Log("ToBeImplemented setting accessorys in CharacterStats.cs");
        accessoryAttack=0;
        accessoryDefense = 0;
        accessoryHealth = 0;
        accessoryMana = 0;
        accessoryCritChance = 0;
        accessoryExpBonus = 0;
        accessoryIceBonus = 0;
        accessoryEarthBonus = 0;
        accessoryFireBonus = 0;
        accessoryAirBonus = 0;
        accessoryHPVamp = 0;
        accessoryMPVamp = 0;
        accessoryDodgeBonus = 0;
        accessoryAttackPercent = 0;

}

    private void LevelUp()
    {
        MaxHP += HealthPerLevel;
        HP += HealthPerLevel;
        mana += ManaPerLevel;
        MaxMana += ManaPerLevel;
        attack += AttackPerLevel;
        defense += DefensePerLevel;
    }

    private void SetExpToNextLevel()
    {
        expToLevel = (Level * 45 + Level * Level * 45)- ((Level-1) * 45 + (Level-1) * (Level-1) * 45);
    }

    // Update is called once per frame
    void Update()
    {
        regenerationCounter += Time.deltaTime;
        if (regenerationCounter >= 3) {
            HP += (int)(MaxHP * .01f);
            mana += (int)(MaxMana * .01f);
            if (HP > MaxHP)
                HP = MaxHP;
            if (mana > MaxMana)
                mana = MaxMana;
            PushCharacterData();

            regenerationCounter = 0;
        }
    }
}
