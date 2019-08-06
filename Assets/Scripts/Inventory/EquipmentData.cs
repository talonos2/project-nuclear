using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : MonoBehaviour
{

    public EquipmentDataStorage[] CommonItemList;
    public EquipmentDataStorage[] RareItemList;
    private GameObject runOneWeapon;
    private GameObject runTwoWeapon;
    private GameObject defaultArmor;

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

    public GameObject getRunOneWeapon() {
        return runOneWeapon;
    }

    public GameObject getRunTwoWeapon()
    {
        return runTwoWeapon;
    }

    public GameObject getDefaultArmor()
    {
        return defaultArmor;
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
                        runTwoWeapon = item.gameObject;
                    }
                    if (item.gameObject.name == "Hiro's Sword")
                    {
                        runOneWeapon = item.gameObject;
                    }
                    if (item.gameObject.name == "Warm Jacket")
                    {
                        defaultArmor = item.gameObject;
                    }
                    if (ItemToAdd.Rare) { RareItemList[Convert.ToInt32(floor)].EquipmentOnFloor.Add(item.gameObject); }
                    else { CommonItemList[Convert.ToInt32(floor)].EquipmentOnFloor.Add(item.gameObject); }
                }
            }         
        }

    }

}

public class EquipmentDataStorage {
    public List<GameObject> EquipmentOnFloor = new List<GameObject>();

    public GameObject RollRandomEquipment() {
        GameObject RolledEquipment = null;
        if (EquipmentOnFloor.Count == 0) {
            return RolledEquipment;
        }
        int rollItem = UnityEngine.Random.Range(0, EquipmentOnFloor.Count);
        RolledEquipment = EquipmentOnFloor[rollItem];
        return RolledEquipment;
    }


}

