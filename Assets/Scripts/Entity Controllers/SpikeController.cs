using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : DoodadData
{

    private Vector2Int SpikeLocation;
    protected bool isAnimating;
    public float AnimationSpeed = 6;
    protected float timeSinceLastFrame = 0;
    protected int frameNumber = 0;
    protected Renderer sRender;
    protected GameData gameData;
    protected bool animateRise = false;
    protected int frameToSet = 0;

    private float timeUntilOpen = 0;
    private float timeUntilClose = 0;

    public bool primBoulder;
    //public bool primShortcutB;
    public int primConnectionNumber;
    private BobPrim primToCheck;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        gameData = GameData.Instance;
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);

        primToCheck = GameObject.Find("Grid").GetComponent<BobPrim>();
        if (isOnCutsceneMap) primToCheck = GameObject.Find("Grid2").GetComponent<BobPrim>();
        if (primBoulder)
        {
            RunPrimAlgorythm();
        }
    }

    public void RunPrimAlgorythm()
    {
        if (primToCheck.result[primConnectionNumber])
        {
            Open(false) ;
        }
    }

    protected void Update()
    {
        if (timeUntilOpen != 0)
        {
            timeUntilOpen -= Time.deltaTime;
            if (timeUntilOpen <= 0)
            {
                timeUntilOpen = 0;
                Open();
            }
        }
        if (timeUntilClose != 0)
        {
            timeUntilClose -= Time.deltaTime;
            if (timeUntilClose <= 0)
            {
                timeUntilClose = 0;
                Close();
            }
        }
    }

    public void Open(bool sound = true)
    {
        this.isPassable = true;
        LowerSpikeAnimation(sound);
    }

    public void Close(bool sound = true)
    {
        this.isPassable = false;
        RaiseSpikeAnimation(sound);
    }


    private void InitializeSpriteLocation()
    {
        MapGrid = GameObject.Find("Grid");
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        SpikeLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        SpikeLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[SpikeLocation.x, SpikeLocation.y] = this.gameObject;

    }

    internal void OpenAfterTime(float time)
    {
        if (time == 0)
        {
            Open();
        }
        else
        {
            timeUntilOpen = time;
        }
    }

    internal void CloseAfterTime(float time)
    {
        if (time == 0)
        {
            Close();
        }
        else
        {
            timeUntilClose = time;
        }
    }

    private void LowerSpikeAnimation(bool sound = true) {
        isAnimating = true;
        animateRise = false;
        if (sound) SoundManager.Instance.PlaySound("DroppingSpikes", 1);
    }

    private void RaiseSpikeAnimation(bool sound = true)
    {
        isAnimating = true;
        animateRise = true;
        if (sound) SoundManager.Instance.PlaySound("RaisingSpikes", 1);
    }

}