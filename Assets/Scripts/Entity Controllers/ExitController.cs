﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : DoodadData
{
    // Start is called before the first frame update


    public string mapToLoad;
    public Vector2Int exitPosition;
    public bool townMap;
    public bool dungeonEntrance;
    public SpriteMovement.DirectionMoved exitFacing;


    public void TransitionMap()
    {
        //Entering the dungeon's first level spawns a special cutscene, and does not count towards leaving.
        if (GameData.Instance.FloorNumber==0 && dungeonEntrance) {

            GameState.fullPause = false;
            CutsceneLoader.LoadCutscene();
            return;
        }


        GameData gameData= GameData.Instance;

        //Keep track of your run time.
        gameData.timesThisRun[gameData.FloorNumber - 1] = gameData.timer;
        float timeTakenThisFloor = gameData.timer;
        if (gameData.FloorNumber > 1)
        {
            timeTakenThisFloor -= gameData.timesThisRun[gameData.FloorNumber - 2];
        }
        if (gameData.bestTimes[gameData.FloorNumber - 1]==0|| gameData.bestTimes[gameData.FloorNumber - 1 ]>timeTakenThisFloor)
        {
            gameData.bestTimes[gameData.FloorNumber - 1] = timeTakenThisFloor;
        }

        gameData.FloorNumber +=1;
        if (townMap) {
            gameData.FloorNumber = 0;
        }

        gameData.SetNextLocation(exitPosition, exitFacing);

        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        fadeout.InitNext(mapToLoad);
        GameObject.Destroy(this);
    }
}
