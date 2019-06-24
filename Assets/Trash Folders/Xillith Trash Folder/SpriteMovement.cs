﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMovement : MonoBehaviour
{
    // Start is called before the first frame update

   // public Sprite MovementSprite;
    public Sprite[] MovementSprites;
    public float MoveSpeed = 6;
    public float AnimationSpeed = 10;
    protected GameObject MapGrid;
    protected Vector2 MapZeroLocation;

    private SpriteRenderer spriteR;
    private string spriteNames;

    private float MovedSoFar = 0;
    private float AnimationStep = 0;

    protected int CharacterLocationX;
    protected int CharacterLocationY;

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
        CharacterLocationX = (int)Math.Round(this.transform.position.x) - (int)MapZeroLocation.x;
        CharacterLocationY = (int)Math.Round(this.transform.position.y) - (int)MapZeroLocation.y;
        Debug.Log("Where am I " + CharacterLocationX + " " + CharacterLocationY);
        Debug.Log("Hmm "+this.gameObject);
        MapGrid.GetComponent<EntityGrid>().grid[CharacterLocationX, CharacterLocationY]= this.gameObject;
    }

    protected void UpdateEntityGridLocation(int xNewLoc, int yNewLoc) {
        MapGrid.GetComponent<EntityGrid>().grid[CharacterLocationX, CharacterLocationY] = this.gameObject;
    }

    public void InitializeNewMap() {
        MapGrid = GetMapGrid();
        MapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
    }

    public GameObject GetMapGrid() {
        GameObject MapGridRetrieved = GameObject.Find("Grid");
            return MapGridRetrieved;
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

}
