﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HealingType;

public class RandomChestController : EntityData
{
    // Start is called before the first frame update
    public PowerupEffect powerUpEffect;

    private GameObject MapGrid;
    private Vector2 MapZeroLocation;
    private Vector2Int ChestLocation;
    private GameObject gameStateData;
    private GameObject equipmentData;
    private GameData gameData;
    private EquipmentData itemListData;
    private GameObject instanciatedObject;

    public int itemChestChance = 45;
    public int goldItemChestChance = 5;
    public int crystalChance = 20;
    public int healingChance = 15;
    public int manaChance = 15;
    public int healingFactor = 12;
    public int manaFactor = 12;

    public bool active = true;
    public bool attackCrystal;
    public bool armorCrystal;
    public bool healthCrystal;
    public bool manaCrystal;
    public bool itemChest;
    public bool rareItemChest;
    public bool healingFountain;
    public bool manaFountain;
    public bool presetItem;
    public float framesPerSecond;
    private float timeSinceLastFrame=0;
    private int currentFrame = 0;
    private float offsetFix = .00001f;

    //private Renderer sRender;
    public GameObject fountainPrefab;
    public GameObject itemPrefab;
    public GameObject itemBreaking;
    public GameObject rareItemPrefab;
    public GameObject rareItemBreaking;
    public GameObject crystalPrefab;
    public GameObject manaFountainBreaking;
    public GameObject healthFountainBreaking;
    public GameObject crystalBreaking;
    public GameObject crystalBreakingGreen;
    public GameObject crystalBreakingRed;
    public GameObject crystalBreakingYellow;

    void Start()
    {
        InitializeNewMap();
        InitializeSpriteLocation();
        gameData = GameData.Instance;
        itemListData = GameObject.Find("EquipmentData").GetComponent<EquipmentData>();
        RollRandomChest();
        InstantiateChestSprite();
        if (attackCrystal || armorCrystal || healthCrystal || manaCrystal) {
            framesPerSecond = 6;
        }
        if (healingFountain || manaFountain)
        {
            framesPerSecond = 6;
        }
    }

