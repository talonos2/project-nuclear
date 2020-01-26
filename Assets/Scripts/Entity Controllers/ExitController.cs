using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : DoodadData
{
    // Start is called before the first frame update


    public String mapToLoad;
    public Vector2Int exitPosition;
    public bool townMap;
    public SpriteMovement.DirectionMoved exitFacing;


    public void TransitionMap()
    {
        if (GameData.Instance.FloorNumber==0) {

            GameState.fullPause = false;
            CutsceneLoader.LoadCutscene();
            return;
        }


        GameData gameData= GameData.Instance;
        gameData.FloorNumber +=1;
        if (townMap) {
            gameData.FloorNumber = 0;
        }

        gameData.SetNextLocation(exitPosition, exitFacing);
        
        
        SceneManager.LoadScene(mapToLoad);

    }
}
