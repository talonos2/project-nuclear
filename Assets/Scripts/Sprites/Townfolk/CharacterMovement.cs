using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : SpriteMovement
{
    private static float DASH_LENGTH = 27;

    public CharacterStats playerStats;
    private Vector2Int jumpTarget;
    private float windJumpSpeed=0;
    public static bool windJump;
    private float hasteSpeed = 2;
    private float stealthspeed = .75f;
    private float dashSpeed = 4;
    private float totalDashed = 0;
    private bool continueDashing = false;
    private Transform jumpPivot;




    new void Start()
    {
        base.Start();
        playerStats = this.GetComponent<CharacterStats>();
        jumpPivot = sRender.transform.parent;

    }
    // Update is called once per frame
    void Update()
    {


        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }
        if (!currentlyMoving && waitTimer >= 0)
        {
            if (gameData.dashing)
            {
                SetShieldGraphic(waitTimer, true);
            }
            waitTimer -= Time.deltaTime;
            return;
        }


        if (gameData.hasted) {
            if (gameData.timerTrigger) {
                if (playerStats.mana >= 12)
                {
                    playerStats.mana -= 12;
                    tempMovementSpeed = MoveSpeed * hasteSpeed;
                    tempFramesPerSecond = framesPerSecond * hasteSpeed;
                }
                else {
                    gameData.hasted = false;
                    tempMovementSpeed = MoveSpeed;
                    tempFramesPerSecond = framesPerSecond;
                }
            }
        }
        if (gameData.stealthed) {

            if (gameData.timerTrigger)
            {
                if (playerStats.mana >= 4)
                {
                    playerStats.mana -= 4;
                    tempMovementSpeed = MoveSpeed * stealthspeed;
                    tempFramesPerSecond = framesPerSecond * stealthspeed;
                }
                else
                {
                    gameData.stealthed = false;
                    tempMovementSpeed = MoveSpeed;
                    tempFramesPerSecond = framesPerSecond;
                }
            }
        }

        if (!currentlyMoving && windJump) {
            bool finishedMoving = JumpToTarget(windJumpSpeed, jumpTarget);
            if (finishedMoving)
            {
                currentlyMoving = false;
                windJump = false;
                TiePositionToGrid();
                tempFramesPerSecond = framesPerSecond;
                CheckWindJumpStatus();
            }
        }
    

        if (!currentlyMoving && (jumping|| jumpQued))
        {
            if (jumpQued &&!jumping)
            {
                jumping = ActivateJump();
                jumpQued = false;
            }

            if (jumping)
            { 
                bool finishedMoving = ContinueJumping();
                if (finishedMoving)
                {
                    currentlyMoving = false;
                    jumping = false;
                    tempFramesPerSecond = framesPerSecond;
                    //   SetCurrentLocation();
                    TiePositionToGrid();
                    CheckExitStatus();
                    CheckWindJumpStatus();
                }
            }
        }

        //if ()
        if (!currentlyMoving && gameData.dashing || continueDashing)
        {
            if (waitTimer >= 0)
            {
                Debug.Log("HI!! :D I don't think this is ever called. If you see me, come check out CharacterMovement.cs to see what changed!");
                waitTimer -= Time.deltaTime;
                SetShieldGraphic(waitTimer, true);
                return;
            }
            tempMovementSpeed = MoveSpeed * dashSpeed;
            tempFramesPerSecond = framesPerSecond * dashSpeed;
            totalDashed += Time.deltaTime * tempMovementSpeed;
            if (!continueDashing)
            {
                if (totalDashed >= DASH_LENGTH)
                {
                    TiePositionToGrid();
                    gameData.dashing = false;
                    sRender.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    CheckWindJumpStatus();
                    CheckExitStatus();
                    tempFramesPerSecond = framesPerSecond;
                    tempMovementSpeed = MoveSpeed;
                }
                else
                {
                    SetShieldGraphic(totalDashed, false);
                    SetNextDashLocation();
                }
            }

            if (continueDashing) {
                if (ContinueMoving() == 0)
                {
                    continueDashing = false; 
                }
            }

        }


        //If in the process of moving, keep moving and do nothing else
        if (currentlyMoving)
        {
            float finishedMoving = ContinueMoving();
            if (finishedMoving == 0)
            {
                currentlyMoving = false;
                TiePositionToGrid();
                //  SetCurrentLocation();
                CheckWindJumpStatus();
                CheckExitStatus();
            }
        }

        sRender.material.SetInt("_IsSmoke", (GameData.Instance.hasted?1:0));
        sRender.material.SetInt("_IsStealthed", (GameData.Instance.stealthed?1:0));

    }

    private void SetShieldGraphic(float amount, bool isCharging)
    {
        GameObject shield = sRender.gameObject.transform.GetChild(0).gameObject;
        Material m = shield.GetComponent<Renderer>().material;
        if (isCharging)
        {
            float amountThrough;
            if (amount > .3f)
            {
                amountThrough = (amount - .3f) * 10;
                m.SetFloat("_fpower", Mathf.Lerp(0, 3, amountThrough));
                m.SetFloat("_offset", Mathf.Lerp(0, 2, amountThrough));
                return;
            }
            if (amount > .2f)
            {
                amountThrough = (amount - .2f) * 10;
                m.SetFloat("_fpower", 0);
                m.SetFloat("_offset", Mathf.Lerp(2, 0, amountThrough));
                return;
            }
            m.SetFloat("_fpower", 0);
            m.SetFloat("_offset", 2);
            return;
        }
        else
        {
            float amountThrough = amount / DASH_LENGTH;
            m.SetFloat("_fpower", Mathf.Lerp(0, 3, amountThrough));
        }
    }

    internal void MurderPlayer()
    {
        GameData.Instance.killPlayer();
    }

    internal void AttemptRest()
    {
        if (GameState.isInBattle) return;
        if (playerStats.mana < playerStats.MaxMana || playerStats.HP < playerStats.MaxHP) {
            if (gameData.addHealToTimer()) {
                //Play a resting doodad
                playerStats.mana += (int)(playerStats.MaxMana * .12f);
                playerStats.HP += (int)(playerStats.MaxHP * .12f);
                if (playerStats.mana > playerStats.MaxMana) playerStats.mana = playerStats.MaxMana;
                if (playerStats.HP > playerStats.MaxHP) playerStats.HP = playerStats.MaxHP;
            }
        }
    }

    private void SetNextDashLocation()
    {
        SetNextLocation(facedDirection);
        if (IsPlayerMoveLocationPassable(characterNextLocation.x, characterNextLocation.y))
        {
            //if it is possible, check for a monster attack
            //Needs to be refractored a bit
            UpdateNewEntityGridLocation();
            RemoveOldEntityGridLocation();
            characterLocation = characterNextLocation;
            continueDashing = true;
        }

    }

    internal void PowerDownCheat()
    {
        playerStats.attack = 0;
        playerStats.defense = 1000;
        
    }

    internal void PowerUpCheat()
    {
        playerStats.AddExp(100000);
        //playerStats.powersGained = 4;
    }

    private void CheckWindJumpStatus()
    {
        GameObject windJumpLocation = MapGrid.GetComponent<DoodadGrid>().grid[characterLocation.x, characterLocation.y];
        if (windJumpLocation != null)
        {
            if (windJumpLocation.GetComponent<DoodadData>().isWindShifter)
            {
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

        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }

        if (waitTimer >= 0 && inputDirection != (int)DirectionMoved.NONE) {
            facedDirection = inputDirection;
            SetLookDirection();
        }

        if (!currentlyMoving && !jumping && !gameData.dashing && !jumpQued&&!windJump )
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
            else { SetLookDirection(); }

            //Check if a monster is in the next location, and initiate combat if so
            GameObject EnemyToFight = isThereAMonster();
            if (EnemyToFight != null && !gameData.dashing)
            {
                Combat.InitiateFight(this.gameObject, EnemyToFight);
            }
        }
    }

    internal void PowerActivateKeyReceived()
    {
        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }
        if (!jumping && !gameData.dashing)
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
        if (playerStats.mana < 10)
        {
            //TODO: Play buzzing Sound
            //TODO: Make MP bar flash
            return false;
        }

        bool jumpingTemp = true;
        switch (facedDirection)
        {
            case DirectionMoved.UP:
                if (isLocationJumpOverable(characterLocation.x, characterLocation.y + 1) && IsPlayerMoveLocationPassable(characterLocation.x, characterLocation.y + 2))
                {

                    SetNextLocationActual(characterLocation.x, characterLocation.y + 2);
                    tempFramesPerSecond *= jumpSpeed;
                }
                else
                {
                    jumpingTemp = false;
                }
                break;
            case DirectionMoved.DOWN:
                if (isLocationJumpOverable(characterLocation.x, characterLocation.y - 1) && IsPlayerMoveLocationPassable(characterLocation.x, characterLocation.y - 2))
                {

                    SetNextLocationActual(characterLocation.x, characterLocation.y - 2);
                    tempFramesPerSecond *= jumpSpeed;
                }
                else
                {
                    jumpingTemp = false;
                }
                break;
            case DirectionMoved.LEFT:
                if (isLocationJumpOverable(characterLocation.x - 1, characterLocation.y) && IsPlayerMoveLocationPassable(characterLocation.x - 2, characterLocation.y))
                {

                    SetNextLocationActual(characterLocation.x - 2, characterLocation.y);
                    tempFramesPerSecond *= jumpSpeed;
                }
                else jumpingTemp = false;
                break;
            case DirectionMoved.RIGHT:
                if (isLocationJumpOverable(characterLocation.x + 1, characterLocation.y) && IsPlayerMoveLocationPassable(characterLocation.x + 2, characterLocation.y))
                {

                    SetNextLocationActual(characterLocation.x + 2, characterLocation.y);
                    tempFramesPerSecond *= jumpSpeed;
                }
                else jumpingTemp = false;
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
        if (gameData.hasted)
        {
            gameData.hasted = false;
            tempMovementSpeed = MoveSpeed;
            tempFramesPerSecond = framesPerSecond;
        }
        else
        {
            if (playerStats.mana >= 6)
            {
                playerStats.mana -= 6;
                tempMovementSpeed = MoveSpeed * hasteSpeed;
                tempFramesPerSecond = framesPerSecond*hasteSpeed;
                gameData.hasted = true;
                gameData.stealthed = false;
            }
            else
            {
                gameData.hasted = false;
                tempMovementSpeed = MoveSpeed;
                tempFramesPerSecond = framesPerSecond;
            }
        }

       
    }

    private void ActivateInvisibility()
    {
        if (gameData.stealthed)
        {
            gameData.stealthed = false;
            tempMovementSpeed = MoveSpeed;
            tempFramesPerSecond = framesPerSecond;
        }
        else
        {
            if (playerStats.mana >= 2)
            {
                playerStats.mana -= 2;
                tempMovementSpeed = MoveSpeed * stealthspeed;
                tempFramesPerSecond = framesPerSecond*stealthspeed;
                gameData.stealthed = true;
                gameData.hasted = false;
            }
            else
            {
                gameData.hasted = false;
                tempMovementSpeed = MoveSpeed;
                tempFramesPerSecond = framesPerSecond;
            }
        }
    }

    private void ActivateShieldDash()
    {
        if (playerStats.mana >= 5)
        {
            sRender.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            playerStats.mana -= 5;
            gameData.dashing = true;
            gameData.hasted = false;
            gameData.stealthed = false;
            waitTimer = .4f;
            totalDashed = 0;
        }
    }

    internal void PowerToggleLeftKeyReceived()
    {
        if (playerStats.powersGained == 0) return;
        playerStats.currentPower -= 1;
        if (playerStats.currentPower < 0) {
            playerStats.currentPower = playerStats.powersGained;
        }

    }


    internal void PowerToggleRightKeyReceived()
    {

        if (playerStats.powersGained == 0) return;
        playerStats.currentPower += 1;
        if (playerStats.currentPower > playerStats.powersGained)
        {
            playerStats.currentPower = 0;
        }

    }

    public void ActivateKeyReceived() {
        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }
        GameObject entityToCheck = null;
        if (!currentlyMoving && !jumping &&!gameData.dashing) {
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
            finishedMoving = MoveUp(tempMovementSpeed);
        }
        if (facedDirection == DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(tempMovementSpeed);
        }
        if (facedDirection == DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(tempMovementSpeed);
        }
        if (facedDirection == DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(tempMovementSpeed);
        }
        return finishedMoving;
    }

    private bool ContinueJumping()
    {
        return Jump(tempMovementSpeed * jumpSpeed, facedDirection);
    }

    public bool Jump(float jumpMoveSpeed, DirectionMoved dir)
    {
        float DistanceToMove = Time.deltaTime * jumpMoveSpeed;
        movedSoFar += DistanceToMove;

        AnimateMove(dir);
        if (movedSoFar > 2)
        {
            TiePositionToGrid();
            jumpPivot.transform.localPosition = Vector3.zero;
            movedSoFar = 0;
            GameData.Instance.jumping = false;
            return true;
        }

        jumpPivot.transform.localPosition = new Vector3(0,JUMP_HEIGHT*(-movedSoFar * movedSoFar + 2* movedSoFar), 0);
        transform.Translate(dir.GetDirectionVector()*DistanceToMove);
        GameData.Instance.jumping = true;
        return false;
    }
}
