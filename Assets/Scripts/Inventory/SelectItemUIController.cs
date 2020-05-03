using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUIController : MonoBehaviour
{
    public ItemHolderUI weaponUIPrefab;
    public ItemHolderUI armorUIPrefab;
    public ItemHolderUI accessoryUIPrefab;
    public Image continueButton;
    public TextMeshProUGUI crystalHealthBonus;
    public TextMeshProUGUI crystalManaBonus;
    public TextMeshProUGUI crystalAttackBonus;
    public TextMeshProUGUI crystalDefenseBonus;
    //public GameObject playerStorage;
    public Image runCharacterBustHolder;
    public Image characterClassStatIcon;
    public TextMeshProUGUI characterName;
    public GameObject itemContainer;
    public ItemHolderUI itemUIPrefab;
    public TextMeshProUGUI itemTypeSelectedText;

    public Sprite hpStatIcon;
    public Sprite atkStatIcon;
    public Sprite defStatIcon;
    public Sprite mpStatIcon;

    public Image itemDetailsImage;
    public Text itemDetailsName;
    public TextMeshProUGUI itemDetailsDescription;

    private int currentEquipCategorySelected;
    private int currentItemSelected;
    private bool selectingAnItem;
    private List<ItemHolderUI> currentlyDisplayedItems;

    private CharacterStats savedStats;

    private float delayCounter;
    private float delayReset = .15f;

    CharacterStats newPlayer;

    public CharacterStats[] Players;

    private bool isHoveringOverContinueButton;

    public Sprite continueButtonOn;
    public Sprite continueButtonOff;

    void Start()
    {
        newPlayer = Instantiate<CharacterStats>(Players[GameData.Instance.RunNumber - 1], new Vector3(1000, 1000, 1000), Quaternion.identity);
        newPlayer.GetComponent<CharacterMovement>().enabled = false;
        newPlayer.transform.GetChild(0).GetChild(0).GetComponent<SpriteShadowLoader>().enabled = false;
        newPlayer.GetComponent<EntityData>().enabled = false;
        newPlayer.GetComponent<BoxCollider>().enabled = false;
        GameObject.Destroy(newPlayer.transform.GetChild(1).gameObject);
        GameState.fullPause = true;

        savedStats = GameObject.Find("GameStateData").GetComponent<CharacterStats>();

        weaponUIPrefab.SetItem(savedStats.weapon);
        armorUIPrefab.SetItem(savedStats.armor);
        accessoryUIPrefab.SetItem(savedStats.accessory);

        crystalAttackBonus.text = "+" + savedStats.AttackCrystalBuff;  //crystal buffs not set here
        crystalDefenseBonus.text = "+" + savedStats.defenseCrystalBuff;
        crystalManaBonus.text = "+" + savedStats.ManaCrystalBuff;
        crystalHealthBonus.text = "+" + savedStats.HealthCrystalBuff;

        currentEquipCategorySelected = 0;
        currentItemSelected = -1;

        currentlyDisplayedItems = new List<ItemHolderUI>();
        populateWeaponList();
        ShowCurrentlySelectedOption();

        //GameObject playerObject = playerStorage.GetComponent<SpawnPlayer>().Players[GameData.Instance.RunNumber - 1];
        //search a prefab for the bust item in it's children?

        runCharacterBustHolder.sprite = newPlayer.bustSprite;
        characterName.text = newPlayer.charName;
        if (savedStats.FighterClass) { characterClassStatIcon.sprite = atkStatIcon; }
        else if (savedStats.MageClass) { characterClassStatIcon.sprite = mpStatIcon; }
        else if (savedStats.ScoutClass) { characterClassStatIcon.sprite = hpStatIcon; }
        else if (savedStats.SurvivorClass) { characterClassStatIcon.sprite = defStatIcon; }

    }

    private void ShowCurrentlySelectedOption()
    {
        weaponUIPrefab.flashingBackground.enabled = false;
        armorUIPrefab.flashingBackground.enabled = false;
        accessoryUIPrefab.flashingBackground.enabled = false;
        UpdateContinueButton();
        ItemHolderUI itemToShow = null;

        if (currentEquipCategorySelected == 0)
        {
            weaponUIPrefab.flashingBackground.enabled = true;
            itemToShow = weaponUIPrefab;
            itemDetailsImage.sprite = itemToShow.GetItemSprite();
            itemDetailsName.text = itemToShow.itemText.text;
            itemDetailsDescription.text = itemToShow.getItemDetails();
        }
        if (currentEquipCategorySelected == 1)
        {
            armorUIPrefab.flashingBackground.enabled = true;
            itemToShow = armorUIPrefab;
            itemDetailsImage.sprite = itemToShow.GetItemSprite();
            itemDetailsName.text = itemToShow.itemText.text;
            itemDetailsDescription.text = itemToShow.getItemDetails();
        }
        if (currentEquipCategorySelected == 2)
        {
            accessoryUIPrefab.flashingBackground.enabled = true;
            itemToShow = accessoryUIPrefab;
            itemDetailsImage.sprite = itemToShow.GetItemSprite();
            itemDetailsName.text = itemToShow.itemText.text;
            itemDetailsDescription.text = itemToShow.getItemDetails();
        }
    }

    private void UpdateContinueButton()
    {
        if (currentEquipCategorySelected == 3 || isHoveringOverContinueButton)
        {
            continueButton.sprite = continueButtonOn;
        }
        else
        {
            continueButton.sprite = continueButtonOff;
        }
    }

    private void populateWeaponList()
    {
        itemTypeSelectedText.text = "Weapons";

        removeCurrentList();

        foreach (Weapon wpnObject in GameData.Instance.townWeapons)
        {
            ItemHolderUI listItem = Instantiate<ItemHolderUI>(itemUIPrefab);
            listItem.GetComponent<ItemHolderUI>().SetItem(wpnObject);
            listItem.gameObject.SetActive(true);
            listItem.transform.SetParent(itemContainer.transform, false);
            currentlyDisplayedItems.Add(listItem);
        }

        currentlyDisplayedItems.Sort(compareWeapons);
        SortItemListUI();


        //List<GameObject> SortedList = tempList.OrderByDescending(o => o.OrderDate).ToList();

    }

    private void populateArmorList()
    {
        itemTypeSelectedText.text = "Armors";

        removeCurrentList();

        foreach (Armor armrObject in GameData.Instance.townArmor)
        {
            ItemHolderUI listItem = Instantiate<ItemHolderUI>(itemUIPrefab);
            listItem.GetComponent<ItemHolderUI>().SetItem(armrObject);
            listItem.gameObject.SetActive(true);
            listItem.transform.SetParent(itemContainer.transform, false);
            currentlyDisplayedItems.Add(listItem);
        }

        currentlyDisplayedItems.Sort(compareArmor);
        SortItemListUI();


        //List<GameObject> SortedList = tempList.OrderByDescending(o => o.OrderDate).ToList();

    }

    private void populateAccessoryList()
    {
        itemTypeSelectedText.text = "Accessories";

        removeCurrentList();
        bool emptySet = true;
        foreach (Accessory accObject in GameData.Instance.townAccessories)
        {

            if (accObject.name == "No Accessory Equipped")
            {
                continue;
            }
            emptySet = false;
            ItemHolderUI listItem = Instantiate<ItemHolderUI>(itemUIPrefab);

            listItem.GetComponent<ItemHolderUI>().SetItem(accObject);
            listItem.gameObject.SetActive(true);
            listItem.transform.SetParent(itemContainer.transform, false);
            currentlyDisplayedItems.Add(listItem);
        }
        if (emptySet) { selectingAnItem = false; }

        currentlyDisplayedItems.Sort(compareAccessories);
        SortItemListUI();


        //List<GameObject> SortedList = tempList.OrderByDescending(o => o.OrderDate).ToList();

    }

    public void SortItemListUI()
    {
        int i = 0;
        foreach (ItemHolderUI o in currentlyDisplayedItems)
        {
            o.transform.SetSiblingIndex(i);
            i++;
        }
    }


    public static int compareWeapons(ItemHolderUI x, ItemHolderUI y)
    {
        int attackX = x.GetItem().GetComponent<Weapon>().addAttack;
        int attackY = y.GetItem().GetComponent<Weapon>().addAttack;

        if (attackX > attackY)
        {
            return -1;
        }
        else if (attackX < attackY)
        {
            return 1;
        }
        else { return 0; }
    }
    public static int compareArmor(ItemHolderUI x, ItemHolderUI y)
    {
        int defX = x.GetItem().GetComponent<Armor>().addDefense;
        int defY = y.GetItem().GetComponent<Armor>().addDefense;

        if (defX > defY)
        {
            return -1;
        }
        else if (defX < defY)
        {
            return 1;
        }
        else { return 0; }
    }

    public static int compareAccessories(ItemHolderUI x, ItemHolderUI y)
    {

        int xFloor = 0;
        int yFloor = 0;

        string[] Floors = x.GetItem().GetComponent<Accessory>().floorFoundOn.Split(' ');

        foreach (string floor in Floors)
        {
            xFloor = Convert.ToInt32(floor);
        }

        Floors = y.GetItem().GetComponent<Accessory>().floorFoundOn.Split(' ');

        foreach (string floor in Floors)
        {
            yFloor = Convert.ToInt32(floor);
        }

        if (xFloor > yFloor)
        {
            return -1;
        }
        else if (xFloor < yFloor)
        {
            return 1;
        }
        else { return 0; }
    }



    private void removeCurrentList()
    {
        foreach (ItemHolderUI itemUI in currentlyDisplayedItems)
        {
            GameObject.Destroy(itemUI.gameObject);
        }
        currentlyDisplayedItems.Clear();
    }

    // Update is called once per frame
    void Update()
    {

        /*
         *Controls The submit button selection 
         */
        if (Input.GetButtonDown("Submit"))
        {
            delayCounter = delayReset + .15f;
            if (currentEquipCategorySelected == 3)
            {
                LoadGameButtonClicked();
                return;
            }

            if (selectingAnItem)
            {
                switch (currentEquipCategorySelected)
                {
                    case 0: //Selecting a Weapon
                        SwapWeapon();
                        break;
                    case 1: //Going into Armor Selection
                        SwapArmor();
                        break;
                    case 2: //Going into Accessory Selection
                        SwapAccessory();
                        break;
                }
                currentEquipCategorySelected++;
                selectingAnItem = false;
                populateItemLists();
                ShowCurrentlySelectedOption();
            }
            else
            {
                if (currentlyDisplayedItems.Count == 0)
                {
                    currentEquipCategorySelected = (currentEquipCategorySelected + 1) % 4;
                    populateItemLists();
                    ShowCurrentlySelectedOption();
                    return;
                }

                selectingAnItem = true;
                currentItemSelected = 0;
                showItemSelected();
            }
        }

        /*If Selecting to the right*/

        if (Input.GetButtonDown("SelectRight"))
        {
            delayCounter = delayReset + .3f;
            if (currentlyDisplayedItems.Count == 0)
            {
                currentEquipCategorySelected += 1;
                if (currentEquipCategorySelected > 3)
                    currentEquipCategorySelected = 3;
                ShowCurrentlySelectedOption();
            }
            else if (!selectingAnItem)
            {
                currentItemSelected = 0;
                selectingAnItem = true;
                showItemSelected();
            }
        }

        if (Input.GetButton("SelectRight"))
        {
            if (delayCounter <= 0)
            {
                delayCounter = delayReset;
                if (currentlyDisplayedItems.Count == 0)
                {
                    currentEquipCategorySelected += 1;
                    if (currentEquipCategorySelected > 3)
                        currentEquipCategorySelected = 3;
                    ShowCurrentlySelectedOption();
                }
                else if (!selectingAnItem)
                {
                    currentItemSelected = 0;
                    selectingAnItem = true;
                    showItemSelected();
                }
            }
            else
            {
                delayCounter -= Time.deltaTime;
            }
        }

        //Selcting to the left
        if (Input.GetButtonDown("SelectLeft"))
        {
            delayCounter = delayReset + .3f;
            if (currentlyDisplayedItems.Count == 0 || !selectingAnItem)
            {
                currentEquipCategorySelected -= 1;
                if (currentEquipCategorySelected < 0)
                    currentEquipCategorySelected = 0;
                ShowCurrentlySelectedOption();
            }
            else if (selectingAnItem)
            {
                selectingAnItem = false;
                showItemSelected();
            }


        }

        if (Input.GetButton("SelectLeft"))
        {
            if (delayCounter <= 0)
            {
                delayCounter = delayReset;
                if (currentlyDisplayedItems.Count == 0 || !selectingAnItem)
                {
                    currentEquipCategorySelected -= 1;
                    if (currentEquipCategorySelected < 0)
                        currentEquipCategorySelected = 0;
                    ShowCurrentlySelectedOption();
                }
                else if (selectingAnItem)
                {
                    selectingAnItem = false;
                    showItemSelected();
                }
            }
            else
            {
                delayCounter -= Time.deltaTime;
            }
        }

        //Selecting Up
        if (Input.GetButtonDown("SelectUp"))
        {
            delayCounter = delayReset + .3f;
            if (!selectingAnItem)
            {
                currentEquipCategorySelected -= 1;
                if (currentEquipCategorySelected < 0)
                    currentEquipCategorySelected = 0;
                populateItemLists();
                ShowCurrentlySelectedOption();
            }
            else if (selectingAnItem)
            {
                currentItemSelected -= 1;
                if (currentItemSelected < 0)
                    currentItemSelected = 0;
                showItemSelected();
            }
        }

        if (Input.GetButton("SelectUp"))
        {
            if (delayCounter <= 0)
            {
                delayCounter = delayReset;
                if (!selectingAnItem)
                {
                    currentEquipCategorySelected -= 1;
                    if (currentEquipCategorySelected < 0)
                        currentEquipCategorySelected = 0;
                    populateItemLists();
                    ShowCurrentlySelectedOption();
                }
                else if (selectingAnItem)
                {
                    currentItemSelected -= 1;
                    if (currentItemSelected < 0)
                        currentItemSelected = 0;
                    showItemSelected();
                }
            }
            else
            {
                delayCounter -= Time.deltaTime;
            }
        }

        //SelectingDownOption
        if (Input.GetButtonDown("SelectDown"))
        {
            delayCounter = delayReset + .3f;
            if (!selectingAnItem)
            {
                currentEquipCategorySelected += 1;
                if (currentEquipCategorySelected > 3)
                    currentEquipCategorySelected = 3;
                populateItemLists();
                ShowCurrentlySelectedOption();
            }
            else if (selectingAnItem)
            {
                currentItemSelected += 1;
                if (currentItemSelected >= currentlyDisplayedItems.Count)
                    currentItemSelected -= 1;
                showItemSelected();
            }
        }

        if (Input.GetButton("SelectDown"))
        {
            if (delayCounter <= 0)
            {
                delayCounter = delayReset;
                if (!selectingAnItem)
                {
                    currentEquipCategorySelected += 1;
                    if (currentEquipCategorySelected > 3)
                        currentEquipCategorySelected = 3;
                    populateItemLists();
                    ShowCurrentlySelectedOption();
                }
                else if (selectingAnItem)
                {
                    currentItemSelected += 1;
                    if (currentItemSelected >= currentlyDisplayedItems.Count)
                        currentItemSelected -= 1;
                    showItemSelected();
                }
            }
            else
            {
                delayCounter -= Time.deltaTime;
            }
        }



    }

    private void populateItemLists()
    {
        if (currentEquipCategorySelected == 0)
        {
            populateWeaponList();
        }
        if (currentEquipCategorySelected == 1)
        {
            populateArmorList();
        }
        if (currentEquipCategorySelected == 2)
        {
            populateAccessoryList();
        }
    }

    private void SwapArmor()
    {
        GameData.Instance.townArmor.Add(savedStats.armor);
        InventoryItem itemToSwap = currentlyDisplayedItems[currentItemSelected].GetComponent<ItemHolderUI>().GetItem();
        savedStats.setArmor((Armor)itemToSwap);
        GameData.Instance.townArmor.Remove((Armor)itemToSwap);
        armorUIPrefab.SetItem(itemToSwap);
        showItemSelected();
        ShowCurrentlySelectedOption();
    }

    public void LoadGameButtonClicked()
    {
        GameState.fullPause = false;
        StartDungeonRun.StartRun();
    }

    private void SwapAccessory()
    {
        if (savedStats.accessory.name != "No Accessory Equipped")
        {
            GameData.Instance.townAccessories.Add(savedStats.accessory);
        }
        InventoryItem itemToSwap = currentlyDisplayedItems[currentItemSelected].GetComponent<ItemHolderUI>().GetItem();
        savedStats.setAccessory((Accessory)itemToSwap);
        savedStats.setFullHPMP();
        GameData.Instance.townAccessories.Remove((Accessory)itemToSwap);
        accessoryUIPrefab.SetItem(itemToSwap);
        populateItemLists();
        selectingAnItem = false;
        showItemSelected();
        ShowCurrentlySelectedOption();
    }

    private void SwapWeapon()
    {
        GameData.Instance.townWeapons.Add(savedStats.weapon);
        InventoryItem itemToSwap = currentlyDisplayedItems[currentItemSelected].GetComponent<ItemHolderUI>().GetItem();
        savedStats.setWeapon((Weapon)itemToSwap);
        GameData.Instance.townWeapons.Remove((Weapon)itemToSwap);
        weaponUIPrefab.SetItem(itemToSwap);
        showItemSelected();
        ShowCurrentlySelectedOption();
    }

    private void showItemSelected()
    {
        foreach (ItemHolderUI itemUI in currentlyDisplayedItems)
        {
            itemUI.flashingBackground.enabled = false;
        }

        if (selectingAnItem)
        {
            currentlyDisplayedItems[currentItemSelected].flashingBackground.enabled = true;
            ItemHolderUI itemToShow = currentlyDisplayedItems[currentItemSelected];
            itemDetailsImage.sprite = itemToShow.GetItemSprite();
            itemDetailsName.text = itemToShow.itemText.text;
            itemDetailsDescription.text = itemToShow.getItemDetails();
        }
    }

    public void ContinueButtonHovered(bool hovered)
    {
        isHoveringOverContinueButton = hovered;
        UpdateContinueButton();
    }
}
