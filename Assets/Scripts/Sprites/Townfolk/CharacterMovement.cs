using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : SpriteMovement
{
    private static float DASH_LENGTH = 26;

    public CharacterStats playerStats;
    private readonly float hasteSpeed = 2;
    private readonly float stealthspeed = .75f;
    private readonly float dashSpeed = 4;
    private float totalDashed = 0;
    private bool continueDashing = false;
    private Transform jumpPivot;
    GameObject shield;
    ParticleSystem smoke;
    int previousFrame = 0;
    int previousStepSound = 0;
    private GameObject enemyToFightHolder;
    internal bool combatDetected;
    internal IsUIOn uiController;

    new void Start()
    {
        base.Start();
        playerStats = this.GetComponent<CharacterStats>();
        uiController = this.gameObject.GetComponentInChildren<IsUIOn>();
        playerStats.setCharacterMoveScript(this);
        jumpPivot = sRender.transform.parent;
        shield = sRender.gameObject.transform.GetChild(0).gameObject;
        smoke = sRender.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

    }
    // Update is called once per frame
    void Update()
    {
        if (GameState.isInBattle || GameState.fullPause || GameData.Instance.isInDialogue)
        {
            return;
        }

        HandlePlayerFootstepSounds();
        if (HandlePauseBeforeDash()) { return; }
        AdjustSpeedAndReduceManaIfHasted();
        AdjustSpeedAndReduceManaIfStealthed();
        HandleCharacterForcedJump();
        HandleCharacterJumpViaPower();
        HandleIceDashContinue();

        if (combatDetected)
        {
            if (!EnemyToFarToFight(enemyToFightHolder))
            {
                Combat.InitiateFight(this.gameObject, enemyToFightHolder);
                combatDetected = false;
                enemyToFightHolder = null;
            }
        }
        //If in the process of moving, keep moving and do nothing else
        if (currentlyMoving)
        {

            float finishedMoving = ContinueMoving();
            CheckUpcomingExitStatus(characterNextLocation);
            if (finishedMoving == 0)
            {
                currentlyMoving = false;
                TiePositionToGrid();
                //  SetCurrentLocation();
                CheckIfStandingOnWindJumper();
                CheckPostMoveExitStatus();
                CheckGabStatus();
            }
        }

        sRender.material.SetInt("_IsSmoke", (GameData.Instance.hasted ? 1 : 0));
        if (GameData.Instance.hasted && !smoke.isPlaying)
        {
            smoke.Play();
            smoke.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }
        sRender.material.SetInt("_IsStealthed", (GameData.Instance.stealthed ? 1 : 0));

    }

    internal void JumpOffOfSpikes()
    {
        if (IsPlayerMoveLocationTerrainPassable(characterLocation.x - 1, characterLocation.y))
        {
            ForceJump(new Vector2(-1, 0), .12f, .3f);
        }
        else if (IsPlayerMoveLocationTerrainPassable(characterLocation.x + 1, characterLocation.y))
        {
            ForceJump(new Vector2(1, 0), .12f, .3f);
        }
        else if (IsPlayerMoveLocationTerrainPassable(characterLocation.x, characterLocation.y - 1))
        {
            ForceJump(new Vector2(0, -1), .12f, .3f);
        }
        else if (IsPlayerMoveLocationTerrainPassable(characterLocation.x, characterLocation.y + 1))
        {
            ForceJump(new Vector2(0, 1), .12f, .3f);
        }
    }

    private void HandleIceDashContinue()
    {
        if ((!currentlyMoving && GameData.Instance.dashing) || continueDashing)
        {
            tempMovementSpeed = MoveSpeed * dashSpeed;
            tempFramesPerSecond = framesPerSecond * dashSpeed;
            totalDashed += Time.deltaTime * tempMovementSpeed;

            if (!IsPlayerMoveLocationPassable(characterNextLocation.x, characterNextLocation.y) && IsThereAMonster() == null)
            {
                totalDashed += Time.deltaTime * tempMovementSpeed;//Effectivly cuts penalty for dashing in half when running 
                //into monsters, but not when running into walls/pits/boulders, ect
            }

            if (!continueDashing)
            {
                if (totalDashed >= DASH_LENGTH)
                {
                    TiePositionToGrid();
                    GameData.Instance.dashing = false;
                    sRender.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    CheckIfStandingOnWindJumper();
                    CheckPostMoveExitStatus();
                    CheckGabStatus();
                    tempFramesPerSecond = framesPerSecond;
                    tempMovementSpeed = MoveSpeed;
                }
                else
                {
                    if (CheckPostMoveExitStatus())
                    {
                        GameData.Instance.dashing = false;
                    }
                    else
                    {
                        SetShieldGraphic(totalDashed, false);
                        SetNextDashLocation();
                    }

                }
            }

            if (continueDashing)
            {
                if (ContinueMoving() == 0)
                {
                    continueDashing = false;
                }
            }

        }
    }

    private void HandleCharacterJumpViaPower()
    {
        if (!currentlyMoving && (jumping || jumpQueued))
        {
            if (jumpQueued && !jumping)
            {
                jumping = ActivateJump();
                jumpQueued = false;
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
                    CheckPostMoveExitStatus();
                    CheckIfStandingOnWindJumper();
                    CheckGabStatus();
                }
            }
        }
    }

    private void HandleCharacterForcedJump()
    {
        if (!currentlyMoving && timeLeftInForcedJump > 0)
        {
            JumpToTarget();
            bool finishedForcedJump = timeLeftInForcedJump <= 0;
            if (finishedForcedJump)
            {
                currentlyMoving = false;
                TiePositionToGrid();
                tempFramesPerSecond = framesPerSecond;
                CheckIfStandingOnWindJumper();
            }
        }
    }

    private void AdjustSpeedAndReduceManaIfStealthed()
    {
        if (GameData.Instance.stealthed)
        {

            if (GameData.Instance.timerTrigger)
            {
                if (playerStats.mana >= 4)
                {
                    playerStats.mana -= 4;
                    tempMovementSpeed = MoveSpeed * stealthspeed;
                    tempFramesPerSecond = framesPerSecond * stealthspeed;
                }
                else
                {
                    SoundManager.Instance.PlaySound("StealthOff", 1f);
                    GameData.Instance.stealthed = false;
                    tempMovementSpeed = MoveSpeed;
                    tempFramesPerSecond = framesPerSecond;
                }
            }
        }
    }

    private void AdjustSpeedAndReduceManaIfHasted()
    {
        if (GameData.Instance.hasted)
        {
            if (GameData.Instance.timerTrigger)
            {
                if (playerStats.mana >= 12)
                {
                    playerStats.mana -= 12;
                    tempMovementSpeed = MoveSpeed * hasteSpeed;
                    tempFramesPerSecond = framesPerSecond * hasteSpeed;
                }
                else
                {
                    TurnHasteOff();
                    tempMovementSpeed = MoveSpeed;
                    tempFramesPerSecond = framesPerSecond;
                }
            }
        }
    }

    private bool HandlePauseBeforeDash()
    {
        if (!currentlyMoving && waitTimer >= 0)
        {
            if (GameData.Instance.dashing)
            {
                SetShieldGraphic(waitTimer, true);
            }
            waitTimer -= Time.deltaTime;
            return true;
        }
        return false;
    }

    private void HandlePlayerFootstepSounds()
    {
        int currentFrame = Mathf.RoundToInt(sRender.material.GetFloat("_Frame"));
        if (previousFrame != currentFrame && (currentFrame % 7 == 1 || currentFrame % 7 == 4))
        {
            int stepSound = UnityEngine.Random.Range(0, 2) + 1; //Step sounds are no 0-indexed.
            if (previousStepSound == 1 || (previousStepSound == 2 && stepSound == 2))
            {
                stepSound++;
            }
            string materialNameStart = "Stone";
            if (groundMaterialGrid)
            {
                materialNameStart = groundMaterialGrid.grid[characterLocation.x, characterLocation.y].GetName();
            }
            // Debug.Log("Footstep");
            SoundManager.Instance.PlaySound("Footsteps/" + materialNameStart + stepSound, 1f);
            previousStepSound = stepSound;
        }
        previousFrame = currentFrame;
    }

    internal void TurnStealthOff()
    {
        tempMovementSpeed = MoveSpeed;
        tempFramesPerSecond = framesPerSecond;
        GameData.Instance.stealthed = false;
    }

    private void SetShieldGraphic(float amount, bool isCharging)
    {
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
        GameData.Instance.DebugKillPlayer();
    }

    internal void AttemptRest()
    {
        if (GameState.isInBattle) return;
        if (playerStats.mana < playerStats.MaxMana || playerStats.HP < playerStats.MaxHP)
        {
            if (GameData.Instance.addHealToTimer())
            {
                SoundManager.Instance.PlaySound("Healing", 1);
                playerStats.gameObject.GetComponent<HealingAnimationController>().PlayHealingAnimation(2);
                playerStats.mana += (int)(playerStats.MaxMana * .125f);
                playerStats.HP += (int)(playerStats.MaxHP * .125f);
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
        else if (attemptToPushMonster(characterNextLocation.x, characterNextLocation.y))
        {
            UpdateNewEntityGridLocation();
            RemoveOldEntityGridLocation();
            characterLocation = characterNextLocation;
            continueDashing = true;
        }

    }

    private bool attemptToPushMonster(int LocX, int LocY)
    {
        bool MoveableLocation = false;
        GameObject entityInLocation = MapGrid.GetComponent<EntityGrid>().grid[LocX, LocY];
        if (entityInLocation == null)
        {
            return false;
        }
        else if (entityInLocation.GetComponent<EntityData>().isAMonster)
        {
            if (entityInLocation.GetComponent<MonsterMovement>().AttemptPushMonster(this.facedDirection))
            {
                MoveableLocation = true;
            }

        }
        return MoveableLocation;
    }

    internal void PowerDownCheat()
    {
        playerStats.AttackCrystalBuff = -1000;
        playerStats.defenseCrystalBuff = 1000;
        playerStats.PushCharacterData();
    }

    internal void PowerUpCheat()
    {
        playerStats.AttackCrystalBuff = 0;
        playerStats.AddExp(100000);
        playerStats.ShutUpLevelUpper();
        playerStats.powersGained = 4;
        playerStats.PushCharacterData();
    }

    private void CheckIfStandingOnWindJumper()
    {
        GameObject windJumpLocation = MapGrid.GetComponent<DoodadGrid>().grid[characterLocation.x, characterLocation.y];
        if (windJumpLocation != null)
        {
            if (windJumpLocation.GetComponent<DoodadData>().isWindShifter)
            {
                WindJumpController wind = windJumpLocation.GetComponent<WindJumpController>();
                SoundManager.Instance.PlaySound("AirGust", 1);
                ForceJump(wind.jumpDestOffset, wind.timeItTakesToJump, wind.jumpHeight);
            }
        }
    }

    public void ForceJump(Vector2 offset, float time, float height)
    {
        jumpStartPos = transform.position;
        jumpTarget = offset;
        
        SetNextLocation(characterLocation.x + (int)jumpTarget.x, characterLocation.y + (int)jumpTarget.y);
        GameObject EnemyToFight = null;
        EnemyToFight = IsThereAMonster();
        if (EnemyToFight != null)
        {
            if (EnemyToFarToFight(EnemyToFight))
            {
                enemyToFightHolder = EnemyToFight;
                combatDetected = true;
            }
        }
        SetNextLocationActual(characterLocation.x + (int)jumpTarget.x, characterLocation.y + (int)jumpTarget.y);
        if (Math.Abs(jumpTarget.x) > Math.Abs(jumpTarget.y))
        {
            if (jumpTarget.x < 0) { facedDirection = DirectionMoved.LEFT; }
            else { facedDirection = DirectionMoved.RIGHT; }
        }
        else
        {
            if (jumpTarget.y < 0) { facedDirection = DirectionMoved.DOWN; }
            else { facedDirection = DirectionMoved.UP; }
        }
        SetLookDirection();

        totalTimeInForcedJump = time;
        forcedJumpHeight = height;
        timeLeftInForcedJump = totalTimeInForcedJump;
        currentlyMoving = false;
    }

    //Key command received from CharacterInputController script
    public void MoveKeyReceived(DirectionMoved inputDirection)
    {

        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }

        if (waitTimer >= 0 && inputDirection != (int)DirectionMoved.NONE)
        {
            facedDirection = inputDirection;
            SetLookDirection();
        }

        if (!currentlyMoving && !jumping && !GameData.Instance.dashing && !jumpQueued && (timeLeftInForcedJump <= 0))
        {
            if (inputDirection == (int)DirectionMoved.NONE)
            {
                SetLookDirection();
                return;
            }

            facedDirection = inputDirection;

            SetNextLocation(inputDirection);
            GameObject EnemyToFight = null;
            if (IsPlayerMoveLocationTerrainPassable(characterNextLocation.x, characterNextLocation.y))
            {
                //if it is possible, check for a monster attack
                //Needs to be refractored a bit
                EnemyToFight = IsThereAMonster();
                if (EnemyToFight != null)
                {
                    if (EnemyToFarToFight(EnemyToFight))
                    {
                        enemyToFightHolder = EnemyToFight;
                        combatDetected = true;
                    }
                    else
                    {
                        Combat.InitiateFight(this.gameObject, EnemyToFight);
                    }

                    //removeEntity
                }
                UpdateNewEntityGridLocation();
                RemoveOldEntityGridLocation();
                characterLocation = characterNextLocation;
                currentlyMoving = true;

            }
            else { SetLookDirection(); }

            //Check if a monster is in the next location, and initiate combat if so
            //GameObject EnemyToFight = IsThereAMonster();    

            //if (EnemyToFight != null && !GameData.Instance.dashing)
            //{
            //    Combat.InitiateFight(this.gameObject, EnemyToFight);
            //}

        }
    }

    protected bool EnemyToFarToFight(GameObject enemyToFight)
    {
        float xdistance = enemyToFight.transform.position.x - this.transform.position.x;
        float ydistance = enemyToFight.transform.position.y - this.transform.position.y;
        float distanctToEnemy = (float)Math.Sqrt(xdistance * xdistance + ydistance * ydistance);
        //Debug.Log("Distance to Monster " + distanctToEnemy);
        //float distanctToMonster=enemyToFight.gameObject.transform.position.x * this.gameObject.transform.position.x
        if (distanctToEnemy > 1.05)
            return true;
        else return false;
    }

    internal void PowerActivateKeyReceived()
    {
        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }
        if (playerStats.currentPower != (int)ElementalPower.EARTH && GameData.Instance.stealthed)
        {
            SoundManager.Instance.PlaySound("StealthOff", 1f);
            GameData.Instance.stealthed = false;
            tempMovementSpeed = MoveSpeed;
            tempFramesPerSecond = framesPerSecond;
        }
        if (playerStats.currentPower != (int)ElementalPower.FIRE && GameData.Instance.hasted)
        {
            TurnHasteOff();
            tempMovementSpeed = MoveSpeed;
            tempFramesPerSecond = framesPerSecond;
        }


        if (!jumping && !GameData.Instance.dashing)
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
                    jumpQueued = true;
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
                if (isLocationJumpOverable(characterLocation.x, characterLocation.y + 1) && IsPlayerJumpLocationPassable(characterLocation.x, characterLocation.y + 2))
                {
                    SetNextLocation(characterLocation.x, characterLocation.y + 2);
                    SetupCombatOnJump();
                    SetNextLocationActual(characterLocation.x, characterLocation.y + 2);
                    tempFramesPerSecond *= jumpSpeed;
                }
                else
                {
                    jumpingTemp = false;
                }
                break;
            case DirectionMoved.DOWN:
                if (isLocationJumpOverable(characterLocation.x, characterLocation.y - 1) && IsPlayerJumpLocationPassable(characterLocation.x, characterLocation.y - 2))
                {
                    SetNextLocation(characterLocation.x, characterLocation.y - 2);
                    SetupCombatOnJump();
                    SetNextLocationActual(characterLocation.x, characterLocation.y - 2);
                    tempFramesPerSecond *= jumpSpeed;
                }
                else
                {
                    jumpingTemp = false;
                }
                break;
            case DirectionMoved.LEFT:
                if (isLocationJumpOverable(characterLocation.x - 1, characterLocation.y) && IsPlayerJumpLocationPassable(characterLocation.x - 2, characterLocation.y))
                {
                    SetNextLocation(characterLocation.x-2, characterLocation.y);
                    SetupCombatOnJump();
                    SetNextLocationActual(characterLocation.x - 2, characterLocation.y);
                    tempFramesPerSecond *= jumpSpeed;
                }
                else jumpingTemp = false;
                break;
            case DirectionMoved.RIGHT:
                if (isLocationJumpOverable(characterLocation.x + 1, characterLocation.y) && IsPlayerJumpLocationPassable(characterLocation.x + 2, characterLocation.y))
                {
                    SetNextLocation(characterLocation.x+2, characterLocation.y );
                    SetupCombatOnJump();
                    SetNextLocationActual(characterLocation.x + 2, characterLocation.y);
                    tempFramesPerSecond *= jumpSpeed;
                }
                else jumpingTemp = false;
                break;

        }
        if (jumpingTemp == true)
        {
            playerStats.mana -= 10;
            SoundManager.Instance.PlaySound("Jump", 1);
        }
        return jumpingTemp;
    }

    private void SetupCombatOnJump() {      
        GameObject EnemyToFight = null;
        EnemyToFight = IsThereAMonster();
        if (EnemyToFight != null)
        {
            if (EnemyToFarToFight(EnemyToFight))
            {
                enemyToFightHolder = EnemyToFight;
                combatDetected = true;
            }
        }
    }
    private bool isLocationJumpOverable(int characterLocationx, int characterLocationy)
    {
        bool jumpable = true;
        if (MapGrid.GetComponent<PassabilityGrid>().grid[characterLocationx, characterLocationy] == PassabilityType.WALL)
        {
            jumpable = false;
        }
        GameObject entityInLocation = MapGrid.GetComponent<EntityGrid>().grid[characterLocationx, characterLocationy];
        if (entityInLocation != null)
        {
            if (entityInLocation.GetComponent<EntityData>().isItem)
            {
                jumpable = false;
            }
            if (entityInLocation.GetComponent<EntityData>().isLargeMonster)
            {
                jumpable = false;
            }
        }
        GameObject doodadObject = MapGrid.GetComponent<DoodadGrid>().grid[characterLocationx, characterLocationy];
        if (doodadObject != null)
        {
            if (doodadObject.GetComponent<DoodadData>().isTallBlockableTerrain)
            {
                jumpable = false;
            }
        }

        return jumpable;
    }

    private void ActivateHaste()
    {
        if (GameData.Instance.hasted)
        {
            TurnHasteOff();
            tempMovementSpeed = MoveSpeed;
            tempFramesPerSecond = framesPerSecond;
        }
        else
        {
            if (playerStats.mana >= 6)
            {
                playerStats.mana -= 6;
                tempMovementSpeed = MoveSpeed * hasteSpeed;
                tempFramesPerSecond = framesPerSecond * hasteSpeed;
                TurnHasteOn();
                if (GameData.Instance.stealthed)
                {
                    SoundManager.Instance.PlaySound("StealthOff", 1f);
                    GameData.Instance.stealthed = false;
                }
            }
            else
            {
                TurnHasteOff();
                tempMovementSpeed = MoveSpeed;
                tempFramesPerSecond = framesPerSecond;
            }
        }


    }

    public void TurnHasteOn()
    {
        GameData.Instance.hasted = true;
        tempMovementSpeed = MoveSpeed * hasteSpeed;
        tempFramesPerSecond = framesPerSecond * hasteSpeed;
        if (smoke == null) smoke = sRender.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        smoke.Play();
        SoundManager.Instance.PlaySound("HasteOn", 1f);
        smoke.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
    }

    internal void TurnHasteOff()
    {
        if (GameData.Instance.hasted)
        {
            GameData.Instance.hasted = false;
            SoundManager.Instance.PlaySound("HasteOff", 1f);
        }
        tempMovementSpeed = MoveSpeed;
        tempFramesPerSecond = framesPerSecond;
        smoke.Stop();
        smoke.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        DoodadGrid doodads = MapGrid.GetComponent<DoodadGrid>();
        GameObject hopefullyADoodad = doodads.grid[characterLocation.x, characterLocation.y];
        if (hopefullyADoodad != null && hopefullyADoodad.GetComponent<SpikeController>() != null && !hopefullyADoodad.GetComponent<SpikeController>().isPassable)
        {
            JumpOffOfSpikes();
        }
    }

    internal void ActivateInvisibility()
    {
        if (GameData.Instance.stealthed)
        {
            SoundManager.Instance.PlaySound("StealthOff", 1f);
            GameData.Instance.stealthed = false;
            tempMovementSpeed = MoveSpeed;
            tempFramesPerSecond = framesPerSecond;
        }
        else
        {
            if (playerStats.mana >= 2)
            {
                playerStats.mana -= 2;
                tempMovementSpeed = MoveSpeed * stealthspeed;
                tempFramesPerSecond = framesPerSecond * stealthspeed;
                SoundManager.Instance.PlaySound("StealthOn", 1f);
                GameData.Instance.stealthed = true;
                TurnHasteOff();
            }
            else
            {
                TurnHasteOff();
                tempMovementSpeed = MoveSpeed;
                tempFramesPerSecond = framesPerSecond;
            }
        }
    }

    private void ActivateShieldDash()
    {
        if (playerStats.mana >= 5)
        {
            SoundManager.Instance.PlaySound("ShieldDash", 1);
            sRender.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            playerStats.mana -= 5;
            GameData.Instance.dashing = true;
            TurnHasteOff();
            if (GameData.Instance.stealthed)
            {
                SoundManager.Instance.PlaySound("StealthOff", 1f);
                GameData.Instance.stealthed = false;
            }
            waitTimer = .4f;
            totalDashed = 0;
        }
    }

    internal void PowerToggleLeftKeyReceived()
    {
        if (playerStats.powersGained == 0 || GameState.fullPause) return;
        SoundManager.Instance.PlaySound("SwitchElement", 1f);
        playerStats.currentPower -= 1;
        if (playerStats.currentPower < 0)
        {
            playerStats.currentPower = playerStats.powersGained;
        }
        if (GameState.isInBattle)
        {
            //GameObject.FindObjectOfType<Combat>().DisplayElementSwitchVFX(playerStats.currentPower);
        }
    }

    internal void PowerToggleRightKeyReceived()
    {

        if (playerStats.powersGained == 0 || GameState.fullPause) return;
        SoundManager.Instance.PlaySound("SwitchElement", 1f);
        playerStats.currentPower += 1;
        if (playerStats.currentPower > playerStats.powersGained)
        {
            playerStats.currentPower = 0;
        }
        if (GameState.isInBattle)
        {
            //GameObject.FindObjectOfType<Combat>().DisplayElementSwitchVFX(playerStats.currentPower);
        }
    }

    public void ActivateKeyReceived()
    {
        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }
        GameObject entityToCheck = null;
        if (!currentlyMoving && !jumping && !GameData.Instance.dashing)
        {
            switch (facedDirection)
            {
                case DirectionMoved.UP:
                    entityToCheck = mapEntityGrid.grid[characterLocation.x, characterLocation.y + 1];
                    break;
                case DirectionMoved.DOWN:
                    entityToCheck = mapEntityGrid.grid[characterLocation.x, characterLocation.y - 1];
                    break;
                case DirectionMoved.LEFT:
                    entityToCheck = mapEntityGrid.grid[characterLocation.x - 1, characterLocation.y];
                    break;
                case DirectionMoved.RIGHT:
                    entityToCheck = mapEntityGrid.grid[characterLocation.x + 1, characterLocation.y];
                    break;

            }
            if (entityToCheck != null)
            {
                entityToCheck.GetComponent<EntityData>().ProcessClick(this.GetComponent<CharacterStats>());
            }
        }
    }

    private bool CheckPostMoveExitStatus()
    {
        GameObject exitLocation = MapGrid.GetComponent<DoodadGrid>().grid[characterLocation.x, characterLocation.y];
        if (exitLocation != null)
        {
            if (exitLocation.GetComponent<DoodadData>().isExit)
            {
                playerStats.PushCharacterData();
                exitLocation.GetComponent<ExitController>().TransitionMap();
                return true;
            }
        }
        return false;
    }

    private bool CheckUpcomingExitStatus(Vector2Int characterNextLocation)
    {
        GameObject exitLocation = MapGrid.GetComponent<DoodadGrid>().grid[characterNextLocation.x, characterNextLocation.y];
        if (exitLocation != null)
        {
            if (exitLocation.GetComponent<DoodadData>().isExit && characterCloseEnough(exitLocation, this.gameObject))
            {
                playerStats.PushCharacterData();
                exitLocation.GetComponent<ExitController>().TransitionMap();
                return true;
            }
        }
        return false;
    }


    private bool characterCloseEnough(GameObject exitLocation, GameObject character)
    {
        float xdistance = exitLocation.transform.position.x - character.transform.position.x;
        float ydistance = exitLocation.transform.position.y - character.transform.position.y;
        float answer = (float)Math.Sqrt(xdistance * xdistance + ydistance * ydistance);
        if (answer > .5) return false;
        else return true;

    }

    private void CheckGabStatus()
    {
        GameObject gabLocation = MapGrid.GetComponent<DoodadGrid>().grid[characterLocation.x, characterLocation.y];
        //Debug.Log("Is there gab?"+gabLocation);
        if (gabLocation != null)
        {
            //Debug.Log("Gab here: " + gabLocation);
            if (gabLocation.GetComponent<GabTriggerer>() != null)
            {
                gabLocation.GetComponent<GabTriggerer>().TriggerGab();
            }
        }

    }

    private int GetInputDirection()
    {

        int NextInputDirection = -1;

        if (FWInputManager.Instance.GetKeyDown(InputAction.RIGHT))
        {
            NextInputDirection = (int)DirectionMoved.RIGHT;
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.LEFT))
        {
            NextInputDirection = (int)DirectionMoved.LEFT;
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.UP))
        {
            NextInputDirection = (int)DirectionMoved.UP;
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.DOWN))
        {
            NextInputDirection = (int)DirectionMoved.DOWN;
        }

        if (NextInputDirection == -1)
        {
            NextInputDirection = (int)DirectionMoved.NONE;
        }
        return NextInputDirection;
    }

    private float ContinueMoving()
    {
        return MoveDirection(tempMovementSpeed, facedDirection);
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

        jumpPivot.transform.localPosition = new Vector3(0, JUMP_HEIGHT * (-movedSoFar * movedSoFar + 2 * movedSoFar), 0);
        transform.Translate(dir.GetDirectionVector() * DistanceToMove);
        GameData.Instance.jumping = true;
        return false;
    }
}
