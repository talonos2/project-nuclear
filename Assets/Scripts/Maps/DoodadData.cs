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

    protected Vector2Int DoodadLocation;
    protected GameObject MapGrid;
    protected Vector2 MapZeroLocation;

    public void Start() {
        InitializeDoodadNewMap();
    }

    protected void InitializeDoodadNewMap()
    {
        MapGrid = GameObject.Find("Grid"); ;
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        DoodadLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        DoodadLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<DoodadGrid>().grid[DoodadLocation.x, DoodadLocation.y] = this.gameObject;
    }

    public Vector2Int getLocation()
    {
        return DoodadLocation;
    }
}
