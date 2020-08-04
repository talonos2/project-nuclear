using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodadData : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isPlatformTerrain = false;
    public bool isWindShifter = false;
    public bool isTeleporter = false;
    public bool isExit = false;
    public bool isBlockableTerrain = false;
    public bool isTallBlockableTerrain = false;
    public bool isBackgroundCharacter = false;
    public bool isSpike = false;
    public bool isPassable = false;
    public bool isOnCutsceneMap;

    protected Vector2Int DoodadLocation;
    protected GameObject MapGrid;
    protected Vector2 MapZeroLocation;
    private bool setup;

    public void Start() {
        if (!setup) InitializeDoodadNewMap();
    }

    protected void InitializeDoodadNewMap()
    {
        MapGrid = GameObject.Find("Grid"); 
        if (isOnCutsceneMap) MapGrid = GameObject.Find("Grid2");
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        DoodadLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        DoodadLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<DoodadGrid>().grid[DoodadLocation.x, DoodadLocation.y] = this.gameObject;
        setup = true;
    }
    public void removeDoodadFromMap() {
        if (!setup) {
            MapGrid = GameObject.Find("Grid"); ;
            MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
            DoodadLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
            DoodadLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
            setup = true;
        }
        
        MapGrid.GetComponent<DoodadGrid>().grid[DoodadLocation.x, DoodadLocation.y] = null;
    }

    public Vector2Int getLocation()
    {
        return DoodadLocation;
    }
}
