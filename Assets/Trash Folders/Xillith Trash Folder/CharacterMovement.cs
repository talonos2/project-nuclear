using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : SpriteMovement
{

    /// <summary>
    /// This is the character's start position. The character's transform won't matter; this will overwrite it.
    /// </summary>
    public Vector2 startPositionOnMap = new Vector2(0,0);
    // Start is called before the first frame update



    
    private int MovedDirection = (int)DirectionMoved.LEFT;
    
    private bool CurrentlyMoving = false;
    private int CharacterLocationX;
    private int CharacterLocationY;

    private void Start()
    {
        base.Start();
        Vector2 transformToMoveTo = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(startPositionOnMap);
        this.transform.position = new Vector3(transformToMoveTo.x, transformToMoveTo.y, -.1f);
    }

    // Update is called once per frame
    void Update()
    {

        float finishedMoving = 0;

        if (CurrentlyMoving)
        {
            if (MovedDirection == (int)DirectionMoved.UP)
            {
                finishedMoving = MoveUp(MoveSpeed);
                if (finishedMoving == 0)
                    CurrentlyMoving = false;
            }
            if (MovedDirection == (int)DirectionMoved.DOWN)
            {
                finishedMoving = MoveDown(MoveSpeed);
                if (finishedMoving == 0)
                    CurrentlyMoving = false;
            }
            if (MovedDirection == (int)DirectionMoved.LEFT)
            {
                finishedMoving = MoveLeft(MoveSpeed);
                if (finishedMoving == 0)
                    CurrentlyMoving = false;
            }
            if (MovedDirection == (int)DirectionMoved.RIGHT)
            {
                finishedMoving = MoveRight(MoveSpeed);
                if (finishedMoving == 0)
                    CurrentlyMoving = false;
            }
            if (CurrentlyMoving == false)
                return;

        }


        Vector2 zero = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        //Debug.Log(" Location x "+ xZeroLocation+" y "+ yZeroLocation);

        float xCharLocation = (int)Math.Round(this.transform.position.x) - zero.x;
        float yCharLocation= (int)Math.Round(this.transform.position.y) - zero.y;



        if (!CurrentlyMoving)
        {
            //Debug.Log("Character Location x " + xCharLocation + " y " + yCharLocation+ " blocked? = "+ this.MapGrid.GetComponent<PassabilityGrid>().grid[(int)xCharLocation, (int)yCharLocation]);
            CharacterLocationX = (int)xCharLocation;
            CharacterLocationY = (int)yCharLocation;
        }
        

        float directionChangeHorizontal = 0;
        float directionChangeVertical = 0;
        


        //
        if (!CurrentlyMoving && (MovedDirection == (int)DirectionMoved.RIGHT || MovedDirection == (int)DirectionMoved.LEFT))
        {
            //Get next input if character isn't currently moving

            if (Input.GetAxisRaw("Horizontal") > .1)
            {
                directionChangeHorizontal = 1;
                MovedDirection = (int)DirectionMoved.RIGHT;
                if (this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocationX+1, CharacterLocationY]== PassabilityType.NORMAL)
                    CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Horizontal") < -.1)
            {
                directionChangeHorizontal = -1;
                MovedDirection = (int)DirectionMoved.LEFT;
                if (this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocationX - 1, CharacterLocationY] == PassabilityType.NORMAL)
                    CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Vertical") > .1)
            {
                directionChangeVertical = 1;
                MovedDirection = (int)DirectionMoved.UP;
                if (this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocationX, CharacterLocationY+1] == PassabilityType.NORMAL)
                    CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Vertical") < -.1)
            {
                directionChangeVertical = -1;
                MovedDirection = (int)DirectionMoved.DOWN;
                if (this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocationX, CharacterLocationY-1] == PassabilityType.NORMAL)
                    CurrentlyMoving = true;
            }

        }

        if (!CurrentlyMoving)
        {
            //Get next input if character isn't currently moving          
            if (Input.GetAxisRaw("Vertical") > .1)
            {
                directionChangeVertical = 1;
                MovedDirection = (int)DirectionMoved.UP;
                if (this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocationX, CharacterLocationY + 1] == PassabilityType.NORMAL)
                    CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Vertical") < -.1)
            {
                directionChangeVertical = -1;
                MovedDirection = (int)DirectionMoved.DOWN;
                if(this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocationX, CharacterLocationY - 1] == PassabilityType.NORMAL)
                    CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Horizontal") > .1)
            {
                directionChangeHorizontal = 1;
                MovedDirection = (int)DirectionMoved.RIGHT;
                if (this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocationX + 1, CharacterLocationY] == PassabilityType.NORMAL)
                    CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Horizontal") < -.1)
            {
                directionChangeHorizontal = -1;
                MovedDirection = (int)DirectionMoved.LEFT;
                if (this.MapGrid.GetComponent<PassabilityGrid>().grid[CharacterLocationX - 1, CharacterLocationY] == PassabilityType.NORMAL)
                    CurrentlyMoving = true;
            }

        }


        if (!CurrentlyMoving)
        {
            //replace sprite to be the sprite number of holding still based on direction last moved, 
            //Assuming that you haven't moved at all recently.
            if (MovedDirection == (int)DirectionMoved.DOWN)
                FaceDown();
            if (MovedDirection == (int)DirectionMoved.UP)
                FaceUp();
            if (MovedDirection == (int)DirectionMoved.LEFT)
                FaceLeft();
            if (MovedDirection == (int)DirectionMoved.RIGHT)
                FaceRight();
        }

       



    }
}
