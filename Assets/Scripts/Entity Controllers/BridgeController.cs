using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : DoodadData
{
    // public bool tempBridge;
    public bool invisibleBridge;
    public bool primBridge;
    public bool primShortcutBridge;
    public int primConnectionNumber;
    public BobPrim primToCheck;

    new void Start()
    {
        base.Start();
        primToCheck = GameObject.Find("Grid").GetComponent<BobPrim>();
        if (primBridge) {
            RunPrimAlgorythm(primToCheck);
        }
        if (this.isPlatformTerrain == false) {
            HidePlatform();
        }


    }

    public void RunPrimAlgorythm(BobPrim checkedPrim) {
        if (checkedPrim.result[primConnectionNumber])
        {
            AddPlatform();            
        }
        else
        {
            HidePlatform();
        }
    }
    public void RemovePlatform() {
        this.isPlatformTerrain = false;
        
        
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        MapGrid.GetComponent<DoodadGrid>().grid[DoodadLocation.x, DoodadLocation.y] = null;

    }

    public void HidePlatform() {
        this.isPlatformTerrain = false;
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    public void AddPlatform() {
        this.isPlatformTerrain = true;
        if (invisibleBridge) return;
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        MapGrid.GetComponent<DoodadGrid>().grid[DoodadLocation.x, DoodadLocation.y] = this.gameObject;
    }
   
}
