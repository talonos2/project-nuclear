using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEntityData : EntityData
{

    private Vector2Int SwitchLocation;
    private GameObject MapGrid;
    private Vector2 MapZeroLocation;
    public GameObject[] TiedEntities;
    private bool isAnimating;
    public float AnimationSpeed=6;
    private float timeSinceLastFrame = 0;
    private int frameNumber = 0;
    private int totalFrames = 5;
    private Renderer sRender;
    private bool forwardAnimation;
    private int animationCounter=0;
    protected bool activeSwitch = true;
    public float timeTillReset = 0;
    public bool prePressed;
    private float tempResetTime = 0;
    private bool timerSet = false;
    // Start is called before the first frame update
    protected void Start()
    {
        InitializeSpriteLocation();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        if (prePressed) {
            this.ProcessClick(null);
        }
    }


    private void InitializeSpriteLocation()
    {
        MapGrid = GameObject.Find("Grid");
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        SwitchLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        SwitchLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[SwitchLocation.x, SwitchLocation.y] = this.gameObject;

    }

    override public void ProcessClick(CharacterStats stats) {

        if (isAnimating) { return; }
        if (stats == null)
        {
            timerSet = false;
        }
        else { timerSet = true;
            tempResetTime = timeTillReset;
        }
        ToggleTiedObjects();
        
    }

    private void ToggleTiedObjects()
    {
        if (activeSwitch)
        {
            foreach (GameObject tiedEntity in TiedEntities)
            {
                SpikeController spikeControlled = tiedEntity.GetComponent<SpikeController>();
                BridgeController bridgeControlled = tiedEntity.GetComponent<BridgeController>();
                if (spikeControlled != null)
                {
                    spikeControlled.isPassable = true;
                    spikeControlled.LowerSpikeAnimation();
                }
                if (bridgeControlled != null)
                {
                    if (bridgeControlled.isPlatformTerrain) { bridgeControlled.removePlatform(); }
                    else { bridgeControlled.addPlatform(); }
                }


            }
            activeSwitch = false;
            SwitchAnimation();
        }
        else
        {
            foreach (GameObject tiedEntity in TiedEntities)
            {
                SpikeController spikeControlled = tiedEntity.GetComponent<SpikeController>();
                BridgeController bridgeControlled = tiedEntity.GetComponent<BridgeController>();
                if (spikeControlled != null)
                {
                    spikeControlled.isPassable = false;
                    spikeControlled.RaiseSpikeAnimation();
                }
                if (bridgeControlled != null)
                {
                    if (bridgeControlled.isPlatformTerrain) { bridgeControlled.removePlatform(); }
                    else { bridgeControlled.addPlatform(); }
                }


            }
            activeSwitch = true;
            SwitchReverseAnimation();

        }
    }

    protected void SwitchAnimation()
    {
        isAnimating = true;
        forwardAnimation = true;
        frameNumber = 0;
    }
    private void SwitchReverseAnimation()
    {
        isAnimating = true;
        forwardAnimation = false;
        frameNumber = totalFrames-1;
    }

    void Update()
    {

        if (GameState.isInBattle == true)
        {
            return;
        }

        if (timerSet && timeTillReset>0) {
            tempResetTime -= Time.deltaTime;
            if (tempResetTime <= 0) {
                ProcessClick(null);
            }
        }

        //Animate the switch section
        if (!isAnimating) { return; }
        timeSinceLastFrame += Time.deltaTime;

        if (timeSinceLastFrame >= 1 / AnimationSpeed) {
            if (forwardAnimation) frameNumber += 1;
            else frameNumber -= 1;
            animationCounter += 1;
            sRender.material.SetInt("_Frame", frameNumber);
            timeSinceLastFrame = 0;
            if (animationCounter == totalFrames - 1)
            {
                isAnimating = false;
                animationCounter = 0;
            }
        }
    }

}