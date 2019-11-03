using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : DoodadData
{
    public bool tempBridge;
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
