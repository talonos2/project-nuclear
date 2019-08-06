using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : SpriteMovement
{

    /// <summary>
    /// This is the character's start position. The character's transform won't matter; this will overwrite it.
    /// </summary>
    public Vector2 startPositionOnMap = new Vector2(0,0);

    // Start is called before the first frame update

    
   


    //private void Start()
   // {
   //     base.Start();
   //     Vector2 transformToMoveTo = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(startPositionOnMap);
   //     this.transform.position = new Vector3(transformToMoveTo.x, transformToMoveTo.y, -.1f);
   // }

    // Update is called once per frame
    void Update()
    {

      
        

        //If in the process of moving, keep moving and do nothing else

        if (CurrentlyMoving)
        {
            float finishedMoving = ContinueMoving();
            if (finishedMoving == 0)
            {
                CurrentlyMoving = false;
                SetCurrentLocation();
                CheckExitStatus();
            }
        }



    }

    //Key command received from CharacterInputController script
    public void MoveKeyReceived(int inputDirection) {

        if (!CurrentlyMoving)
        {
            if (inputDirection == (int)DirectionMoved.NONE)
            {
                SetLookDirection();
                return;
            }

            FacedDirection = inputDirection;
            SetNextLocation(inputDirection);
            if (IsPlayerMoveLocationPassable(CharacterNextLocation.x, CharacterNextLocation.y))
            {
                //if it is possible, check for a monster attack
                //Needs to be refractored a bit
                UpdateNewEntityGridLocation();
                RemoveOldEntityGridLocation();
                CurrentlyMoving = true;

            }

            //Check if a monster is in the next location, and initiate combat if so
            GameObject EnemyToFight = isThereAMonster();
            if (EnemyToFight != null)
            {
                Combat.initiateFight(this.gameObject, EnemyToFight);
            }
        }
    }

    private void CheckExitStatus()
    {
        GameObject exitLocation = MapGrid.GetComponent<DoodadGrid>().grid[CharacterLocation.x, CharacterLocation.y];
        if (exitLocation != null)
        {
            if (exitLocation.GetComponent<DoodadData>().isExit) {
                SceneManager.LoadScene("Map1-1");
            }
        }

    }

    /*private bool RunIntoMonster(int inputDirection)
    {
        if (inputDirection == (int)DirectionMoved.NONE)
            return false;

        if (inputDirection == (int)DirectionMoved.RIGHT
            && this.MapGrid.GetComponent<EntityGrid>().IsThereAMonster(CharacterLocation.x + 1, CharacterLocation.y))
            return true;

        if (inputDirection == (int)DirectionMoved.LEFT
            && this.MapGrid.GetComponent<EntityGrid>().IsThereAMonster(CharacterLocation.x - 1, CharacterLocation.y))          
            return true;

        if (inputDirection == (int)DirectionMoved.UP
            && this.MapGrid.GetComponent<EntityGrid>().IsThereAMonster(CharacterLocation.x, CharacterLocation.y + 1))
            return true;

        if (inputDirection == (int)DirectionMoved.DOWN
            && this.MapGrid.GetComponent<EntityGrid>().IsThereAMonster(CharacterLocation.x, CharacterLocation.y - 1))            
            return true;

        return false;
    }*/





    private int GetInputDirection()
    {

        int NextInputDirection=-1;

            if (Input.GetAxisRaw("Horizontal") > .1)
            {
                NextInputDirection = (int)DirectionMoved.RIGHT;             
            }
            if (Input.GetAxisRaw("Horizontal") < -.1)
            {
                NextInputDirection = (int)DirectionMoved.LEFT;
            }
            if (Input.GetAxisRaw("Vertical") > .1)
            {
                NextInputDirection = (int)DirectionMoved.UP;
            }
            if (Input.GetAxisRaw("Vertical") < -.1)
            {
                NextInputDirection = (int)DirectionMoved.DOWN;
            }

        if (NextInputDirection == -1) {
            NextInputDirection = (int)DirectionMoved.NONE;
        }
        return NextInputDirection;
    }

    private float ContinueMoving()
    {
        float finishedMoving=0;
        if (FacedDirection == (int)DirectionMoved.UP)
        {
            finishedMoving = MoveUp(MoveSpeed);
        }
        if (FacedDirection == (int)DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(MoveSpeed);
        }
        if (FacedDirection == (int)DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(MoveSpeed);
        }
        if (FacedDirection == (int)DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(MoveSpeed);
        }
        return finishedMoving;
    }
}
