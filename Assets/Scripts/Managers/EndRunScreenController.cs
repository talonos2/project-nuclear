using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRunScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            EndRunAndLoadTown();
        }
    }

    public void EndRunAndLoadTown()
    {

        GameData.Instance.RunNumber += 1;
        GameData.Instance.FloorNumber = 0;
        GameData.Instance.autoSaveStats();
        NewCrystalLevelController.SetCrystalBuffs();
        GameData.Instance.SetNextLocation(new Vector2Int(-4,-13), SpriteMovement.DirectionMoved.DOWN);
        SceneManager.LoadScene("TownMap_1");

    }
}
