using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : MonoBehaviour
{

    public EquipmentDataStorage[] CommonItemList;
    public EquipmentDataStorage[] RareItemList;
    private Weapon runOneWeapon;
    private Weapon runTwoWeapon;
    private Armor defaultArmor;
    private Accessory noAccessoryItem;

    private bool EquipmentListCreated=false;
    void Start() {
        if (!EquipmentListCreated) {
            CommonItemList = new EquipmentDataStorage[21];
            RareItemList = new EquipmentDataStorage[21];
            for (int i = 0; i < 21; i++) {
                CommonItemList[i] = new EquipmentDataStorage();
                RareItemList[i] = new EquipmentDataStorage();
            }            
            GenerateEquipmentList();
            EquipmentListCreated = true;
        }

    }

    public Weapon getRunOneWeapon() {
        return runOneWeapon;
    }

    public Weapon getRunTwoWeapon()
    {
        return runTwoWeapon;
    }

    public Armor getDefaultArmor()
    {
        return defaultArmor;
    }

    public InventoryItem getRandomCommonItem(int floor) {
        if (floor < 1 || floor > 20) return null;
        int numberOfItems = CommonItemList[floor].EquipmentOnFloor.Count;
        if (numberOfItems == 0) return null;

        return CommonItemList[floor].EquipmentOnFloor[UnityEngine.Random.Range(0, numberOfItems)];
    }

    public InventoryItem getRandomRareItem(int floor) {
        {
            if (floor < 1 || floor > 20) return null;
            if (floor == 1) floor = 2;
            int numberOfItems = RareItemList[floor].EquipmentOnFloor.Count;
            if (numberOfItems == 0) return null;
            return RareItemList[floor].EquipmentOnFloor[UnityEngine.Random.Range(0, numberOfItems)];
        }
    }


    private void GenerateEquipmentList() {
        //GameObject[] ParentDatabase = this.GetComponentsInParent<GameObject>();
        foreach (Transform item in this.transform) {
            InventoryItem ItemToAdd = item.gameObject.GetComponent<InventoryItem>();
           // Accessory AccessoryToAdd = item.gameObject.GetComponent<Accessory>();
          //  Armor ArmorToAdd = item.gameObject.GetComponent<Armor>();
          //  item.
            if (ItemToAdd != null) {
                string[] Floors = ItemToAdd.floorFoundOn.Split(' ');
                foreach (string floor in Floors)
                {
                    if (item.gameObject.name == "Knife") {
                        runTwoWeapon = item.gameObject.GetComponent<Weapon>();
                    }
                    if (item.gameObject.name == "Hiro's Sword")
                    {
                        runOneWeapon = item.gameObject.GetComponent<Weapon>();
                    }
                    if (item.gameObject.name == "Warm Jacket")
                    {
                        defaultArmor = item.gameObject.GetComponent<Armor>();
                    }
                    if (item.gameObject.name == "No Accessory Equipped") {
                        noAccessoryItem = item.gameObject.GetComponent<Accessory>();
                    }

                    if (ItemToAdd.Rare) { RareItemList[Convert.ToInt32(floor)].EquipmentOnFloor.Add(ItemToAdd); }
                    else { CommonItemList[Convert.ToInt32(floor)].EquipmentOnFloor.Add(ItemToAdd); }
                }
            }         
        }

    }

    internal Accessory getEmptyAccessory()
    {
        return noAccessoryItem;
    }
}

public class EquipmentDataStorage {
    public List<InventoryItem> EquipmentOnFloor = new List<InventoryItem>();

    public InventoryItem RollRandomEquipment() {
        InventoryItem RolledEquipment = null;
        if (EquipmentOnFloor.Count == 0) {
            return RolledEquipment;
        }
        int rollItem = UnityEngine.Random.Range(0, EquipmentOnFloor.Count);
        RolledEquipment = EquipmentOnFloor[rollItem];
        return RolledEquipment;
    }


}

