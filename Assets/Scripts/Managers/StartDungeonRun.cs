using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDungeonRun : MonoBehaviour
{


    public static void StartRun()
    {
        GameData.Instance.FloorNumber = 1;
        GameData.Instance.SetNextLocation(new Vector2Int ( -13, -10 ), SpriteMovement.DirectionMoved.RIGHT);
        GameData.Instance.ResetTimer();

        SceneManager.LoadScene("Map1-1");
    }



}
