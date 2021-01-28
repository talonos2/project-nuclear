using Naninovel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpriteMovement;

public class PawnInteraction : EntityData
{

    private Vector2Int pawnLocation;
    //private GameObject mapGrid;
   // private Vector2 mapZeroLocation;
    //private Renderer sRender;
    //private EntityGrid mapEntityGrid;
    protected float FLOATING_POINT_FIX = .00001f;

    public String scriptName;
    public int pawnNum;
    private bool waitFrameAfterDialogue;
    private DirectionMoved clickedDirection;
    public bool clickForwarding;
    public bool punchBag;
    private bool punching;
    private float punchTimer;
    public List<int> daysToPunchBag;
    internal bool punchAnyway;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSpriteLocation();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
    }

    internal void SetPunchAnyway()
    {
        punchAnyway = true;
    }

    private void InitializeSpriteLocation()
    {
        mapGrid = GameObject.Find("Grid");
        mapZeroLocation = mapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        pawnLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        pawnLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
        mapEntityGrid=mapGrid.GetComponent<EntityGrid>();
        mapEntityGrid.grid[pawnLocation.x, pawnLocation.y] = this.gameObject;

    }

    public override void ProcessClick(CharacterStats stats)
    {

        if (GameState.getFullPauseStatus() == true || GameData.Instance.isInDialogue || GameState.isInBattle == true || GameData.Instance.inPauseMenu) return;

        //bool punch

        if (clickForwarding)
        {
            PassClickOn( stats  );
        }
        else 
        {
            GameData.Instance.isInDialogue = true;
            //GameState.isInBattle = true;
            if (stats.GetComponentInParent<SpriteMovement>().facedDirection == SpriteMovement.DirectionMoved.DOWN)
            {
                this.GetComponentInParent<SpriteMovement>().SetLookDirection(3);
            }
            if (stats.GetComponentInParent<SpriteMovement>().facedDirection == SpriteMovement.DirectionMoved.UP)
            {
                this.GetComponentInParent<SpriteMovement>().SetLookDirection(0);
            }
            if (stats.GetComponentInParent<SpriteMovement>().facedDirection == SpriteMovement.DirectionMoved.RIGHT)
            {
                this.GetComponentInParent<SpriteMovement>().SetLookDirection(1);
            }
            if (stats.GetComponentInParent<SpriteMovement>().facedDirection == SpriteMovement.DirectionMoved.LEFT)
            {
                this.GetComponentInParent<SpriteMovement>().SetLookDirection(2);
            }

            Vector2Int whoWhen = new Vector2Int(pawnNum, GameData.Instance.RunNumber);

            if (pawnNum != 0 && !GameData.Instance.dialoguesSeen.Contains(whoWhen))
            {
                GameData.Instance.dialoguesSeen.Add(whoWhen);
                FinalWinterAchievementManager.Instance.SetStatAndGiveAchievement(FWStatAchievement.READ_ALL_DIALOGUE, GameData.Instance.dialoguesSeen.Count);
            }

            RuntimeInitializer.InitializeAsync();
            Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(scriptName);

        }

    }

    private void PassClickOn(CharacterStats stats)
    {        
        GameObject entityToCheck = null;
        DirectionMoved facedDirection= stats.GetComponentInParent<SpriteMovement>().facedDirection;
        switch (facedDirection)
        {
            case DirectionMoved.UP:
                entityToCheck = mapEntityGrid.grid[pawnLocation.x, pawnLocation.y + 1];
                break;
            case DirectionMoved.DOWN:
                entityToCheck = mapEntityGrid.grid[pawnLocation.x, pawnLocation.y - 1];
                break;
            case DirectionMoved.LEFT:
                entityToCheck = mapEntityGrid.grid[pawnLocation.x - 1, pawnLocation.y];
                break;
            case DirectionMoved.RIGHT:
                entityToCheck = mapEntityGrid.grid[pawnLocation.x + 1, pawnLocation.y];
                break;
        }
        if (entityToCheck != null)
        {
            entityToCheck.GetComponent<EntityData>().ProcessClick(stats);
        }
    }

    public void clickDummy()
    {
        GameObject entityToCheck = null;
        entityToCheck = mapEntityGrid.grid[pawnLocation.x, pawnLocation.y + 1];
      
        if (entityToCheck != null)
        {
            entityToCheck.GetComponent<EntityData>().ProcessClick(null);
        }
    }

    private bool isItTrainingDay()
    {
        if (!punchBag) return false;

        foreach (int trainingDay in daysToPunchBag) {
            if (trainingDay == GameData.Instance.RunNumber) { return true; }
        }

        return false;
    }

    void Update() {
        if (GameState.getFullPauseStatus() == true  || GameState.isInBattle == true || GameData.Instance.isInDialogue) return;
        if (GameData.Instance.isCutscene && !punchAnyway) return;//So training dummies can punch during cutscenes

        if (isItTrainingDay() || punchAnyway) {
            punchTimer += Time.deltaTime;
            if (punchTimer >= 1f && punchTimer < 2.2f && !punching)
            {
                punching = true;
                clickDummy();
                sRender.material.SetFloat("_Frame", FLOATING_POINT_FIX + 22);
            }
            if (punchTimer >= 1.7f)
            {
                punching = false;
                punchTimer = 0;
                sRender.material.SetFloat("_Frame", FLOATING_POINT_FIX + 21);
                punchAnyway = false;
            }

        }
       

    }


}