﻿using Naninovel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInteraction : EntityData
{

    private Vector2Int SwitchLocation;
    private GameObject MapGrid;
    private Vector2 MapZeroLocation;
    public float AnimationSpeed = 6;
    private Renderer sRender;

    public String scriptName;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSpriteLocation();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
    }


    private void InitializeSpriteLocation()
    {
        MapGrid = GameObject.Find("Grid");
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        SwitchLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        SwitchLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[SwitchLocation.x, SwitchLocation.y] = this.gameObject;

    }

    public override void ProcessClick(CharacterStats stats)
    {
        Debug.Log("Here in interact");
        RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(scriptName);
    }

    void Update()
    {

    }

}