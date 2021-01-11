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
    private CharacterMovement savedCharacterMovement;

    internal void setCharacterMoveScript(CharacterMovement characterMovement)
    {
        savedCharacterMovement = characterMovement;
    }

    internal void deactivatePowers() {
        savedCharacterMovement.TurnHasteOff();
        if (GameData.Instance.stealthed)
            savedCharacterMovement.ActivateInvisibility();
    }

    public bool MageClass;
    public bool FighterClass;
    public bool SurvivorClass;
    public bool ScoutClass;

    public Weapon weapon;
    public Armor armor;
    public Accessory accessory;

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
    private CharacterStats SavedStats;
    private GameObject equipmentData;
    private EquipmentData itemData;
    private float regenerationCounter;
    private LevelUpper levelUpper;
    private float remainingHP = 0;
    private float remainingMP = 0;

    void Start()
    {        
        gameStateData = GameObject.Find("GameStateData");
        equipmentData = GameObject.Find("EquipmentData");
        //gameData = gameStateData.GetComponent <GameData> ();
        itemData = equipmentData.GetComponent<EquipmentData>();
        SavedStats = gameStateData.GetComponent<CharacterStats>();

        /*if (gameData.FloorNumber == 0)
        {
            SetupBaseStats();
            PushCharacterData();
        }
        else
        {*/
        if (!GameData.Instance.statsSetup) {
            GameData.Instance.statsSetup = true;
            SetupBaseStats();
        }

        if (GameData.Instance.FloorNumber >= 1)
        {
            
            PullCharacterData();
        }

        
    }



    internal void setInitialStats()
    {
        SetupBaseStats();
        PushCharacterData();
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
        this.powersGained = GameData.Instance.PowersGained;
        this.currentPower = SavedStats.currentPower;

        this.HealthCrystalsGained = SavedStats.HealthCrystalsGained;
        this.ManaCrystalsGained = SavedStats.ManaCrystalsGained;
        this.AttackCrystalsGained = SavedStats.AttackCrystalsGained;
        this.defenseCrystalsGained = SavedStats.defenseCrystalsGained;

        this.HealthCrystalBuff = SavedStats.HealthCrystalBuff;
        this.ManaCrystalBuff = SavedStats.ManaCrystalBuff;
        this.defenseCrystalBuff = SavedStats.defenseCrystalBuff;
        this.AttackCrystalBuff = SavedStats.AttackCrystalBuff;

        this.weapon = SavedStats.weapon;
        this.armor = SavedStats.armor;
        this.accessory = SavedStats.accessory;

        this.baseAttack = SavedStats.baseAttack;
        this.baseDefense = SavedStats.baseDefense;
        this.baseMaxHealth = SavedStats.baseMaxHealth;
        this.baseMaxMana = SavedStats.baseMaxMana;

        this.HealthPerLevel = SavedStats.HealthPerLevel;
        this.ManaPerLevel = SavedStats.ManaPerLevel;
        this.AttackPerLevel = SavedStats.AttackPerLevel;
        this.DefensePerLevel = SavedStats.DefensePerLevel;

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
        GameData.Instance.PowersGained = this.powersGained;

        SavedStats.HealthCrystalsGained=this.HealthCrystalsGained;
        SavedStats.ManaCrystalsGained=this.ManaCrystalsGained;
        SavedStats.AttackCrystalsGained= this.AttackCrystalsGained;
        SavedStats.defenseCrystalsGained= this.defenseCrystalsGained;

        SavedStats.HealthCrystalBuff = this.HealthCrystalBuff;
        SavedStats.ManaCrystalBuff=this.ManaCrystalBuff;
        SavedStats.defenseCrystalBuff=this.defenseCrystalBuff;
        SavedStats.AttackCrystalBuff=this.AttackCrystalBuff;

        SavedStats.baseAttack = this.baseAttack;
        SavedStats.baseDefense = this.baseDefense;
        SavedStats.baseMaxHealth = this.baseMaxHealth;
        SavedStats.baseMaxMana = this.baseMaxMana;

        SavedStats.HealthPerLevel = this.HealthPerLevel;
        SavedStats.ManaPerLevel = this.ManaPerLevel;
        SavedStats.AttackPerLevel = this.AttackPerLevel;
        SavedStats.DefensePerLevel = this.DefensePerLevel;

        SavedStats.weapon = this.weapon;
        SavedStats.armor = this.armor;
        SavedStats.accessory = this.accessory;

        SavedStats.FighterClass = this.FighterClass;
        SavedStats.MageClass = this.MageClass;
        SavedStats.ScoutClass = this.ScoutClass;
        SavedStats.SurvivorClass = this.SurvivorClass;

        setWeaponStats(weapon);
        setArmorStats(armor);
        setAccessoryStats(accessory);
        SavedStats.weaponBonusAttack = this.weaponBonusAttack;
        SavedStats.armorBonusDefense = this.armorBonusDefense;

    }

    //Each dungion run this section sets the stats. Main stats to set are the CrystalBuffs and Item buffs. 
    //Also sets class information
    private void SetupBaseStats()
    {

        gameStateData = GameObject.Find("GameStateData");
        equipmentData = GameObject.Find("EquipmentData");
        //gameData = gameStateData.GetComponent <GameData> ();
        itemData = equipmentData.GetComponent<EquipmentData>();
        SavedStats = gameStateData.GetComponent<CharacterStats>();

        Level = 1;
        SetExpToNextLevel();

        HealthPerLevel = 10;
        ManaPerLevel = 10;
        AttackPerLevel = 2f;
        DefensePerLevel = 1f;

        baseMaxHealth = 150;
        baseMaxMana = 100;
        baseAttack = 10 ;
        baseDefense = 0;

        if (MageClass) {
            ManaPerLevel = 11;
        }
        if (FighterClass) {
            AttackPerLevel = AttackPerLevel*1.1f;
        }
        if (SurvivorClass)
        {
            DefensePerLevel = DefensePerLevel * 1.1f;
        }
        if (ScoutClass) {
            HealthPerLevel = 11;
        }

        baseMaxHealth += HealthPerLevel;
        baseMaxMana += ManaPerLevel;
        baseAttack += AttackPerLevel;
        baseDefense += DefensePerLevel;

        if (GameData.Instance.RunNumber == 1) {
            weapon = itemData.getRunOneWeapon();
           // attack += weapon.GetComponent<Weapon>().addAttack;
        }
        else {
            weapon = itemData.getRunTwoWeapon();
        }

        armor = itemData.getDefaultArmor();

        if (GameData.Instance.RunNumber == 12) {
            armor = itemData.getJezerineArmor();
        }
        accessory = itemData.getEmptyAccessory();
        setWeaponStats(weapon);
        setArmorStats(armor);
        setAccessoryStats(accessory);
        NewCrystalLevelController.SetCrystalBuffs();
        HealthCrystalBuff = GameData.Instance.HealhCrystalBonus;
        ManaCrystalBuff = GameData.Instance.ManaCrystalBonus;
        AttackCrystalBuff = GameData.Instance.AttackCrystalBonus;
        defenseCrystalBuff = GameData.Instance.DefenseCrystalBonus;


        if (GameData.Instance.iceBoss1) { powersGained = 1; }
        if (GameData.Instance.earthBoss1) { powersGained = 2; }
        if (GameData.Instance.fireBoss1) { powersGained = 3; }
        if (GameData.Instance.airBoss1) { powersGained = 4; }

        setMaxStats();
        setFullHPMP();

    }

    internal void setFullHPMP()
    {
        HP = MaxHP;
        mana = MaxMana;
        //PushCharacterData();
        
    }

    internal void setMaxStats()
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
        while (experience >= expToLevel)
        {
            experience -= expToLevel;
            Level += 1;
            if (!levelUpper)
            {
                levelUpper = GetComponentInChildren<LevelUpper>();
            }
            LevelUp();
            levelUpper.AddLevel();
            SetExpToNextLevel();
        }
    }

    private void setWeaponStats(Weapon weaponChanged) {
        if (!weaponChanged) return;
        weaponBonusAttack = weaponChanged.GetComponent<Weapon>().addAttack;
        setMaxStats();
    }

    internal void ShutUpLevelUpper()
    {
        levelUpper.ShutUp();
    }

    private void setArmorStats(Armor armorChanged) {
        if (!armorChanged) return;
        armorBonusDefense = armorChanged.GetComponent<Armor>().addDefense;
        setMaxStats();
    }

    private void setAccessoryStats(Accessory accessoryChanged) {
        if (!accessoryChanged) return;
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
        mana -= (int)(manaPercent * oldaccMana);
        mana += (int)(manaPercent * accessoryMana);

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
        if (GameState.fullPause || GameData.Instance.pauseTimer || GameData.Instance.isInDialogue) {
            return;
        }
        regenerationCounter += Time.deltaTime;
        if (regenerationCounter >= 3) {
            float tempHP = (MaxHP * .01f+remainingHP);
            float tempMana = (MaxMana * .01f+remainingMP);
            remainingHP = tempHP % 1;
            remainingMP = tempMana % 1;
            HP += (int)tempHP;
            mana += (int)tempMana;

            if (HP > MaxHP)
                HP = MaxHP;
            if (mana > MaxMana)
                mana = MaxMana;
            PushCharacterData();

            regenerationCounter = 0;
        }
    }

    internal void setAccessory(Accessory itemToSwap)
    {
        this.accessory = itemToSwap;
        setAccessoryStats(itemToSwap);
    }

    internal void setWeapon(Weapon itemToSwap)
    {
        this.weapon = itemToSwap;
        setWeaponStats(itemToSwap);
    }

    internal void setArmor(Armor itemToSwap)
    {
        this.armor = itemToSwap;
        setArmorStats(itemToSwap);
    }
}
