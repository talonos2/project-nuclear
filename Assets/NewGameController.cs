using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameController : MonoBehaviour
{

    public GameObject RunNumberTextField;
    public Vector2Int Map1EntrancePoint;
    public Vector2Int TownSpawnPosition;
    public SpriteMovement.DirectionMoved TownSpwanFacing;
    public SpriteMovement.DirectionMoved Map1Facing;
    // Start is called before the first frame update


    public void StartNewGame (){
        Text runVariable=RunNumberTextField.GetComponent <Text> ();
        Debug.Log("hmm " + runVariable.text);
        if (runVariable.text == "" || Convert.ToInt32(runVariable.text)==1)
        {
            GameData.Instance.SetNextLocation(Map1EntrancePoint, Map1Facing);
            GameData.Instance.FloorNumber = 1;
            SceneManager.LoadScene("Map1-1");
        }
        else {
            GameData.Instance.SetNextLocation(TownSpawnPosition, TownSpwanFacing);
            GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);
            GameData.Instance.FloorNumber = 0;
            SceneManager.LoadScene("TownMap_1");
        }
            
    }


}
