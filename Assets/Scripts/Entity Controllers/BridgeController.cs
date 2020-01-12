using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : DoodadData
{
   // public bool tempBridge;
    public bool primBridge;
    public bool primShortcutBridge;
    public int primConnectionNumber;
    private BobPrim primToCheck;

    new void Start()
    {
        base.Start();
        primToCheck = GameObject.Find("Grid").GetComponent<BobPrim>();
        if (primBridge) {
            RunPrimAlgorythm();
        }
        if (this.isPlatformTerrain == false) {
            removePlatform();
        }


    }

    public void RunPrimAlgorythm() {
        if (primToCheck.result[primConnectionNumber])
        {
            addPlatform();
        }
        else
        {
            removePlatform();
        }
    }
    public void removePlatform() {
        this.isPlatformTerrain = false;
        
        
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    public void addPlatform() {
        this.isPlatformTerrain = true;
       
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
