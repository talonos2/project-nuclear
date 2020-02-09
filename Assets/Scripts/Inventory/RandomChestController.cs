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
    public float framesPerSecond;
    private float timeSinceLastFrame=0;
    private int currentFrame = 0;
    private float offsetFix = .00001f;

    private Renderer sRender;
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
        if (GameState.isInBattle || GameState.fullPause)
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
            Instantiate(crystalBreakingRed, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (armorCrystal)
        {
            amountGained = (int)(9 * Mathf.Pow((rarity + 3) / 4, 2));
            playerData.defenseCrystalsGained += amountGained;
            Instantiate(crystalBreakingYellow, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (healthCrystal)
        {
            amountGained = (int)(9 * Mathf.Pow((rarity + 3) / 4, 2));
            playerData.HealthCrystalsGained += amountGained;
            Instantiate(crystalBreakingGreen, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (manaCrystal)
        {
            amountGained = (int)(9 * Mathf.Pow((rarity + 3) / 4, 2));
            playerData.ManaCrystalsGained += amountGained;
            Instantiate(crystalBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (itemChest)
        {
            InventoryItem itemFound=itemListData.getRandomCommonItem(rarity);
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<ChooseItemUI>().setupItemChoiceDisplay(playerData, itemFound);


            
            Instantiate(itemBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (rareItemChest)
        {
            InventoryItem rareItemFound = itemListData.getRandomRareItem(rarity);
            //InventoryItem itemTypeCheck = rareItemFound.GetComponent<InventoryItem>();
            GameObject uiController = GameObject.FindGameObjectWithTag("DungeonUI");
            uiController.GetComponent<ChooseItemUI>().setupItemChoiceDisplay(playerData, rareItemFound);

            Instantiate(rareItemBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);

        }
        else if (healingFountain)
        {
            amountGained = 20 + rarity * healingFactor;
            playerData.HP += amountGained;
            if (playerData.HP > playerData.MaxHP) playerData.HP = playerData.MaxHP;
            Instantiate(healthFountainBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
        }
        else if (manaFountain)
        {
            amountGained = 20 + rarity * manaFactor;
            playerData.mana += amountGained;
            if (playerData.mana > playerData.MaxMana) playerData.mana = playerData.MaxMana;
            Instantiate(manaFountainBreaking, this.transform.position + new Vector3(0, .5f, -10), Quaternion.identity, this.transform);
            Destroy(instanciatedObject);
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
