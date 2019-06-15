using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FWSystems
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        GameObject EquipmentData = GameObject.Instantiate(Resources.Load("Prefabs/Singletons/EquipmentData") as GameObject);
        EquipmentData.name = "EquipmentData";
        Object.DontDestroyOnLoad(EquipmentData);
        Debug.Log("ManagersLoaded");
    }
}
