using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChestController : EntityData
{
    // Start is called before the first frame update

    private GameObject MapGrid;
    private Vector2 MapZeroLocation;
    private Vector2Int ChestLocation;
    private GameObject gameStateData;
    private GameObject equipmentData;
    private GameData gameData;
    private EquipmentData itemListData;

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
    public float framesPerSecond;
    private float timeSinceLastFrame=0;
    private int currentFrame = 0;

    private Renderer sRender;
    public GameObject fountainPrefab;
    public GameObject itemPrefab;
    public GameObject rareItemPrefab;
    public GameObject crystalPrefab;

    void Start()
    {
        InitializeNewMap();
        InitializeSpriteLocation();
        gameStateData = GameObject.Find("GameStateData");
        gameData = gameStateData.GetComponent<GameData>();
        itemListData = GameObject.Find("EquipmentData").GetComponent<EquipmentData>();
        RollRandomChest();
        InstantiateChestSprite();
        if (attackCrystal || armorCrystal || healthCrystal || manaCrystal) {
            framesPerSecond = 8;
        }
        if (healingFountain || manaFountain)
        {
            framesPerSecond = 6;
        }
    }

    private void InstantiateChestSprite()
    {


        this.GetComponent<SpriteRenderer>().enabled = false;
        if (attackCrystal) { Instantiate(crystalPrefab, this.transform.position+new Vector3(0,.5f,-10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
        }
        if (armorCrystal) { Instantiate(crystalPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
        }
        if (healthCrystal) { Instantiate(crystalPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
        }
        if (manaCrystal) { Instantiate(crystalPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
        }
        if (itemChest) { Instantiate(itemPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
        }
        if (rareItemChest) { Instantiate(rareItemPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
        }
        if (healingFountain) { Instantiate(fountainPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
            currentFrame = 10;
            sRender.material.SetInt("_Frame", currentFrame);
        }
        if (manaFountain) { Instantiate(fountainPrefab, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            this.sRender = this.GetComponentInChildren<MeshRenderer>();
            this.sRender.material = new Material(this.sRender.material);
            currentFrame = 20;
            sRender.material.SetInt("_Frame", currentFrame);
        }
        
    }

    void Update()
    {

        if (GameState.isInBattle == true)
        {
            return;
        }

        if (active && !(itemChest || rareItemChest) ) AnimateChest();
    }

    private void AnimateChest()
    {
       
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= (1f / framesPerSecond)) {
            timeSinceLastFrame = 0;
            if (healingFountain) {
                currentFrame += 1;
                if (currentFrame > 9)
                    currentFrame = 0;
                sRender.material.SetInt("_Frame", currentFrame);
            }
            if (manaFountain) {
                currentFrame += 1;
                if (currentFrame > 29)
                    currentFrame = 20;
                sRender.material.SetInt("_Frame", currentFrame);
            }
            
        }
    }

    private void RollRandomChest()
    {
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

    public override void ProcessClick(CharacterStats playerData) {

        if (!active) { return; }

        int rarity = GetFloorRarity();
        int amountGained=0;

        if (attackCrystal) {
            amountGained = (int)(9 * Mathf.Pow((rarity + 3) / 4, 2));
            playerData.AttackCrystalsGained += amountGained;            
        }
        else if (armorCrystal)
        {
            amountGained = (int)(9 * Mathf.Pow((rarity + 3) / 4, 2));
            playerData.defenseCrystalsGained += amountGained;
        }
        else if (healthCrystal)
        {
            amountGained = (int)(9 * Mathf.Pow((rarity + 3) / 4, 2));
            playerData.HealthCrystalsGained += amountGained;
        }
        else if (manaCrystal)
        {
            amountGained = (int)(9 * Mathf.Pow((rarity + 3) / 4, 2));
            playerData.ManaCrystalsGained += amountGained;
        }
        else if (itemChest)
        {
            GameObject itemFound=itemListData.getRandomCommonItem(rarity);
            InventoryItem itemTypeCheck = itemFound.GetComponent<InventoryItem>();
            if (itemTypeCheck.Weapon)
            {
               if (playerData.weapon!=null) gameData.townWeapons.Add(playerData.weapon);
                playerData.weapon = itemFound;
                playerData.setWeaponStats(itemFound);
            }
            else if (itemTypeCheck.Armor) {
                if (playerData.armor != null) gameData.townArmor.Add(playerData.armor);
                playerData.armor = itemFound;
                playerData.setArmorStats(itemFound);
            }
            else if (itemTypeCheck.Accessory){
                if (playerData.accessory != null) gameData.townAccessories.Add(playerData.accessory);
                playerData.accessory = itemFound;
                playerData.setAccessoryStats(itemFound);
            }                        
        }
        else if (rareItemChest)
        {
            GameObject rareItemFound = itemListData.getRandomRareItem(rarity);
            InventoryItem itemTypeCheck = rareItemFound.GetComponent<InventoryItem>();
            if (itemTypeCheck.Weapon)
            {
                if (playerData.weapon != null) gameData.townWeapons.Add(playerData.weapon);
                playerData.weapon = rareItemFound;
                playerData.setWeaponStats(rareItemFound);
            }
            else if (itemTypeCheck.Armor)
            {
                if (playerData.armor != null) gameData.townArmor.Add(playerData.armor);
                playerData.armor = rareItemFound;
                playerData.setArmorStats(rareItemFound);
            }
            else if (itemTypeCheck.Accessory)
            {
                if (playerData.accessory != null) gameData.townAccessories.Add(playerData.accessory);
                playerData.accessory = rareItemFound;
                playerData.setAccessoryStats(rareItemFound);
            }
        }
        else if (healingFountain)
        {
            amountGained = 20 + rarity * healingFactor;
            playerData.HP += amountGained;
            if (playerData.HP > playerData.MaxHP) playerData.HP = playerData.MaxHP;
        }
        else if (manaFountain)
        {
            amountGained = 20 + rarity * manaFactor;
            playerData.mana += amountGained;
            if (playerData.mana > playerData.MaxMana) playerData.mana = playerData.MaxMana;
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
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));

    }

    private void InitializeSpriteLocation()
    {
        ChestLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        ChestLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[ChestLocation.x, ChestLocation.y] = this.gameObject;

    }
}
