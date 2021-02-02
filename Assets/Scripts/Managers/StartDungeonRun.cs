using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDungeonRun : MonoBehaviour
{


    public static void StartRun()
    {
        GameData.Instance.FloorNumber = 1;
        GameData.Instance.isCutscene = false;
        GameData.Instance.SetNextLocation(new Vector2Int ( -13, -10 ), SpriteMovement.DirectionMoved.RIGHT);
        GameData.Instance.ResetTimer();

        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        fadeout.InitNext("Map1-1", .2f);
        //SceneManager.LoadScene("Map1-1");
    }



}
