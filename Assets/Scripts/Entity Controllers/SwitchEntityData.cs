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

    private int lastTickNumber = 0;

    private static readonly float OFFSET_FIX = .00001f;
    private static readonly float TIME_PER_TICK_SOUND = .7f;

    public bool playParticlesOnSwitchUndo;
    public bool timerSwitch;
    public bool map1_3Switch;



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
        if (timerSet == true)
        {
            while (tempResetTime + TIME_PER_TICK_SOUND < timeTillReset)
            {
                tempResetTime += TIME_PER_TICK_SOUND;
            }
            lastTickNumber = (int)(tempResetTime / TIME_PER_TICK_SOUND);
            return;
        }
        if (timeTillReset > 0)
        {
            tempResetTime = timeTillReset;
            timerSet = true;
            SoundManager.Instance.PlaySound("Environment/TimeStart", 1f);
            lastTickNumber = 0;
        }

        SoundManager.Instance.PlaySound("switchSound", 1f);

        ToggleTiedObjects();
        
    }

    public virtual void ToggleTiedObjects()
    {
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
                    bridgeControlled.SwapPlatformAfterTime(time);
                }
                if (windJumpControlled != null)
                {
                    windJumpControlled.EnableWindJumper();

                }


            }
            activeSwitch = false;
            SwitchAnimation();
            SwitchTrailMover trail;
            if(!map1_3Switch) trail = GameObject.Instantiate<SwitchTrailMover>(mover);
             else trail=Instantiate (mover, new Vector3 (this.transform.position.x, transform.position.y -.2f, this.transform.position.z),Quaternion.identity);
            trail.gameObject.transform.position = new Vector3(Mathf.RoundToInt(sRender.transform.position.x*2f)/2f, Mathf.RoundToInt(sRender.transform.position.y * 2f) / 2f,-.001f); ;
            trail.InitStart();
            trail.path = particlePath;
        }
        else
        {
            for (int x = 0; x < TiedEntities.Length; x++)
            {
                GameObject tiedEntity = TiedEntities[x];
                float time = 0;
                if (playParticlesOnSwitchUndo)
                {
                    time = (times.Length > x ? times[x] : 0);
                }
                SpikeController spikeControlled = tiedEntity.GetComponent<SpikeController>();
                BridgeController bridgeControlled = tiedEntity.GetComponent<BridgeController>();
                if (spikeControlled != null)
                {
                    spikeControlled.CloseAfterTime(time);
                }
                if (bridgeControlled != null)
                {
                    bridgeControlled.SwapPlatformAfterTime(time);
                }


            }
            activeSwitch = true;
            SwitchReverseAnimation();

            if (playParticlesOnSwitchUndo)
            {
                SwitchTrailMover trail = GameObject.Instantiate<SwitchTrailMover>(mover);
                trail.gameObject.transform.position = new Vector3(Mathf.RoundToInt(sRender.transform.position.x * 2f) / 2f, Mathf.RoundToInt(sRender.transform.position.y * 2f) / 2f, -.001f); ;
                trail.InitStart();
                trail.path = particlePath;
            }
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
      if (!timerSwitch)  frameNumber = totalFrames-1;
    }

    void Update()
    {

        if (GameState.isInBattle && !GameState.pickingItem)
        {
            return;
        }
        if (GameState.fullPause) {
            return;
        }
        

        if (timerSet) {
            tempResetTime -= Time.deltaTime;
            int currentTickNumber = (int)(tempResetTime / TIME_PER_TICK_SOUND);
            if (currentTickNumber != lastTickNumber)
            {
                if (lastTickNumber != 0) SoundManager.Instance.PlaySound("Environment/TickTock", 1);
                lastTickNumber = currentTickNumber;
            }
            if (tempResetTime <= 0) {
                ToggleTiedObjects();
                timerSet = false;
                SoundManager.Instance.PlaySound("Environment/TimeStop", 1);
            }
        }

        //Animate the switch section
        if (!isAnimating) { return; }
        timeSinceLastFrame += Time.deltaTime;

        if (timerSwitch)
        {
            if (timeSinceLastFrame >= 1 / AnimationSpeed)
            {
                frameNumber += 1;
                if (frameNumber == totalFrames - 1)
                {
                    frameNumber = 0;
                }
                if (!timerSet && frameNumber == 0) {
                    isAnimating = false;
                }
                sRender.material.SetFloat("_Frame", frameNumber + OFFSET_FIX);
                timeSinceLastFrame = 0;
            }
        }
        else {
            if (timeSinceLastFrame >= 1 / AnimationSpeed)
            {
                if (forwardAnimation) frameNumber += 1;
                else frameNumber -= 1;
                animationCounter += 1;
                sRender.material.SetFloat("_Frame", frameNumber + OFFSET_FIX);
                timeSinceLastFrame = 0;
                if (animationCounter == totalFrames - 1)
                {
                    isAnimating = false;
                    animationCounter = 0;
                }
            }

        }

        
    }

}