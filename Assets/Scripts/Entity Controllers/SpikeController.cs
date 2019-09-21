﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : EntityData
{

    private Vector2Int SpikeLocation;
    private GameObject MapGrid;
    private Vector2 MapZeroLocation;
    protected bool isAnimating;
    public float AnimationSpeed = 6;
    protected float timeSinceLastFrame = 0;
    protected int frameNumber = 0;
    protected Renderer sRender;
    protected GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSpriteLocation();
        gameData = GameObject.Find("GameStateData").GetComponent<GameData>();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
    }


    private void InitializeSpriteLocation()
    {
        MapGrid = GameObject.Find("Grid");
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        SpikeLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        SpikeLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[SpikeLocation.x, SpikeLocation.y] = this.gameObject;

    }

    public void LowerSpikeAnimation() {
        isAnimating = true;
    }

    public void RaiseSpikeAnimation()
    {
        isAnimating = true;
    }

}