using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : DoodadData
{

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
