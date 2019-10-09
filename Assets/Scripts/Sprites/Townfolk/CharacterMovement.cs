using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : SpriteMovement
{

    public CharacterStats playerStats;
    private Vector2Int jumpTarget;
    private float windJumpSpeed=0;
    private bool windJump;
    // Update is called once per frame
    void Update()
    {


        if (GameState.isInBattle == true)
        {
            return;
        }

        if (!currentlyMoving && windJump) {
            bool finishedMoving = JumpToTarget(windJumpSpeed, jumpTarget);
            if (finishedMoving)
            {
                currentlyMoving = false;
                windJump = false;
                tiePositionToGrid();
                tempFramesPerSecond = framesPerSecond;
                CheckWindJumpStatus();
            }
        }
    

        if (!currentlyMoving && (jumping|| jumpQued)) {


            if (jumpQued &&!jumping) {
                jumping = ActivateJump();
                jumpQued = false;
            }

            if (jumping) { 
                float finishedMoving = ContinueJumping();
                if (finishedMoving == 0)
                {
                    currentlyMoving = false;
                    jumping = false;
                    tempFramesPerSecond = framesPerSecond;
                    //   SetCurrentLocation();
                    tiePositionToGrid();
                    CheckExitStatus();
                    CheckWindJumpStatus();
                }
            }
        }

        if (!currentlyMoving && dashing) {

        }


        //If in the process of moving, keep moving and do nothing else
        if (currentlyMoving)
        {
            float finishedMoving = ContinueMoving();
            if (finishedMoving == 0)
            {
                currentlyMoving = false;
                tiePositionToGrid();
                //  SetCurrentLocation();
                CheckWindJumpStatus();
                CheckExitStatus();
            }
        }



    }


    private void CheckWindJumpStatus()
    {
        GameObject windJumpLocation = MapGrid.GetComponent<DoodadGrid>().grid[characterLocation.x, characterLocation.y];
        if (windJumpLocation != null)
        {
            if (windJumpLocation.GetComponent<DoodadData>().isWindShifter)
            {
                if (playerStats == null)
                {
                    playerStats = this.GetComponent<CharacterStats>();
                }
                jumpTarget = windJumpLocation.GetComponent<WindJumpController>().jumpDestOffset;
                
                SetNextLocationActual(characterLocation.x+jumpTarget.x, characterLocation.y +jumpTarget.y);
                windJumpSpeed= windJumpLocation.GetComponent<WindJumpController>().jumpSpeedMultiplier;
                windJump = true;
                currentlyMoving = false;
                
            }
        }
    }

    //Key command received from CharacterInputController script
    public void MoveKeyReceived(DirectionMoved inputDirection) {

        if (GameState.isInBattle == true)
        {
            return;
        }

        if (!currentlyMoving && !jumping && !dashing && !jumpQued&&!windJump)
        {
            if (inputDirection == (int)DirectionMoved.NONE)
            {
                SetLookDirection();
                return;
            }

            facedDirection = inputDirection;
            SetNextLocation(inputDirection);
            if (IsPlayerMoveLocationPassable(characterNextLocation.x, characterNextLocation.y))
            {
                //if it is possible, check for a monster attack
                //Needs to be refractored a bit
                UpdateNewEntityGridLocation();
                RemoveOldEntityGridLocation();
                characterLocation = characterNextLocation;
                currentlyMoving = true;

            }

            //Check if a monster is in the next location, and initiate combat if so
            GameObject EnemyToFight = isThereAMonster();
            if (EnemyToFight != null)
            {
                Combat.InitiateFight(this.gameObject, EnemyToFight);
            }
        }
    }

    internal void PowerActivateKeyReceived()
    {
        if (GameState.isInBattle == true)
        {
            return;
        }
        if (playerStats == null)
        {
            playerStats = this.GetComponent<CharacterStats>();
        }
        if (!jumping && !dashing )
        {
            switch (playerStats.currentPower)
            {
                case (int)ElementalPower.ICE:
                    ActivateShieldDash();
                    break;
                case (int)ElementalPower.EARTH:
                    ActivateInvisibility();
                    break;
                case (int)ElementalPower.FIRE:
                    ActivateHaste();
                    break;
                case (int)ElementalPower.AIR:
                    jumpQued = true;
                    //ActivateJump();
                    break;
                default:
                    return;
            }
        }
    }

    private bool ActivateJump()
    {
        bool jumpingTemp = true; ;
        if (playerStats.mana < 10) {
            jumpingTemp = false;
            return jumpingTemp;
        }
            switch (facedDirection)
            {
                case DirectionMoved.UP:
                if (isLocationJumpOverable(characterLocation.x, characterLocation.y + 1) && IsPlayerMoveLocationPassable(characterLocation.x, characterLocation.y + 2))
                {

                    SetNextLocationActual(characterLocation.x, characterLocation.y + 2);
                  //  jumping = true;
                    tempFramesPerSecond *= jumpSpeed;
                }
                else jumpingTemp = false;
                //  entityToCheck = mapEntityGrid.grid[characterLocation.x, characterLocation.y + 1];
                break;
                case DirectionMoved.DOWN:
                if (isLocationJumpOverable(characterLocation.x, characterLocation.y - 1) && IsPlayerMoveLocationPassable(characterLocation.x, characterLocation.y - 2))
                {

                    SetNextLocationActual(characterLocation.x, characterLocation.y - 2);
                    //jumping = true;
                    tempFramesPerSecond *= jumpSpeed;
                }
                else jumpingTemp = false;
                // entityToCheck = mapEntityGrid.grid[characterLocation.x, characterLocation.y - 1];
                break;
                case DirectionMoved.LEFT:
                    if (isLocationJumpOverable(characterLocation.x - 1, characterLocation.y) && IsPlayerMoveLocationPassable(characterLocation.x - 2, characterLocation.y)) {
                        
                        SetNextLocationActual(characterLocation.x - 2, characterLocation.y);                        
                       // jumping = true;
                    tempFramesPerSecond *= jumpSpeed;
                    }
                else jumpingTemp = false;
                break;
                case DirectionMoved.RIGHT:
                if (isLocationJumpOverable(characterLocation.x + 1, characterLocation.y) && IsPlayerMoveLocationPassable(characterLocation.x + 2, characterLocation.y))
                {

                    SetNextLocationActual(characterLocation.x + 2, characterLocation.y);
                   // jumping = true;
                    tempFramesPerSecond *= jumpSpeed;
                }
                else jumpingTemp = false;
                //  entityToCheck = mapEntityGrid.grid[characterLocation.x + 1, characterLocation.y];
                break;

            }
        if (jumpingTemp == true) { playerStats.mana -= 10; }
        return jumpingTemp;
    }

    private bool isLocationJumpOverable(int characterLocationx, int characterLocationy)
    {
        bool jumpable = true;
        if (MapGrid.GetComponent<PassabilityGrid>().grid[characterLocationx, characterLocationy] == PassabilityType.WALL)
        {
            jumpable = false;
        }
        GameObject entityInLocation = MapGrid.GetComponent<EntityGrid>().grid[characterLocationx, characterLocationy];
        if (entityInLocation != null )
        {
            if (entityInLocation.GetComponent<EntityData>().isItem) {
                jumpable = false;
            }
            if (entityInLocation.GetComponent<EntityData>().isLargeMonster) {
                jumpable = false;
            }            
        }
        GameObject doodadObject= MapGrid.GetComponent<DoodadGrid>().grid[characterLocationx, characterLocationy];
        if (doodadObject != null) {
            if (doodadObject.GetComponent<DoodadData>().isTallBlockableTerrain) {
                jumpable = false;
            }
        }

        return jumpable;
    }

    private void ActivateHaste()
    {
        throw new NotImplementedException();
    }

    private void ActivateInvisibility()
    {
        throw new NotImplementedException();
    }

    private void ActivateShieldDash()
    {
        throw new NotImplementedException();
    }

    internal void PowerToggleLeftKeyReceived()
    {
        if (playerStats == null)
        {
            playerStats = this.GetComponent<CharacterStats>();
        }
        if (playerStats.powersGained == 0) return;
        playerStats.currentPower -= 1;
        if (playerStats.currentPower < 0) {
            playerStats.currentPower = playerStats.powersGained;
        }

    }


    internal void PowerToggleRightKeyReceived()
    {
        if (playerStats == null)
        {
            playerStats = this.GetComponent<CharacterStats>();
        }
        if (playerStats.powersGained == 0) return;
        playerStats.currentPower += 1;
        if (playerStats.currentPower > playerStats.powersGained)
        {
            playerStats.currentPower = 0;
        }

    }

    public void ActivateKeyReceived() {
        if (GameState.isInBattle == true)
        {
            return;
        }
        GameObject entityToCheck = null;
        if (!currentlyMoving && !jumping &&!dashing) {
            switch (facedDirection) {
                case DirectionMoved.UP:
                    entityToCheck = mapEntityGrid.grid[characterLocation.x, characterLocation.y+1];
                    break;
                case DirectionMoved.DOWN:
                    entityToCheck = mapEntityGrid.grid[characterLocation.x, characterLocation.y - 1];
                    break;
                case DirectionMoved.LEFT:
                    entityToCheck = mapEntityGrid.grid[characterLocation.x-1, characterLocation.y];
                    break;
                case DirectionMoved.RIGHT:
                    entityToCheck = mapEntityGrid.grid[characterLocation.x+1, characterLocation.y];
                    break;

            }
            if (entityToCheck != null)
            {
                entityToCheck.GetComponent<EntityData>().ProcessClick(this.GetComponent<CharacterStats>());
            }
        }
    }

    private void CheckExitStatus()
    {
        GameObject exitLocation = MapGrid.GetComponent<DoodadGrid>().grid[characterLocation.x, characterLocation.y];
        if (exitLocation != null)
        {
            if (exitLocation.GetComponent<DoodadData>().isExit) {
                if (playerStats == null)
                {
                    playerStats = this.GetComponent<CharacterStats>();
                }
                playerStats.PushCharacterData();
                exitLocation.GetComponent<ExitController>().TransitionMap(); 
            }
        }

    }


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
        if (facedDirection == DirectionMoved.UP)
        {
            finishedMoving = MoveUp(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(MoveSpeed);
        }
        return finishedMoving;
    }

    private float ContinueJumping()
    {
        float finishedMoving = 0;
        if (facedDirection == DirectionMoved.UP)
        {
            finishedMoving = JumpUp(MoveSpeed * jumpSpeed);
        }
        if (facedDirection == DirectionMoved.DOWN)
        {
            finishedMoving = JumpDown(MoveSpeed * jumpSpeed);
        }
        if (facedDirection == DirectionMoved.LEFT)
        {
            finishedMoving = JumpLeft(MoveSpeed * jumpSpeed);
        }
        if (facedDirection == DirectionMoved.RIGHT)
        {
            finishedMoving = JumpRight(MoveSpeed * jumpSpeed);
        }
        return finishedMoving;
    }
}
