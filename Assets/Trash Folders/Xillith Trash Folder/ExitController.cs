using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    // Start is called before the first frame update

    Vector2Int ExitLocation;
    private GameObject MapGrid;
    private Vector2 MapZeroLocation;
    public String mapToLoad;
    public int nextMapLevel;
    public Vector2Int exitPosition;
    public SpriteMovement.DirectionMoved exitFacing;
    void Start()
    {
        InitializeNewMap();        
    }

    private void InitializeNewMap()
    {
        MapGrid = GameObject.Find("Grid"); ;
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        ExitLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        ExitLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<DoodadGrid>().grid[ExitLocation.x, ExitLocation.y] = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransitionMap()
    {
        GameData gameData= GameObject.Find("GameStateData").GetComponent<GameData>();
        gameData.FloorNumber = nextMapLevel;

            gameData.SetNextLocation(exitPosition, exitFacing);
        
        SceneManager.LoadScene(mapToLoad);

    }
}