    private void InstantiateChestSprite()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        if (attackCrystal) {
            instanciatedObject= Instantiate(crystalPrefab, this.transform.position+new Vector3(0,.5f,-10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
            currentFrame = 16;
            sRender.material.SetFloat("_Frame", currentFrame+ offsetFix);
        }
        if (armorCrystal) {
            instanciatedObject = Instantiate(crystalPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
            currentFrame = 32;
            sRender.material.SetFloat("_Frame", currentFrame+ offsetFix);
        }
        if (healthCrystal) {
            instanciatedObject = Instantiate(crystalPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
            currentFrame = 48;
            sRender.material.SetFloat("_Frame", currentFrame+ offsetFix);
        }
        if (manaCrystal) {
            instanciatedObject = Instantiate(crystalPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
            currentFrame = 0;
            sRender.material.SetFloat("_Frame", currentFrame+offsetFix);
        }
        if (itemChest) {
            instanciatedObject = Instantiate(itemPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
        }
        if (rareItemChest) {
            instanciatedObject = Instantiate(rareItemPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
        }
        if (healingFountain) {
            instanciatedObject = Instantiate(fountainPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
            currentFrame = 10;
            sRender.material.SetFloat("_Frame", currentFrame+ offsetFix);
        }
        if (manaFountain) {
            instanciatedObject = Instantiate(fountainPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
            currentFrame = 20;
            sRender.material.SetFloat("_Frame", currentFrame+ offsetFix);
        }        
    }

    void Update()
    {
        if (GameState.isInBattle || GameState.getFullPauseStatus())
        {
            return;
        }
        if (active && !(itemChest || rareItemChest) ) AnimateItem();
    }

   
    private void AnimateItem()
    {
       
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= (1f / framesPerSecond)) {
            timeSinceLastFrame = 0;
            if (healingFountain) {
                currentFrame += 1;
                if (currentFrame > 19)
                    currentFrame = 10;
            }
            if (manaFountain) {
                currentFrame += 1;
                if (currentFrame > 29)
                    currentFrame = 20;
            }
            if (attackCrystal)
            {
                currentFrame += 1;
                if (currentFrame > 31)
                    currentFrame = 16;
            }
            if (manaCrystal)
            {
                currentFrame += 1;
                if (currentFrame > 15)
                    currentFrame = 0;
            }
            if (healthCrystal)
            {
                currentFrame += 1;
                if (currentFrame > 63)
                    currentFrame = 48;
            }
            if (armorCrystal)
            {
                currentFrame += 1;
                if (currentFrame > 47)
                    currentFrame = 32;
            }
            
            sRender.material.SetFloat("_Frame", currentFrame+ offsetFix);
        }
    }

    private void RollRandomChest()
    {

        if (presetItem) return;

        int itemRolled = UnityEngine.Random.Range(0, 100)+1;
        
        if (itemRolled <= itemChestChance)
        {
            itemChest = true;
        }
        else if (itemRolled <= (itemChestChance + goldItemChestChance))
        {
            if (gameData.FloorNumber == 1) RollRandomChest();
            else rareItemChest = true;
        }
        else if (itemRolled <= (itemChestChance + goldItemChestChance + crystalChance))
        {
            int crystalRolled = UnityEngine.Random.Range(0, 4) + 1;
            if (crystalRolled!=4) crystalRolled = UnityEngine.Random.Range(0, 4) + 1;
            switch (crystalRolled) {
                case 1:
                    attackCrystal = true;
                    break;
                case 2:
                    armorCrystal = true;
                    break;
                case 3:
                    healthCrystal = true;
                    break;
                case 4:
                    manaCrystal = true;
                    break;
                default: break;
            }

        }
        else if (itemRolled <= (itemChestChance + goldItemChestChance + crystalChance + healingChance))
        {
            healingFountain = true;
        }
        else if (itemRolled <= (itemChestChance + goldItemChestChance + crystalChance + manaChance + healingChance)) {
            if (gameData.PowersGained == 0) RollRandomChest();
            else manaFountain = true;
        }

    }

    private void SpawnCrystalParticles(CrystalType type, int crystalsGained, CharacterStats playerData)
    {
        SoundManager.Instance.PlaySound("CrystalShatter", 1);
        int numberOfParticles = Mathf.RoundToInt(Mathf.Sqrt(crystalsGained));

        List<float> delays = new List<float>();
        for (int x = 0; x < numberOfParticles; x++)
        {
            float delay = UnityEngine.Random.Range(0, Mathf.Sqrt(x)/35);
            delays.Add(delay);
        }

        delays.Sort();

        foreach (float delay in delays)
        {
                PowerupEffect pe = GameObject.Instantiate<PowerupEffect>(powerUpEffect, this.transform.position, Quaternion.identity);
            pe.Initialize(this.transform.GetChild(0).position, playerData.transform.GetChild(0).GetChild(0), delay, type);
        }
    }

    public override void ProcessClick(CharacterStats playerData) {

        if (!active) { return; }

        int rarity = GetFloorRarity();
        int amountGained=0;

        if (attackCrystal) {
            amountGained = (int)(9.0f * Mathf.Pow((rarity + 3) / 4f, 2));
            SpawnCrystalParticles(CrystalType.ATTACK, amountGained, playerData);
            playerData.AttackCrystalsGained += amountGained;
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<GabTextController>().AddGabToPlay("<B>" + amountGained + "</B><sprite=5>crystals absorbed into the magic ring. Attack is being <B>fortified</B>.");
            Instantiate(crystalBreakingRed, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (armorCrystal)
        {
            amountGained = (int)(9.0f * Mathf.Pow((rarity + 3) / 4f, 2));
            SpawnCrystalParticles(CrystalType.DEFENSE, amountGained, playerData);
            playerData.defenseCrystalsGained += amountGained;
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<GabTextController>().AddGabToPlay("<B>" + amountGained + "</B><sprite=3>crystals absorbed into the magic ring. Defense is being <B>fortified</B>.");
            Instantiate(crystalBreakingYellow, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (healthCrystal)
        {
            amountGained = (int)(9.0f * Mathf.Pow((rarity + 3) / 4f, 2));
            SpawnCrystalParticles(CrystalType.HEALTH, amountGained, playerData);
            playerData.HealthCrystalsGained += amountGained;
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<GabTextController>().AddGabToPlay("<B>" + amountGained + "</B><sprite=4>crystals absorbed into the magic ring. Health is being <B>fortified</B>.");
            Instantiate(crystalBreakingGreen, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (manaCrystal)
        {
            amountGained = (int)(9.0f * Mathf.Pow((rarity + 3) / 4f, 2));
            SpawnCrystalParticles(CrystalType.MANA, amountGained, playerData);
            playerData.ManaCrystalsGained += amountGained;
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<GabTextController>().AddGabToPlay("<B>"+amountGained+ "</B><sprite=6>crystals absorbed into the magic ring. Mana is being fortified.");
            Instantiate(crystalBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (itemChest)
        {


            InventoryItem itemFound=itemListData.getRandomCommonItem(rarity);
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<ChooseItemUI>().SetupItemChoiceDisplay(playerData, itemFound);


            
            Instantiate(itemBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);

            SoundManager.Instance.PlaySound("potBreak", 1f);
        }
        else if (rareItemChest)
        {
            FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.FIND_RARE_ITEM);
            InventoryItem rareItemFound = itemListData.getRandomRareItem(rarity);
            //InventoryItem itemTypeCheck = rareItemFound.GetComponent<InventoryItem>();
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<ChooseItemUI>().SetupItemChoiceDisplay(playerData, rareItemFound);

            Instantiate(rareItemBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);

            SoundManager.Instance.PlaySound("potBreakGold", 1f);

        }
        else if (healingFountain)
        {
            amountGained = 25 + rarity * healingFactor;
            playerData.HP += amountGained;
            if (playerData.HP > playerData.MaxHP) playerData.HP = playerData.MaxHP;
            Instantiate(healthFountainBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
            SoundManager.Instance.PlaySound("Pool", 1f);
            playerData.gameObject.GetComponent<HealingAnimationController>().PlayHealingAnimation(HEALTH);
        }
        else if (manaFountain)
        {
            amountGained = 25 + rarity * manaFactor;
            playerData.mana += amountGained;
            if (playerData.mana > playerData.MaxMana) playerData.mana = playerData.MaxMana;
            Instantiate(manaFountainBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
            SoundManager.Instance.PlaySound("Pool", 1f);
            playerData.gameObject.GetComponent<HealingAnimationController>().PlayHealingAnimation(MANA);
        }

        playerData.PushCharacterData();

        active = false;
}

    private int GetFloorRarity()
    {
        int result=1;
        int currentFloor = gameData.FloorNumber;
        int roll = UnityEngine.Random.Range(0, 100) + 1;
        if (roll <= 30)
        {
            result = currentFloor - 1;
        }
        else if (roll <= 85) {
            result = currentFloor;
        }else { result = currentFloor + 1; }

        if (result < 1) result = 1;
        if (result > 20) result = 20;

        return result;

    }

    // Update is called once per frame
  

    private void InitializeNewMap()
    {
        MapGrid = GameObject.Find("Grid"); 
        if (isOnCutsceneMap) MapGrid = GameObject.Find("Grid2");
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
    }

    private void InitializeSpriteLocation()
    {
        ChestLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        ChestLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[ChestLocation.x, ChestLocation.y] = this.gameObject;
    }
}
