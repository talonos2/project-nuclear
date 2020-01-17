using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUIController : MonoBehaviour
{
    public ItemHolderUI weaponUIPrefab;
    public ItemHolderUI armorUIPrefab;
    public ItemHolderUI accessoryUIPrefab;
    public GameObject continueButtonHolder;
    public Text crystalHealthBonus;
    public Text crystalManaBonus;
    public Text crystalAttackBonus;
    public Text crystalDefenseBonus;
    //public GameObject playerStorage;
    public Image runCharacterBustHolder;
    public GameObject itemContainer;
    public GameObject itemUIPrefab;
    public Text itemTypeSelectedText;

    private int currentOptionSelected;
    private int currentItemSelected;
    private bool selectingAnItem;
    private List<GameObject> currentlyDisplayedItems;

    private CharacterStats savedStats;

    private float delayCounter;
    private float delayReset = .15f;
    // Start is called before the first frame update
    void Start()
    {
        //GameState.fullPause = true;
        savedStats = GameObject.Find("GameStateData").GetComponent<CharacterStats>();
        weaponUIPrefab.SetItem(savedStats.weapon);
        armorUIPrefab.SetItem(savedStats.armor);
        accessoryUIPrefab.SetItem(savedStats.accessory);
        crystalAttackBonus.text = "+" + savedStats.AttackCrystalBuff;
        crystalDefenseBonus.text = "+" + savedStats.defenseCrystalBuff;
        crystalManaBonus.text = "+" + savedStats.ManaCrystalBuff;
        crystalHealthBonus.text = "+" + savedStats.HealthCrystalBuff;
        currentOptionSelected = 0;
        currentItemSelected = -1;
        currentlyDisplayedItems = new List<GameObject>();
        populateWeaponList();
        showCurrentlySelectedOption();
        //GameObject playerObject = playerStorage.GetComponent<SpawnPlayer>().Players[GameData.Instance.RunNumber - 1];
        //search a prefab for the bust item in it's children?

        //runCharacterBustHolder.sprite=GameObject.Find("Bust").GetComponent<SpriteRenderer>().sprite;

    }

    private void showCurrentlySelectedOption()
    {
        weaponUIPrefab.gameObject.GetComponent<Image>().enabled = false;
        armorUIPrefab.gameObject.GetComponent<Image>().enabled = false;
        accessoryUIPrefab.gameObject.GetComponent<Image>().enabled = false;
        continueButtonHolder.GetComponent<Image>().enabled = false;

        if (currentOptionSelected == 0) {
            weaponUIPrefab.gameObject.GetComponent<Image>().enabled = true;
        }
        if (currentOptionSelected == 1)
        {
            armorUIPrefab.gameObject.GetComponent<Image>().enabled = true;
        }
        if (currentOptionSelected == 2)
        {
            accessoryUIPrefab.gameObject.GetComponent<Image>().enabled = true;
        }
        if (currentOptionSelected == 3)
        {
            continueButtonHolder.GetComponent<Image>().enabled = true;
        }

    }

    private void populateWeaponList()
    {
        itemTypeSelectedText.text = "Weapons";

        removeCurrentList();

        foreach (GameObject wpnObject in GameData.Instance.townWeapons) {
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

        foreach (GameObject armrObject in GameData.Instance.townArmor)
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

        foreach (GameObject accObject in GameData.Instance.townAccessories)
        {
            GameObject listItem = Instantiate(itemUIPrefab) as GameObject;
            listItem.GetComponent<ItemHolderUI>().SetItem(accObject);
            listItem.SetActive(true);
            listItem.transform.SetParent(itemContainer.transform, false);
            currentlyDisplayedItems.Add(listItem);
        }

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
            return 1;
        }
        else if (attackX < attackY)
        {
            return -1;
        }
        else { return 0; }
    }
    public static int compareArmor(GameObject x, GameObject y) {
        int defX = x.GetComponent<ItemHolderUI>().GetItem().GetComponent<Armor>().addDefense;
        int defY = y.GetComponent<ItemHolderUI>().GetItem().GetComponent<Armor>().addDefense;

        if (defX > defY)
        {
            return 1;
        }
        else if (defX < defY)
        {
            return -1;
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
            return 1;
        }
        else if (xFloor < yFloor)
        {
            return -1;
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

   //         private int currentOptionSelected;
   // private int currentItemSelected;
    //private bool selectingAnItem;
       
        /*
         *Controls The submit button selection 
         */
        if (Input.GetButtonDown("Submit"))
        {
                delayCounter = delayReset + .15f;
                if (currentOptionSelected == 0 && currentItemSelected == -1)
                {
                    if (currentlyDisplayedItems.Count == 0)
                    {
                        populateArmorList();
                        currentOptionSelected = 1;
                        showCurrentlySelectedOption();
                        
                        return;
                    }
                    else
                    {
                        currentItemSelected = 0;
                        showItemSelected();
                        return;
                    }
                }

                if (currentOptionSelected == 0 && currentItemSelected >= 0) {
                    SwapWeapon();

                    return;
                }

            if (currentOptionSelected == 1 && currentItemSelected == -1)
            {
                if (currentlyDisplayedItems.Count == 0)
                {
                    populateAccessoryList();
                    currentOptionSelected = 2;
                    showCurrentlySelectedOption();
                    return;
                }
                else
                {
                    currentItemSelected = 0;
                    showItemSelected();
                    return;
                }
            }

            if (currentOptionSelected == 1 && currentItemSelected >= 0)
            {
                SwapAccessory();
                return;
            }

            if (currentOptionSelected == 2 && currentItemSelected == -1)
            {
                if (currentlyDisplayedItems.Count == 0)
                {
                    currentOptionSelected = 3;
                    showCurrentlySelectedOption();
                    return;
                }
                else
                {
                    currentItemSelected = 0;
                    showItemSelected();
                    return;
                }
            }

            if (currentOptionSelected == 2 && currentItemSelected >= 0)
            {
                SwapAccessory();
                return;
            }

            if (currentOptionSelected == 3) {
                LoadGameButtonClicked();
            }

        }
/*
            if (Input.GetButtonDown("SelectNext"))
            {

                delayCounter = delayReset + .3f;
                hideButtonSelection();
                buttonSelected += 1;
                if (buttonSelected >= selected.Length) { buttonSelected = 0; }
                showButtonSelection();


            }
            if (Input.GetButtonDown("SelectPrevious"))
            {

                delayCounter = delayReset + .3f;
                hideButtonSelection();
                buttonSelected -= 1;
                if (buttonSelected < 0) { buttonSelected = selected.Length - 1; }
                showButtonSelection();


            }
            if (Input.GetButton("SelectNext"))
            {
                if (delayCounter <= 0)
                {
                    delayCounter = delayReset;
                    hideButtonSelection();
                    buttonSelected += 1;
                    if (buttonSelected >= selected.Length) { buttonSelected = 0; }
                    showButtonSelection();
                }
                else
                {
                    delayCounter -= Time.deltaTime;
                }
            }
            if (Input.GetButton("SelectPrevious"))
            {
                if (delayCounter <= 0)
                {
                    delayCounter = delayReset;
                    hideButtonSelection();
                    buttonSelected -= 1;
                    if (buttonSelected < 0) { buttonSelected = selected.Length - 1; }
                    showButtonSelection();
                }
                else
                {
                    delayCounter -= Time.deltaTime;
                }
            
            }
            */
    }

    private void LoadGameButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void SwapAccessory()
    {
        throw new NotImplementedException();
    }

    private void SwapWeapon()
    {
        throw new NotImplementedException();
    }

    private void showItemSelected()
    {
        foreach (GameObject itemUI in currentlyDisplayedItems)
        {
            itemUI.GetComponent<Image>().enabled = false;
        }
        currentlyDisplayedItems[currentItemSelected].GetComponent<Image>().enabled = true;
    }
}
