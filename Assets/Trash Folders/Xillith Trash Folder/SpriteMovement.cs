﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpriteMovement : MonoBehaviour
{
    // Start is called before the first frame update

   // public Sprite MovementSprite;
    //public Sprite[] MovementSprites;
    public float MoveSpeed = 6;
    public float AnimationSpeed = 10;
    protected GameObject MapGrid;
    protected Vector2 MapZeroLocation;
    protected bool CurrentlyMoving = false;
    protected GameObject ThePlayer;

    //private SpriteRenderer spriteR;
    private string spriteNames;

    private float MovedSoFar = 0;
    private float AnimationStep = 0;

    protected Vector2Int CharacterLocation;
    protected Vector2Int CharacterNextLocation;
    protected int FacedDirection = (int)DirectionMoved.LEFT;
    protected Renderer sRender;
    protected Vector2Int HomeLocation;

    public enum DirectionMoved
    { NONE, UP, RIGHT, DOWN, LEFT }

    public void Start()
    {
        //spriteR = gameObject.GetComponent<SpriteRenderer>();
        InitializeNewMap();
        InitializeSpriteLocation();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");

    }

    private void InitializeSpriteLocation()
    {
        CharacterLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        CharacterLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[CharacterLocation.x, CharacterLocation.y] = this.gameObject;
        HomeLocation.x = CharacterLocation.x;
        HomeLocation.y = CharacterLocation.y;
    }

    protected void SetCurrentLocation() {
        //Vector3 roundLocation = new Vector3((int)Math.Round(this.transform.position.x), (int)Math.Round(this.transform.position.y),0);
        //this.transform.position = roundLocation;
        CharacterLocation.x = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        CharacterLocation.y = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        //Debug.Log("" + CharacterLocation.x + " "+CharacterLocation.y);
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

    protected bool IsMoveLocationPassable(int LocX, int LocY) {

        bool MoveableLocation = false;


        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL  || IsPlatformUp(LocX, LocY)) {
            if (IsLocationDoodadFree(LocX, LocY) && IsLocationEntityFree(LocX, LocY))
                MoveableLocation = true;
        }
           
        
        return MoveableLocation;
    }

    protected bool IsPlayerMoveLocationPassable(int LocX, int LocY)
    {

        bool MoveableLocation = false;

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL
            || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || IsPlatformUp(LocX, LocY))
        {
            if (IsLocationDoodadFree(LocX, LocY) && IsLocationEntityFree(LocX, LocY))
                MoveableLocation = true;
        }
        return MoveableLocation;
    }

    protected bool IsMoveLocationMonsterChaseable(int LocX, int LocY) {
        bool MoveableLocation = false;


        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL
            || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || IsPlatformUp(LocX, LocY))
        {
            if (IsLocationDoodadFree(LocX, LocY) && IsLocationEntityFree(LocX, LocY))
                MoveableLocation = true;
        }


        return MoveableLocation;
    }

    private bool IsPlatformUp(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        GameObject doddadObject = MapGrid.GetComponent<DoodadGrid>().grid[LocX, LocY];

        if (doddadObject != null)
        {
            if (doddadObject.GetComponent<DoodadData>().isPlatformTerrain)
                MoveableLocation = true;
        }
            return MoveableLocation;
    }

    protected bool IsRandomMoveLocationPassable(int LocX, int LocY) {

        bool MoveableLocation = false;

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER) {
            if (IsLocationDoodadFree(LocX, LocY) && IsLocationEntityFree(LocX, LocY))
                MoveableLocation = true;
        }
           
        //Toggleable Terrain check
       

        return MoveableLocation;

    }

    protected bool IsLocationDoodadFree(int LocX, int LocY) {
        bool MoveableLocation = true;

        GameObject DoddadObject = MapGrid.GetComponent<DoodadGrid>().grid[LocX, LocY];
        if (DoddadObject != null)
        {
            if (DoddadObject.GetComponent<DoodadData>().isBlockableTerrain)
                MoveableLocation = false;
            if (DoddadObject.GetComponent<DoodadData>().isBackgroundCharacter)
                MoveableLocation = false;
        }
        return MoveableLocation;

    }

    protected bool IsLocationEntityFree(int LocX, int LocY) {
        bool MoveableLocation = false;

        if (MapGrid.GetComponent<EntityGrid>().grid[LocX, LocY] == null)
            MoveableLocation = true;
       

        return MoveableLocation;
    }

    protected GameObject isThereAMonster() {
        GameObject EnemyPresent = null;
        GameObject EntityToFight = MapGrid.GetComponent<EntityGrid>().grid[CharacterNextLocation.x, CharacterNextLocation.y]; 

        if (EntityToFight != null) {
            if (EntityToFight.GetComponent<EntityData>().isAMonster) {
                EnemyPresent = EntityToFight;
            }
        }
        return EnemyPresent;
    }

    protected GameObject isThereAPlayer(int CharLocX, int CharLocY)
    {
        GameObject EnemyPresent = null;
        GameObject EntityToFight = MapGrid.GetComponent<EntityGrid>().grid[CharLocX, CharLocY]; ;

        if (EntityToFight != null)
        {
            if (EntityToFight.GetComponent<EntityData>().isMainCharacter)
            {
                EnemyPresent = EntityToFight;
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
        sRender.material.SetInt("_Frame", 0);
        AnimationStep = 0;
    }

    private void AnimateMoveDown()
    {

        AnimationStep += 1;
        if (AnimationStep<=(1* AnimationSpeed)) { sRender.material.SetInt("_Frame", 1); }
        else if (AnimationStep == (2 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 2); }
        else if (AnimationStep == (3 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 3); }
        else if (AnimationStep == (4 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 4); }
        else if (AnimationStep == (5 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 5); }
        else if (AnimationStep == (6 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 6); AnimationStep = 0; }
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
        sRender.material.SetInt("_Frame", 21);
        AnimationStep = 0;
    }

    
    private void AnimateMoveUp()
    {
        AnimationStep += 1; 
        if (AnimationStep <= (1 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 22); }
        else if (AnimationStep == (2 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 23); }
        else if (AnimationStep == (3 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 24); }
        else if (AnimationStep == (4 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 25); }
        else if (AnimationStep == (5 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 26); }
        else if (AnimationStep == (6 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 27); ; AnimationStep = 0; }
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
        sRender.material.SetInt("_Frame", 7);
        AnimationStep = 0;
    }

    private void AnimateMoveLeft()
    {

        AnimationStep += 1;
        if (AnimationStep <= (1 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 8); }
        else if (AnimationStep == (2 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 9); }
        else if (AnimationStep == (3 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 10); }
        else if (AnimationStep == (4 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 11); }
        else if (AnimationStep == (5 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 12); }
        else if (AnimationStep == (6 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 13); AnimationStep = 0; }
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
        sRender.material.SetInt("_Frame", 14);
        AnimationStep = 0;
    }

    private void AnimateMoveRight()
    {
        
        AnimationStep += 1;
        if (AnimationStep <= (1 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 15); }
        else if (AnimationStep == (2 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 16); }
        else if (AnimationStep == (3 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 17); }
        else if (AnimationStep == (4 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 18); }
        else if (AnimationStep == (5 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 19); }
        else if (AnimationStep == (6 * AnimationSpeed)) { sRender.material.SetInt("_Frame", 20); AnimationStep = 0; }
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

    protected int GetChaseStep()
    {
        int TempX = ThePlayer.GetComponent<CharacterMovement>().CharacterLocation.x - CharacterLocation.x;
        int TempY = ThePlayer.GetComponent<CharacterMovement>().CharacterLocation.y - CharacterLocation.y;
        if (TempX == 0 && TempY == 0)
        {
            return (int)DirectionMoved.NONE;
        }
        //Pick an axis based on a random number with the abs value distances as it's input
        int AxisChosen = Random.Range(0, (Math.Abs(TempX) + Math.Abs(TempY))) + 1;
        if (Math.Abs(TempX) - AxisChosen >= 0)
        {
            //go x direction
            if (TempX < 0) { return (int)DirectionMoved.LEFT; }
            else { return (int)DirectionMoved.RIGHT; }
        }
        else
        {
            //go y Direction
            if (TempY < 0) { return (int)DirectionMoved.DOWN; }
            else { return (int)DirectionMoved.UP; }
        }
    }

}
