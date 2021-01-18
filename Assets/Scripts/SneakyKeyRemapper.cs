using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakyKeyRemapper : MonoBehaviour
{

    void Update()
    {
        if (GameData.Instance.sneakyKeyMap != KeymapType.UNDEFINED)
        {
            GameObject.Destroy(this);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameData.Instance.sneakyKeyMap = KeymapType.ARROWS;
            FWInputManager.Instance.SetToArrowKeys();
            GameObject.Destroy(this);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            GameData.Instance.sneakyKeyMap = KeymapType.WASD;
            FWInputManager.Instance.SetToWASD();
            GameObject.Destroy(this);
        }
    }
}
