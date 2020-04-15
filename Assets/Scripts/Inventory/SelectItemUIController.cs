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
    public TextMeshProUGUI continueButtonText;
    public TextMeshProUGUI crystalHealthBonus;
    public TextMeshProUGUI crystalManaBonus;
    public TextMeshProUGUI crystalAttackBonus;
    public TextMeshProUGUI crystalDefenseBonus;
    //public GameObject playerStorage;
    public Image runCharacterBustHolder;
    public Image characterClassStatIcon;
    public TextMeshProUGUI characterName;
    public GameObject itemContainer;
    public GameObject itemUIPrefab;
    public TextMeshProUGUI itemTypeSelectedText;

    public Sprite hpStatIcon;
    public Sprite atkStatIcon;
    public Sprite defStatIcon;
    public Sprite mpStatIcon;

    public TMP_FontAsset continueButtonDisabled;
    public TMP_FontAsset continueButtonEnabled;

    public Image itemDetailsImage;
    public Text itemDetailsName;
    public TextMeshProUGUI itemDetailsDescription;

    private int currentOptionSelected;
    private int currentItemSelected;
    private bool selectingAnItem;
    private List<GameObject> currentlyDisplayedItems;

    private CharacterStats savedStats;

    private float delayCounter;
    private float delayReset = .15f;

    CharacterStats newPlayer;

    public CharacterStats[] Players;

    void Start()
    {
        newPlayer = Instantiate<CharacterStats>(Players[GameData.Instance.RunNumber - 1], new Vector3(1000,1000,1000), Quaternion.identity);
        GameState.fullPause = true;

        savedStats = GameObject.Find("GameStateData").GetComponent<CharacterStats>();

        weaponUIPrefab.SetItem(savedStats.weapon);
        armorUIPrefab.SetItem(savedStats.armor);
        accessoryUIPrefab.SetItem(savedStats.accessory);

        crystalAttackBonus.text = "+" + savedStats.AttackCrystalBuff;  //crystal buffs not set here
        crystalDefenseBonus.text = "+" + savedStats.defenseCrystalBuff;
        crystalManaBonus.text = "+" + savedStats.ManaCrystalBuff;
        crystalHealthBonus.text = "+" + savedStats.HealthCrystalBuff;

        currentOptionSelected = 0;
        currentItemSelected = -1;

        currentlyDisplayedItems = new List<GameObject>();
        populateWeaponList();
        ShowCurrentlySelectedOption();

        //GameObject playerObject = playerStorage.GetComponent<SpawnPlayer>().Players[GameData.Instance.RunNumber - 1];
        //search a prefab for the bust item in it's children?

        runCharacterBustHolder.sprite = newPlayer.bustSprite;
        characterName.text = newPlayer.charName;
        if      (savedStats.FighterClass) {characterClassStatIcon.sprite = atkStatIcon;}
        else if (savedStats.MageClass)    {characterClassStatIcon.sprite = mpStatIcon;}
        else if (savedStats.ScoutClass)   {characterClassStatIcon.sprite = hpStatIcon;}
        else if (savedStats.SurvivorClass){characterClassStatIcon.sprite = defStatIcon;}

    }

    private void ShowCurrentlySelectedOption()
    {
        weaponUIPrefab.gameObject.GetComponent<Image>().enabled = false;
        armorUIPrefab.gameObject.GetComponent<Image>().enabled = false;
        accessoryUIPrefab.gameObject.GetComponent<Image>().enabled = false;
        continueButtonText.font = continueButtonDisabled;
        ItemHolderUI itemToShow = null;

        if (currentOptionSelected == 0) {
            weaponUIPrefab.gameObject.GetComponent<Image>().enabled = true;
            itemToShow = weaponUIPrefab.GetComponent<ItemHolderUI>();
            itemDetailsImage.sprite = itemToShow.GetItemSprite();
            itemDetailsName.text = itemToShow.itemText.text;
            itemDetailsDescription.text = itemToShow.getItemDetails();
        }
        if (currentOptionSelected == 1)
        {
            armorUIPrefab.gameObject.GetComponent<Image>().enabled = true;
            itemToShow = armorUIPrefab.GetComponent<ItemHolderUI>();
            itemDetailsImage.sprite = itemToShow.GetItemSprite();
            itemDetailsName.text = itemToShow.itemText.text;
            itemDetailsDescription.text = itemToShow.getItemDetails();
        }
        if (currentOptionSelected == 2)
        {
            accessoryUIPrefab.gameObject.GetComponent<Image>().enabled = true;
            itemToShow = accessoryUIPrefab.GetComponent<ItemHolderUI>();
            itemDetailsImage.sprite = itemToShow.GetItemSprite();
            itemDetailsName.text = itemToShow.itemText.text;
            itemDetailsDescription.text = itemToShow.getItemDetails();
        }        

        if (currentOptionSelected == 3)
        {
            continueButtonText.font = continueButtonEnabled;
        }

    }

    private void populateWeaponList()
    {
        itemTypeSelectedText.text = "Weapons";

        removeCurrentList();

        foreach (Weapon wpnObject in GameData.Instance.townWeapons) {
            GameObject listItem = Instantiate(itemUIPrefab) as GameObject;
            listItem.GetComponent<ItemHolderUI>().SetItem(wpnObject);
            listItem.SetActive(true);
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
            GameObject listItem = Instantiate(itemUIPrefab) as GameObject;
            listItem.GetComponent<ItemHolderUI>().SetItem(armrObject);
            listItem.SetActive(true);
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
             
            if (accObject.name == "No Accessory Equipped") {
                continue; 
            }
            emptySet = false;
            GameObject listItem = Instantiate(itemUIPrefab) as GameObject;
            
            listItem.GetComponent<ItemHolderUI>().SetItem(accObject);
            listItem.SetActive(true);
            listItem.transform.SetParent(itemContainer.transform, false);
            currentlyDisplayedItems.Add(listItem);
        }
        if (emptySet) { selectingAnItem = false; }

        currentlyDisplayedItems.Sort(compareAccessories);
        SortItemListUI();


        //List<GameObject> SortedList = tempList.OrderByDescending(o => o.OrderDate).ToList();

    }

    public void SortItemListUI() {
        int i = 0;
        foreach (GameObject o in currentlyDisplayedItems)
        {
            o.transform.SetSiblingIndex(i);
            i++;
        }
    }


    public static int compareWeapons(GameObject x, GameObject y)
    {
        int attackX = x.GetComponent<ItemHolderUI>().GetItem().GetComponent<Weapon>().addAttack;
        int attackY = y.GetComponent<ItemHolderUI>().GetItem().GetComponent<Weapon>().addAttack;

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
    public static int compareArmor(GameObject x, GameObject y) {
        int defX = x.GetComponent<ItemHolderUI>().GetItem().GetComponent<Armor>().addDefense;
        int defY = y.GetComponent<ItemHolderUI>().GetItem().GetComponent<Armor>().addDefense;

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

    public static int compareAccessories(GameObject x, GameObject y)
    {

        int xFloor = 0;
        int yFloor = 0;

        string[] Floors = x.GetComponent<ItemHolderUI>().GetItem().GetComponent<Accessory>().floorFoundOn.Split(' ');

        foreach (string floor in Floors)
        {
            xFloor = Convert.ToInt32(floor);
        }

        Floors = y.GetComponent<ItemHolderUI>().GetItem().GetComponent<Accessory>().floorFoundOn.Split(' ');

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



    private void removeCurrentList() {
        foreach (GameObject itemUI in currentlyDisplayedItems) {
            GameObject.Destroy(itemUI);            
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
            //selecting weapons
                delayCounter = delayReset + .15f;
            if (currentOptionSelected == 0)
            {
                if (currentlyDisplayedItems.Count == 0)
                {
                    currentOptionSelected = 1;
                    populateItemLists();
                    ShowCurrentlySelectedOption();
                    return;
                }
                else if (!selectingAnItem)
                {
                    selectingAnItem = true;
                    currentItemSelected = 0;
                    showItemSelected();
                    return;
                }
                else if (selectingAnItem) {
                        SwapWeapon();
                    selectingAnItem = false;
                    currentOptionSelected = 1;
                    populateItemLists();
                    ShowCurrentlySelectedOption();
                    return;
                }  
            }

            if (currentOptionSelected == 1)
            {
                if (currentlyDisplayedItems.Count == 0)
                {

                    currentOptionSelected = 2;
                    populateItemLists();
                    ShowCurrentlySelectedOption();
                    return;
                }
                else if (!selectingAnItem)
                {
                    selectingAnItem = true;
                    currentItemSelected = 0;
                    showItemSelected();
                    return;
                }
                else if (selectingAnItem) {
                    SwapArmor();
                    currentOptionSelected = 2;
                    selectingAnItem = false;
                    populateItemLists();
                    ShowCurrentlySelectedOption();
                    return;
                }
            }

            if (currentOptionSelected == 2)
            {
                if (currentlyDisplayedItems.Count == 0)
                {
                    currentOptionSelected = 3;
                    ShowCurrentlySelectedOption();
                    return;
                }
                else if (!selectingAnItem)
                {
                    selectingAnItem = true;
                    currentItemSelected = 0;
                    showItemSelected();
                    return;
                }
                else if (selectingAnItem) {
                    SwapAccessory();
                    selectingAnItem = false;
                    currentOptionSelected = 3;
                    ShowCurrentlySelectedOption();
                    return;
                }
            }


            if (currentOptionSelected == 3) {
                LoadGameButtonClicked();
            }

        }

        /*
         *End of Submit Checks.  
         */



/*If Selecting to the right*/

         if (Input.GetButtonDown("SelectRight"))
         {
                delayCounter = delayReset + .3f;
            if (currentlyDisplayedItems.Count == 0)
            {
                currentOptionSelected += 1;
                if (currentOptionSelected > 3)
                    currentOptionSelected = 3;
                ShowCurrentlySelectedOption();
            }
            else if (!selectingAnItem){
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
                    currentOptionSelected += 1;
                    if (currentOptionSelected > 3)
                        currentOptionSelected = 3;
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
                currentOptionSelected -= 1;
                if (currentOptionSelected < 0)
                    currentOptionSelected = 0;
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
                    currentOptionSelected -= 1;
                    if (currentOptionSelected < 0)
                        currentOptionSelected = 0;
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
                currentOptionSelected -= 1;
                if (currentOptionSelected < 0)
                    currentOptionSelected = 0;
                populateItemLists();
                ShowCurrentlySelectedOption();
            }
            else if (selectingAnItem)
            {
                currentItemSelected -= 1 ;
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
                    currentOptionSelected -= 1;
                    if (currentOptionSelected < 0)
                        currentOptionSelected = 0;
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
                currentOptionSelected += 1;
                if (currentOptionSelected >3)
                    currentOptionSelected =3;
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
                    currentOptionSelected += 1;
                    if (currentOptionSelected > 3)
                        currentOptionSelected = 3;
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
        if (currentOptionSelected == 0) {
            populateWeaponList();
        }
        if (currentOptionSelected == 1)
        {
            populateArmorList();
        }
        if (currentOptionSelected == 2)
        {
            populateAccessoryList();
        }
    }

    private void SwapArmor()
    {
        GameData.Instance.townArmor.Add(savedStats.armor);
        InventoryItem itemToSwap= currentlyDisplayedItems[currentItemSelected].GetComponent<ItemHolderUI>().GetItem();
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
        if (savedStats.accessory.name != "No Accessory Equipped") {
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
        foreach (GameObject itemUI in currentlyDisplayedItems)
        {
            itemUI.GetComponent<Image>().enabled = false;
        }
        if (selectingAnItem)
        {
            currentlyDisplayedItems[currentItemSelected].GetComponent<Image>().enabled = true;
            ItemHolderUI itemToShow = currentlyDisplayedItems[currentItemSelected].GetComponent<ItemHolderUI>();
            itemDetailsImage.sprite= itemToShow.GetItemSprite();
            itemDetailsName.text= itemToShow.itemText.text;
            itemDetailsDescription.text= itemToShow.getItemDetails();
        }
    }
}
