﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteMovement : MonoBehaviour
{
    // Start is called before the first frame update

   // public Sprite MovementSprite;
    public Sprite[] MovementSprites;
    public float MoveSpeed = 6;
    public float AnimationSpeed = 10;
    protected GameObject MapGrid;
    protected Vector2 MapZeroLocation;
    protected bool CurrentlyMoving = false;

    private SpriteRenderer spriteR;
    private string spriteNames;

    private float MovedSoFar = 0;
    private float AnimationStep = 0;

    protected Vector2Int CharacterLocation;
    protected Vector2Int CharacterNextLocation;
    protected int FacedDirection = (int)DirectionMoved.LEFT;
    //protected int CharacterLocationX;
    //protected int CharacterLocationY;

    public enum DirectionMoved
    { UP, DOWN, LEFT, RIGHT, NONE }

    public void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        InitializeNewMap();
        InitializeSpriteLocation();
    }

    private void InitializeSpriteLocation()
    {
        CharacterLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        CharacterLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[CharacterLocation.x, CharacterLocation.y] = this.gameObject;
    }

    protected void SetCurrentLocation() {
        CharacterLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        CharacterLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
    }

    protected float MoveToNextSquare()
    {

        float finishedMoving = 0;
        if (FacedDirection == (int)DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(MoveSpeed);
        }
        if (FacedDirection == (int)DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(MoveSpeed);
        }
        if (FacedDirection == (int)DirectionMoved.UP)
        {
            finishedMoving = MoveUp(MoveSpeed);
        }
        if (FacedDirection == (int)DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(MoveSpeed);
        }
        return finishedMoving;
    }

    protected void SetNextLocation(int nextStep) {

        //Debug.Log("next step loc x " + CharacterLocation.x +" y "+ CharacterLocation.y);

       
        if (nextStep == (int)DirectionMoved.UP) {
            CharacterNextLocation.x = CharacterLocation.x;
            CharacterNextLocation.y = CharacterLocation.y + 1;
        }
        if (nextStep == (int)DirectionMoved.DOWN)
        {
            CharacterNextLocation.x = CharacterLocation.x;
            CharacterNextLocation.y = CharacterLocation.y - 1;

        }
        if (nextStep == (int)DirectionMoved.LEFT)
        {
            CharacterNextLocation.x = CharacterLocation.x-1;
            CharacterNextLocation.y = CharacterLocation.y;
        }
        if (nextStep == (int)DirectionMoved.RIGHT)
        {
            CharacterNextLocation.x = CharacterLocation.x + 1;
            CharacterNextLocation.y = CharacterLocation.y;

        }


    }

    protected bool IsMoveLocationPassable() {

        bool MoveableLocation = false;

        if(MapGrid.GetComponent<PassabilityGrid>().grid[CharacterNextLocation.x , CharacterNextLocation.y] == PassabilityType.NORMAL)
            MoveableLocation= true;
        //Toggleable Terrain check
        GameObject DoddadObject = MapGrid.GetComponent<DoodadGrid>().grid[CharacterLocation.x, CharacterLocation.y];
        if (DoddadObject != null){
            if (DoddadObject.GetComponent<EntityData>().isBlockableTerrain)
                MoveableLocation = false;
            if (DoddadObject.GetComponent<EntityData>().isPlatformTerrain)
                MoveableLocation = true;
            if (DoddadObject.GetComponent<EntityData>().isBackgroundCharacter)
                MoveableLocation = false;
        }



        return MoveableLocation;
    }

    protected bool IsLocationEntityFree() {
        bool MoveableLocation = false;

        if (MapGrid.GetComponent<EntityGrid>().grid[CharacterNextLocation.x, CharacterNextLocation.y] == null)
            MoveableLocation = true;

        return MoveableLocation;
    }

    protected bool isThereAMonster() {
        bool EnemyPresent = false;
        GameObject EntityToFight = MapGrid.GetComponent<EntityGrid>().grid[CharacterNextLocation.x, CharacterNextLocation.y]; ;

        if (EntityToFight != null) {
            if (EntityToFight.GetComponent<EntityData>().isAMonster) {
                EnemyPresent = true;
            }
        }
        return EnemyPresent;
    }

    protected bool isThereAPlayer()
    {
        bool EnemyPresent = false;
        GameObject EntityToFight = MapGrid.GetComponent<EntityGrid>().grid[CharacterNextLocation.x, CharacterNextLocation.y]; ;

        if (EntityToFight != null)
        {
            if (EntityToFight.GetComponent<EntityData>().isMainCharacter)
            {
                EnemyPresent = true;
            }
        }
        return EnemyPresent;
    }

    protected void RemoveOldEntityGridLocation() {

        MapGrid.GetComponent<EntityGrid>().grid[CharacterLocation.x, CharacterLocation.y] = null;
    }
    protected void UpdateNewEntityGridLocation() {

        MapGrid.GetComponent<EntityGrid>().grid[CharacterNextLocation.x, CharacterNextLocation.y] = this.gameObject;
    }

    public void InitializeNewMap() {
        MapGrid = GetMapGrid();
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
    }

    public GameObject GetMapGrid() {
        GameObject MapGridRetrieved = GameObject.Find("Grid");
            return MapGridRetrieved;
    }

    protected bool ValidMoveLocation(int inputDirection)
    {
        if (inputDirection == (int)DirectionMoved.NONE)
            return false;

        if (inputDirection == (int)DirectionMoved.RIGHT
            && this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocation.x + 1, CharacterLocation.y] == PassabilityType.NORMAL)
            return true;

        if (inputDirection == (int)DirectionMoved.LEFT
            && this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocation.x - 1, CharacterLocation.y] == PassabilityType.NORMAL)
            return true;

        if (inputDirection == (int)DirectionMoved.UP
            && this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocation.x, CharacterLocation.y + 1] == PassabilityType.NORMAL)
            return true;

        if (inputDirection == (int)DirectionMoved.DOWN
            && this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocation.x, CharacterLocation.y - 1] == PassabilityType.NORMAL)
            return true;

        return false;
    }

    // Update is called once per frame



    void Update()
    {
       


    }


    public float MoveDown(float MoveSpeed){

        float DistanceToMove = Time.deltaTime * MoveSpeed;
        MovedSoFar += DistanceToMove;

        AnimateMoveDown();


        if (MovedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (MovedSoFar - 1);
            MovedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(0.0f, -DistanceToMove, 0.0f);
        transform.position = transform.position + getMotion;
        //transform.position.y = (float)(int)Math.Round(transform.position.y);
        return MovedSoFar;
    }

    public void FaceDown()
    {
        spriteR.sprite = MovementSprites[0];
        AnimationStep = 0;
    }

    private void AnimateMoveDown()
    {

        AnimationStep += 1;
        if (AnimationStep<=(1* AnimationSpeed)) { spriteR.sprite = MovementSprites[1]; }
        else if (AnimationStep == (2 * AnimationSpeed)) { spriteR.sprite = MovementSprites[2]; }
        else if (AnimationStep == (3 * AnimationSpeed)) { spriteR.sprite = MovementSprites[3]; }
        else if (AnimationStep == (4 * AnimationSpeed)) { spriteR.sprite = MovementSprites[4]; }
        else if (AnimationStep == (5 * AnimationSpeed)) { spriteR.sprite = MovementSprites[5]; }
        else if (AnimationStep == (6 * AnimationSpeed)) { spriteR.sprite = MovementSprites[6]; AnimationStep = 0; }
    }

    public float MoveUp(float MoveSpeed)
    {
    
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * MoveSpeed;
        MovedSoFar += DistanceToMove;

        AnimateMoveUp();

        if (MovedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (MovedSoFar - 1);
            MovedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(0.0f, DistanceToMove, 0.0f);
        transform.position = transform.position + getMotion;
        return MovedSoFar;

    }

    public void FaceUp()
    {
        spriteR.sprite = MovementSprites[21];
        AnimationStep = 0;
    }

    private void AnimateMoveUp()
    {

        AnimationStep += 1;
        if (AnimationStep <= (1 * AnimationSpeed)) { spriteR.sprite = MovementSprites[22]; }
        else if (AnimationStep == (2 * AnimationSpeed)) { spriteR.sprite = MovementSprites[23]; }
        else if (AnimationStep == (3 * AnimationSpeed)) { spriteR.sprite = MovementSprites[24]; }
        else if (AnimationStep == (4 * AnimationSpeed)) { spriteR.sprite = MovementSprites[25]; }
        else if (AnimationStep == (5 * AnimationSpeed)) { spriteR.sprite = MovementSprites[26]; }
        else if (AnimationStep == (6 * AnimationSpeed)) { spriteR.sprite = MovementSprites[27]; AnimationStep = 0; }
    }
    public float MoveLeft(float MoveSpeed)
    {
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * MoveSpeed;
        MovedSoFar += DistanceToMove;

        AnimateMoveLeft();
        if (MovedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (MovedSoFar - 1);
            MovedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(-DistanceToMove, 0.0f,  0.0f);
        transform.position = transform.position + getMotion;

        return MovedSoFar;
    }

    public void FaceLeft()
    {
        spriteR.sprite = MovementSprites[7];
        AnimationStep = 0;
    }

    private void AnimateMoveLeft()
    {

        AnimationStep += 1;
        if (AnimationStep <= (1 * AnimationSpeed)) { spriteR.sprite = MovementSprites[8]; }
        else if (AnimationStep == (2 * AnimationSpeed)) { spriteR.sprite = MovementSprites[9]; }
        else if (AnimationStep == (3 * AnimationSpeed)) { spriteR.sprite = MovementSprites[10]; }
        else if (AnimationStep == (4 * AnimationSpeed)) { spriteR.sprite = MovementSprites[11]; }
        else if (AnimationStep == (5 * AnimationSpeed)) { spriteR.sprite = MovementSprites[12]; }
        else if (AnimationStep == (6 * AnimationSpeed)) { spriteR.sprite = MovementSprites[13]; AnimationStep = 0; }
    }
    public float MoveRight(float MoveSpeed)
    {
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * MoveSpeed;
        MovedSoFar += DistanceToMove;
        AnimateMoveRight();
        if (MovedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (MovedSoFar - 1);
            MovedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(DistanceToMove, 0.0f, 0.0f);
        transform.position = transform.position + getMotion;

        return MovedSoFar;
    }

    public void FaceRight()
    {
        spriteR.sprite = MovementSprites[14];
        AnimationStep = 0;
    }

    private void AnimateMoveRight()
    {

        AnimationStep += 1;
        if (AnimationStep <= (1 * AnimationSpeed)) { spriteR.sprite = MovementSprites[15]; }
        else if (AnimationStep == (2 * AnimationSpeed)) { spriteR.sprite = MovementSprites[16]; }
        else if (AnimationStep == (3 * AnimationSpeed)) { spriteR.sprite = MovementSprites[17]; }
        else if (AnimationStep == (4 * AnimationSpeed)) { spriteR.sprite = MovementSprites[18]; }
        else if (AnimationStep == (5 * AnimationSpeed)) { spriteR.sprite = MovementSprites[19]; }
        else if (AnimationStep == (6 * AnimationSpeed)) { spriteR.sprite = MovementSprites[20]; AnimationStep = 0; }
    }

    protected void SetLookDirection()
    {
        if (FacedDirection == (int)DirectionMoved.DOWN)
            FaceDown();
        if (FacedDirection == (int)DirectionMoved.UP)
            FaceUp();
        if (FacedDirection == (int)DirectionMoved.LEFT)
            FaceLeft();
        if (FacedDirection == (int)DirectionMoved.RIGHT)
            FaceRight();
    }

}
