using Naninovel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpriteMovement;

public class PawnInteraction : EntityData
{

    private Vector2Int pawnLocation;
    private GameObject mapGrid;
    private Vector2 mapZeroLocation;
    private Renderer sRender;

    public String scriptName;
    private bool waitFrameAfterDialogue;

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

        if (GameState.fullPause == true || GameData.Instance.isInDialogue) return;




        if (GameState.isInBattle == false)
        {
            GameData.Instance.isInDialogue = true;
            //GameState.isInBattle = true;
            DirectionMoved playerFace = stats.GetComponentInParent<SpriteMovement>().facedDirection;
            if (playerFace == DirectionMoved.RIGHT)
            {
                this.GetComponentInParent<SpriteMovement>().FaceLeft();
            }
            if (playerFace == DirectionMoved.LEFT)
            {
                this.GetComponentInParent<SpriteMovement>().FaceRight();
            }
            if (playerFace == DirectionMoved.UP)
            {
                this.GetComponentInParent<SpriteMovement>().FaceDown();
            }
            if (playerFace == DirectionMoved.DOWN)
            {
                this.GetComponentInParent<SpriteMovement>().FaceUp();
            }

            //movment.facedDirection
            RuntimeInitializer.InitializeAsync();
           
            Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(scriptName);

        }
    }

    void Update()
    {

    }

}