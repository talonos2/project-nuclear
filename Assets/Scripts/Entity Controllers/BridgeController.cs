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
    private BobPrim primToCheck;

    new void Start()
    {
        base.Start();
        primToCheck = GameObject.Find("Grid").GetComponent<BobPrim>();
        if (primBridge) {
            RunPrimAlgorythm();
        }
        if (this.isPlatformTerrain == false) {
            RemovePlatform();
        }


    }

    public void RunPrimAlgorythm() {
        if (primToCheck.result[primConnectionNumber])
        {
            AddPlatform();            
        }
        else
        {
            RemovePlatform();
        }
    }
    public void RemovePlatform() {
        this.isPlatformTerrain = false;
        
        
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    public void AddPlatform() {
        this.isPlatformTerrain = true;
        if (invisibleBridge) return;
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
