﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameController : MonoBehaviour
{

    public GameObject RunNumberTextField;
    public GameObject dropDown;
    public Vector2Int Map1EntrancePoint;
    public Vector2Int TownSpawnPosition;
    public SpriteMovement.DirectionMoved TownSpwanFacing;
    public SpriteMovement.DirectionMoved Map1Facing;
    // Start is called before the first frame update


    public void StartNewGame (){
        Text runVariable=RunNumberTextField.GetComponent <Text> ();
        Dropdown runType = dropDown.GetComponent<Dropdown>();

        if (runType.value == 0)
        {
            if (runVariable.text == "" || Convert.ToInt32(runVariable.text) == 1)
            {
                GameData.Instance.SetNextLocation(Map1EntrancePoint, Map1Facing);
                GameData.Instance.FloorNumber = 1;
                SceneManager.LoadScene("Map1-1");
            }
            else
            {
                GameData.Instance.SetNextLocation(TownSpawnPosition, TownSpwanFacing);
                GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);
                GameData.Instance.FloorNumber = 0;
                SceneManager.LoadScene("TownMap_1");
            }
        }
        if (runType.value == 1) {
            GameData.Instance.isCutscene = true;
            if (runVariable.text == "" || Convert.ToInt32(runVariable.text)==0 )
            {
                GameData.Instance.RunNumber = 1;
                SceneManager.LoadScene("TownMap_1");
            }
            else {
                //load cutscene runvar-1
                GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);
                SceneManager.LoadScene("TownMap_1"); //I need to know which map each are in
            }
            
        }
        if (runType.value == 2) {
            if (runVariable.text != "")
            {
                GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);
            }
            else {
                GameData.Instance.RunNumber = 1;
            }
            GameData.Instance.SetNextLocation(Map1EntrancePoint, Map1Facing);
            GameData.Instance.FloorNumber = 1;
            SceneManager.LoadScene("Map1-1");

        }

            
    }


}
