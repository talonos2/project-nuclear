using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDungeonRun : MonoBehaviour
{


    public static void StartRun() {
        GameData.Instance.FloorNumber = 1;
        //NewCrystalLevelController.SetCrystalBuffs();//Really shouldn't be called here. Instead should be called in the post run screen after crystals are added
        GameData.Instance.SetNextLocation(new Vector2Int ( -13, -10 ), SpriteMovement.DirectionMoved.RIGHT);
        GameData.Instance.resetTimer();
        //Should really transistion to the get equipment screen
        SceneManager.LoadScene("Map1-1");
    }



}
