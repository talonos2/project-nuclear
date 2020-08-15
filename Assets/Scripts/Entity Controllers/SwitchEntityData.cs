using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEntityData : EntityData
{
    private Vector2Int SwitchLocation;
    private GameObject MapGrid;
    //private Vector2 mapZeroLocation;
    public GameObject[] TiedEntities;
    public float[] times;
    public SpriteMovement.DirectionMoved[] particlePath;
    public SwitchTrailMover mover;
    protected bool isAnimating;
    public float AnimationSpeed=6;
    private float timeSinceLastFrame = 0;
    private int frameNumber = 0;
    public int totalFrames = 5;
    //private Renderer sRender;
    private bool forwardAnimation;
    private int animationCounter=0;
    protected bool activeSwitch = true;
    public float timeTillReset = 0;
    public bool prePressed;
    private float tempResetTime = 0;
    private bool timerSet = false;
    private float offsetFix = .00001f;
    // Start is called before the first frame update
    protected void Start()
    {
        InitializeSpriteLocation();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        if (prePressed) {
               ToggleTiedObjects();
        }
    }


    private void InitializeSpriteLocation()
    {
        MapGrid = GameObject.Find("Grid");
        if (isOnCutsceneMap) MapGrid = GameObject.Find("Grid2");
        mapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        SwitchLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        SwitchLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[SwitchLocation.x, SwitchLocation.y] = this.gameObject;

    }

    override public void ProcessClick(CharacterStats stats) {

        if (isAnimating) { return; }
        if (timerSet == true) {
            tempResetTime = timeTillReset;
            return; }
        if (timeTillReset > 0) {
            tempResetTime = timeTillReset;
            timerSet = true;
            SoundManager.Instance.PlaySound("Environment/TimeStart", 1f);
        }

        SoundManager.Instance.PlaySound("switchSound", 1f);

        ToggleTiedObjects();
        
    }

    public virtual void ToggleTiedObjects()
    {
        bool movedABridge = false;
        if (activeSwitch)
        {
            for (int x = 0; x < TiedEntities.Length; x++)
            {
                GameObject tiedEntity = TiedEntities[x];
                float time = (times.Length > x ? times[x]:0);
                SpikeController spikeControlled = tiedEntity.GetComponent<SpikeController>();
                BridgeController bridgeControlled = tiedEntity.GetComponent<BridgeController>();
                WindJumpController windJumpControlled = tiedEntity.GetComponent<WindJumpController>();
                if (spikeControlled != null)
                {
                    if (prePressed) { spikeControlled.Open(false);
                        prePressed = false;
                    }
                    else spikeControlled.OpenAfterTime(time);
                }
                if (bridgeControlled != null)
                {
                    movedABridge = true;
                    if (bridgeControlled.isPlatformTerrain) { bridgeControlled.HidePlatform(); }
                    else { bridgeControlled.AddPlatform(); }
                }
                if (windJumpControlled != null)
                {
                    windJumpControlled.EnableWindJumper();

                }


            }
            activeSwitch = false;
            SwitchAnimation();
            SwitchTrailMover trail = GameObject.Instantiate<SwitchTrailMover>(mover);
            trail.gameObject.transform.position = new Vector3(Mathf.RoundToInt(sRender.transform.position.x*2f)/2f, Mathf.RoundToInt(sRender.transform.position.y * 2f) / 2f,-.001f); ;
            trail.InitStart();
            trail.path = particlePath;
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
                    movedABridge = true;
                    if (bridgeControlled.isPlatformTerrain) { bridgeControlled.HidePlatform(); }
                    else { bridgeControlled.AddPlatform(); }
                }


            }
            activeSwitch = true;
            SwitchReverseAnimation();

        }
        if (movedABridge)
        {
            SoundManager.Instance.PlaySound("Environment/Bridge", 1);
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

        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }

        if (timerSet) {
            tempResetTime -= Time.deltaTime;
            if (tempResetTime <= 0) {
                ToggleTiedObjects();
                timerSet = false;
                SoundManager.Instance.PlaySound("Environment/TimeStop", 1);
            }
        }

        //Animate the switch section
        if (!isAnimating) { return; }
        timeSinceLastFrame += Time.deltaTime;

        if (timeSinceLastFrame >= 1 / AnimationSpeed) {
            if (forwardAnimation) frameNumber += 1;
            else frameNumber -= 1;
            animationCounter += 1;
            sRender.material.SetFloat("_Frame", frameNumber+ offsetFix);
            timeSinceLastFrame = 0;
            if (animationCounter == totalFrames - 1)
            {
                isAnimating = false;
                animationCounter = 0;
            }
        }
    }

}