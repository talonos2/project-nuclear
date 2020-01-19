using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Stats
{
    public string charName = "FIX ME - I HAVE NO NAME";

    public int HealthCrystalsGained = 0;
    internal int HealthCrystalBuff = 0;
    public int ManaCrystalsGained = 0;
    internal int ManaCrystalBuff = 0;
    public int AttackCrystalsGained = 0;
    internal int AttackCrystalBuff = 0;
    public int defenseCrystalsGained = 0;
    internal int defenseCrystalBuff = 0;

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
    public Sprite bustSprite;

    public bool MageClass;
    public bool FighterClass;
    public bool SurvivorClass;
    public bool ScoutClass;

    public GameObject weapon;
    public GameObject armor;
    public GameObject accessory;

    public float armorBonusDefense;
    public float weaponBonusAttack;
    public float accessoryAttack;
    public float accessoryDefense;
    public float accessoryHealth;
    public float accessoryMana;
    public float accessoryCritChance;
    public float accessoryExpBonus;
    public float accessoryIceBonus;
    public float accessoryEarthBonus;
    public float accessoryFireBonus;
    public float accessoryAirBonus;
    public float accessoryHPVamp;
    public float accessoryMPVamp;
    public float accessoryDodgeBonus;
    public float accessoryAttackPercent;

    private int baseMaxHealth;
    private int baseMaxMana;
    private float baseAttack;
    private float baseDefense;

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
        if (gameData.FloorNumber==0)
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

            baseMaxHealth = 140+ HealthPerLevel*Level;
            baseMaxMana = 95+ ManaPerLevel*Level;

            baseAttack = 10+(int)AttackPerLevel*Level;
            baseDefense = (int)DefensePerLevel*Level;

}
        if (FighterClass) {
            HealthPerLevel = 10;
            ManaPerLevel = 10;
            AttackPerLevel = 2.2f;
            DefensePerLevel = 1;

            baseMaxHealth = 140 + HealthPerLevel * Level;
            baseMaxMana = 90 + ManaPerLevel * Level;
            baseAttack = 11 + (int)AttackPerLevel * Level;
            baseDefense = (int)DefensePerLevel * Level;


        }
        if (SurvivorClass)
        {
            HealthPerLevel = 10;
            ManaPerLevel = 10;
            AttackPerLevel = 2;
            DefensePerLevel = 1.1f;

            baseMaxHealth = 140 + HealthPerLevel * Level;
            baseMaxMana = 90 + ManaPerLevel * Level;
            baseAttack = 10 + (int)AttackPerLevel * Level;
            baseDefense = 1+(int)DefensePerLevel * Level;

        }
        if (ScoutClass) {
            HealthPerLevel = 11;
            ManaPerLevel = 10;
            AttackPerLevel = 2;
            DefensePerLevel = 1;

            baseMaxHealth = 154 + HealthPerLevel * Level;
            baseMaxMana = 90 + ManaPerLevel * Level;
            baseAttack = 10 + (int)AttackPerLevel * Level;
            baseDefense = (int)DefensePerLevel * Level;

        }

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
        setMaxStats();
        setFullHPMP();
        
    }

    internal void setFullHPMP()
    {
        HP = MaxHP;
        mana = MaxMana;
    }

    private void setMaxStats()
    {
        MaxMana = baseMaxMana+ManaCrystalBuff+(int)accessoryMana;
        MaxHP = baseMaxHealth + HealthCrystalBuff+(int)accessoryHealth;
        if (HP > MaxHP)
            HP = MaxHP;
        if (mana > MaxMana)
            mana = MaxMana;
        attack = baseAttack + AttackCrystalBuff+weaponBonusAttack+accessoryAttack;
        defense = baseDefense + defenseCrystalBuff+armorBonusDefense+accessoryDefense;
    }

    internal void AddExp(int expGiven)
    {
        expGiven = (int)((float)expGiven * (float)(1+accessoryExpBonus/100));
        experience += expGiven;
        while (experience >= expToLevel) {
            experience -= expToLevel;
            Level += 1;
            LevelUp();
            SetExpToNextLevel();
        }
    }

    private void setWeaponStats(GameObject weaponChanged) {
        weaponBonusAttack = weaponChanged.GetComponent<Weapon>().addAttack;
        setMaxStats();
    }

    private void setArmorStats(GameObject armorChanged) {
        armorBonusDefense = armorChanged.GetComponent<Armor>().addDefense;
        setMaxStats();
    }

    private void setAccessoryStats(GameObject accessoryChanged) {
        float oldaccHealth=accessoryHealth;
        float oldaccMana= accessoryMana;

        accessoryAttack =0;
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

        Accessory accItem = accessoryChanged.GetComponent<Accessory>();

        setEffectStats(accItem.effectType, accItem.effectStrength);
        setEffectStats(accItem.effectType1, accItem.effectStrength1);
        setEffectStats(accItem.effectType2, accItem.effectStrength2);
        setEffectStats(accItem.effectType3, accItem.effectStrength3);
        setEffectStats(accItem.effectType4, accItem.effectStrength4);

        float HPpercent = (float)(HP) / MaxHP;
        HP -= (int)(HPpercent * oldaccHealth);
        HP += (int)(HPpercent * accessoryHealth);

        float manaPercent = (float)(mana) / MaxMana;
        HP -= (int)(manaPercent * oldaccMana);
        HP += (int)(manaPercent * accessoryMana);

        setMaxStats();



    }

    private void setEffectStats(Accessory.EffectType effectType, float effectStrength)
    {
        switch (effectType) {
            case Accessory.EffectType.None:
                break;
            case Accessory.EffectType.Health:
                accessoryHealth = effectStrength;
                break;
            case Accessory.EffectType.Mana:
                accessoryMana = effectStrength;
                break;
            case Accessory.EffectType.Defense:
                accessoryDefense = effectStrength;
                break;
            case Accessory.EffectType.Attack:
                accessoryAttack = effectStrength;
                break;
            case Accessory.EffectType.Ice:
                accessoryIceBonus = effectStrength;
                break;
            case Accessory.EffectType.Earth:
                accessoryEarthBonus = effectStrength;
                break;
            case Accessory.EffectType.Fire:
                accessoryFireBonus = effectStrength;
                break;
            case Accessory.EffectType.Air:
                accessoryAirBonus = effectStrength;
                break;
            case Accessory.EffectType.HPVamp:
                accessoryHPVamp = effectStrength;
                break;
            case Accessory.EffectType.MPVamp:
                accessoryMPVamp = effectStrength;

                break;
            case Accessory.EffectType.Crit:
                accessoryCritChance = effectStrength;

                break;
            case Accessory.EffectType.XP:
                accessoryExpBonus = effectStrength;

                break;
            case Accessory.EffectType.Dodge:
                accessoryDodgeBonus = effectStrength;

                break;
            case Accessory.EffectType.AttackPercent:
                accessoryAttackPercent = effectStrength;

                break;


        }
    }

    private void LevelUp()
    {
        baseMaxHealth += HealthPerLevel;
        HP += HealthPerLevel;
        mana += ManaPerLevel;
        baseMaxMana += ManaPerLevel;
        baseAttack += AttackPerLevel;
        baseDefense += DefensePerLevel;
        setMaxStats();
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

    internal void setAccessory(GameObject itemToSwap)
    {
        this.accessory = itemToSwap;
        setAccessoryStats(itemToSwap);
    }

    internal void setWeapon(GameObject itemToSwap)
    {
        this.weapon = itemToSwap;
        setWeaponStats(itemToSwap);
    }

    internal void setArmor(GameObject itemToSwap)
    {
        this.armor = itemToSwap;
        setArmorStats(itemToSwap);
    }
}
