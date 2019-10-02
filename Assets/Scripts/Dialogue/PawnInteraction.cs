using Naninovel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInteraction : EntityData
{

    private Vector2Int pawnLocation;
    private GameObject mapGrid;
    private Vector2 mapZeroLocation;
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
        mapGrid = GameObject.Find("Grid");
        mapZeroLocation = mapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        pawnLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        pawnLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
        mapGrid.GetComponent<EntityGrid>().grid[pawnLocation.x, pawnLocation.y] = this.gameObject;

    }

    public override void ProcessClick(CharacterStats stats)
    {
        if (GameState.isInBattle == false)
        {
            GameState.isInBattle = true;
            RuntimeInitializer.InitializeAsync();
            Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(scriptName);
        }
    }

    void Update()
    {

    }

}