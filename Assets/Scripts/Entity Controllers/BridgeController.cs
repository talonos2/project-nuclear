using System;
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

    private List<float> swapTimes = new List<float>();

    public bool playsSoundOnPlatformAdd = false;

    CharacterMovement thePlayer;

    new void Start()
    {
        base.Start();
        primToCheck = GameObject.Find("Grid").GetComponent<BobPrim>();
        if (primBridge) {
            RunPrimAlgorythm(primToCheck);
        }
        if (this.isPlatformTerrain == false) {
            //HidePlatform();
            RemovePlatform();
        }
        thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    protected void Update()
    {
        List<float> newSwapTimes = new List<float>();
        foreach (float f in swapTimes)
        {
            float newf = f-Time.deltaTime;
            if (newf <= 0)
            {
                SwapPlatform();
            }
            else
            {
                newSwapTimes.Add(newf);
            }
        }
        swapTimes = newSwapTimes;
    }

    public void RunPrimAlgorythm(BobPrim checkedPrim) {
        if (primToCheck == null) {
            primToCheck=GameObject.Find("Grid").GetComponent<BobPrim>();
            if (isOnCutsceneMap) primToCheck=GameObject.Find("Grid2").GetComponent<BobPrim>();
            checkedPrim = primToCheck;
        }
        if (checkedPrim.result[primConnectionNumber])
        {
            SwapPlatform();            
        }
        else
        {
            RemovePlatform();
        }
    }
    public void RemovePlatform() {
        this.isPlatformTerrain = false;
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        MapGrid.GetComponent<DoodadGrid>().grid[DoodadLocation.x, DoodadLocation.y] = null;

    }

    public void SwapPlatform() {
        this.isPlatformTerrain = !this.isPlatformTerrain;
        if (playsSoundOnPlatformAdd)
        {
            SoundManager.Instance.PlaySound("Environment/Bridge", 1);
        }
        if (this.isPlatformTerrain == true)
        {
            MapGrid.GetComponent<DoodadGrid>().grid[DoodadLocation.x, DoodadLocation.y] = this.gameObject;
        }
        if (this.isPlatformTerrain == false) {
            if (thePlayer != null) {
                if (thePlayer.characterLocation.x == DoodadLocation.x && thePlayer.characterLocation.y == DoodadLocation.y)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().BumpOffBridge();
                }
            }
           
            
        }
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = !invisibleBridge&& this.isPlatformTerrain == true;
    }

    internal void SwapPlatformAfterTime(float time)
    {
        if (time == 0)
        {
            SwapPlatform();
        }
        else
        {
            swapTimes.Add(time);
        }
    }
}
