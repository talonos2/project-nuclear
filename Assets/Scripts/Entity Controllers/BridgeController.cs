using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : DoodadData
{
    public bool tempBridge;
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
        if (tempBridge) {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            return;
        }
        
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    public void addPlatform() {
        this.isPlatformTerrain = true;
        if (tempBridge)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true ;
            return;
        }
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
