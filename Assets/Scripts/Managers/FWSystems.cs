using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FWSystems
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        GameObject EquipmentData = GameObject.Instantiate(Resources.Load("Prefabs/Singletons/EquipmentData") as GameObject);
        EquipmentData.name = "EquipmentData";
        Object.DontDestroyOnLoad(EquipmentData);


        GameObject EnemyData = GameObject.Instantiate(Resources.Load("Prefabs/Singletons/EnemyData") as GameObject);
        EnemyData.name = "EnemyData";
        Object.DontDestroyOnLoad(EnemyData);

        GameObject GameStateData = GameObject.Instantiate(Resources.Load("Prefabs/Singletons/GameStateData") as GameObject);
        GameStateData.name = "GameStateData";
        Object.DontDestroyOnLoad(GameStateData);

        Debug.Log("ManagersLoaded");
    }
}
