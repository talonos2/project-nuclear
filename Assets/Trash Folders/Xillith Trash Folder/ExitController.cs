﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : DoodadData
{
    // Start is called before the first frame update


    public String mapToLoad;
    public int nextMapLevel;
    public Vector2Int exitPosition;
    public SpriteMovement.DirectionMoved exitFacing;


    public void TransitionMap()
    {
        GameData gameData= GameObject.Find("GameStateData").GetComponent<GameData>();
        gameData.FloorNumber = nextMapLevel;

            gameData.SetNextLocation(exitPosition, exitFacing);
        
        SceneManager.LoadScene(mapToLoad);

    }
}
