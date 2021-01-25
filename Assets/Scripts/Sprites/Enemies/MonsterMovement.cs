using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MonsterMovement : SpriteMovement
{
    // Start is called before the first frame update

    protected DirectionMoved NextStep;
    protected float finishedMoving;

    public bool PathRandomly;
    public bool PathAntiRandomly;
    public bool PathViaSteps;

    public DirectionMoved[] Pathing = {DirectionMoved.UP, DirectionMoved.UP, DirectionMoved.UP,
                                            DirectionMoved.RIGHT, DirectionMoved.RIGHT, DirectionMoved.RIGHT,
                                            DirectionMoved.DOWN, DirectionMoved.DOWN, DirectionMoved.DOWN,
                                            DirectionMoved.LEFT, DirectionMoved.LEFT, DirectionMoved.LEFT};

    public bool SpotterMonster;
    public int SpottingDistance = 5;//in squares
    public int LookDuration = 10; //in deciseconds
    public int ChaseRange = 15;
    public float SpotWaitTimer = .3f;
    public DirectionMoved[] LookPattern;

    protected bool CurrentlyChasingPlayer=false;

    protected int ChaseStepNumber = 0;
    protected int CurrentStep = 0;
    protected float LookTiming = 0;
    protected int CurrentFacing = 0;
    protected int stuck = 0;
    public bool bossMonster;
    public SpriteRenderer hazardIcon;
    Random rand = new Random();
    protected bool combatTriggered;
    private GameObject enemyToFightHolder;





    // Update is called once per frame
    void Update()
    {

        if ((GameState.isInBattle||GameState.fullPause || GameData.Instance.isInDialogue) && !isOnCutsceneMap ) {
            return; 
        }
        if (bossMonster)
        {
            return;
        }

        HandleMonsterForcedJump();
        

        if (!currentlyMoving)
        {
            if (IsInAForcedJump()) {
                return; }

                SetCurrentLocation();

            if (PathViaSteps) {
                NextStep = GetNextStep();
                SetNextLocation(NextStep);
                facedDirection = NextStep;
                if (IsMoveLocationPassable(characterNextLocation.x, characterNextLocation.y))
                {
                    CheckForFight(characterNextLocation.x, characterNextLocation.y);
                    if (!combatTriggered) {
                        UpdateNewEntityGridLocation();
                    }
                    
                    RemoveOldEntityGridLocation();
                    characterLocation = characterNextLocation;
                    currentlyMoving = true;
                }
                
            }

            if (PathRandomly) {
                if (waitTimer >= 0) {
                    waitTimer -= Time.deltaTime;
                    return;
                }
                NextStep = GetRandomStep();
                SetNextLocation(NextStep);
                facedDirection = NextStep;
                if (IsRandomMoveLocationPassable(characterNextLocation.x, characterNextLocation.y))
                {
                    if (IsPlayerInMonsterTerritory(characterNextLocation.x, characterNextLocation.y)) {
                        CheckForFight(characterNextLocation.x, characterNextLocation.y);
                    }
                        
                    if (!combatTriggered)
                    {
                        UpdateNewEntityGridLocation();
                    }
                    RemoveOldEntityGridLocation();
                    characterLocation = characterNextLocation;
                    currentlyMoving = true;
                }
                else {
                    waitTimer = .25f;
                    SetLookDirection();
                }

                //if (IsPlayerInMonsterTerritory(characterNextLocation.x, characterNextLocation.y))
                 //   CheckForFight(characterNextLocation.x, characterNextLocation.y);
            }

            if (PathAntiRandomly)
            {
                if (waitTimer >= 0)
                {
                    waitTimer -= Time.deltaTime;
                    return;
                }
                NextStep = GetRandomStep();
                SetNextLocation(NextStep);
                facedDirection = NextStep;
                if (IsAntiRandomMoveLocationPassable(characterNextLocation.x, characterNextLocation.y))
                {
                    if (!IsPlayerInMonsterTerritory(characterNextLocation.x, characterNextLocation.y))
                    {
                        CheckForFight(characterNextLocation.x, characterNextLocation.y);
                    }
                    if (!combatTriggered)
                    {
                        UpdateNewEntityGridLocation();
                    }
                    RemoveOldEntityGridLocation();
                    characterLocation = characterNextLocation;
                    currentlyMoving = true;
                }
                else
                {
                    waitTimer = .25f;
                    SetLookDirection();
                }
              // if (!IsPlayerInMonsterTerritory(characterNextLocation.x, characterNextLocation.y))
               // {
               //     CheckForFight(characterNextLocation.x, characterNextLocation.y);
               // }
            }


            if (SpotterMonster) {

                if (!CurrentlyChasingPlayer) {
                    ChangeMonsterFacing();
                }


                if (IsPlayerInView() && !CurrentlyChasingPlayer) {
                    CurrentlyChasingPlayer = true;
                    ChaseStepNumber = 0;
                    hazardIcon.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                    waitTimer = SpotWaitTimer;
                }
                if (waitTimer >= 0)
                {
                    waitTimer -= Time.deltaTime;
                    return;
                }
                
                hazardIcon.enabled = false;

                if (CurrentlyChasingPlayer) {
                    NextStep = GetChaseStep();
                    SetNextLocation(NextStep);//
                    //CheckForFight(characterNextLocation.x, characterNextLocation.y);//
                    if (ChaseStepNumber >= ChaseRange)
                    {
                        NextStep = PathToHomeLocation();
                        if (NextStep == (int)DirectionMoved.NONE) {
                            CurrentlyChasingPlayer = false;
                            currentlyMoving = false;
                            return;
                        }
                    }
                    SetNextLocation(NextStep);
                    facedDirection = NextStep;

                    if (!IsMoveLocationMonsterChaseable(characterNextLocation.x, characterNextLocation.y) && stuck >= 15)
                    {
                        NextStep = GetRandomStep();
                        SetNextLocation(NextStep);
                        facedDirection = NextStep;


                    }

                    if (IsMoveLocationMonsterChaseable(characterNextLocation.x, characterNextLocation.y))
                    {
                        CheckForFight(characterNextLocation.x, characterNextLocation.y);
                        if (!combatTriggered) {
                            UpdateNewEntityGridLocation();
                        }

                        RemoveOldEntityGridLocation();
                        characterLocation = characterNextLocation;
                        currentlyMoving = true;
                        ChaseStepNumber += 1;
                        stuck = 0;
                    }
                    else
                    {
                        stuck += 1;
                    }
                    //Debug.Log("FightCheckLocation " + CharacterNextLocation.x + " " + CharacterNextLocation.y);
                    //CheckForFight(CharacterNextLocation.x, CharacterNextLocation.y);
                }
                
                
            }

            //Check if there is a fight about to happen.
           

        }

        if (currentlyMoving == true)
        {
            
            float finishedMoving = MoveToNextSquare();
            if (finishedMoving == 0)
            {
                currentlyMoving = false;
                TiePositionToGrid();
            }
        }

        handleActivateCombat();

    }

    protected void HandleMonsterForcedJump()
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
            }
        }
    }

    protected void handleActivateCombat()
    {
        if (combatTriggered && (GameData.Instance.dashing || GameData.Instance.stealthed)) {
            NextStep = GetRandomStep();
            SetNextLocation(NextStep);
            if (IsMoveLocationPassable(characterNextLocation.x, characterNextLocation.y)) {
                facedDirection = NextStep;
                UpdateNewEntityGridLocation();         
                RemoveOldEntityGridLocation();
                characterLocation = characterNextLocation;
                currentlyMoving = true;
                combatTriggered = false;
            }

            return;
        }

        if (combatTriggered) {

            if (!EnemyToFarToFight(enemyToFightHolder)) {
                Combat.InitiateFight(enemyToFightHolder, this.gameObject);
                combatTriggered = false;
            }
            
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

    protected void CheckForFight(int locX, int locY)
    {
        if (!IsLocationDoodadMonsterPassible(locX, locY)) {
            return;
        }
        if (gameData.dashing||gameData.stealthed){
            return;
        }
        GameObject EnemyToFight = IsThereAPlayer(locX, locY);
        if (EnemyToFight != null)
        {
            enemyToFightHolder = EnemyToFight;
            combatTriggered = true;
            //Combat.InitiateFight(EnemyToFight, this.gameObject);
        }
    }

    protected void ChangeMonsterFacing()
    {

        LookTiming += Time.deltaTime;
        if (LookTiming >= (.1 * LookDuration)) {
            LookTiming = 0;
            if (CurrentFacing == LookPattern.Length)
                CurrentFacing = 0;
            facedDirection = LookPattern[CurrentFacing];
            CurrentFacing++;
            SetLookDirection();
        }
            
    }

    protected DirectionMoved PathToHomeLocation()
    {
        //Returns DirectionMoved.NONE if they are at home. 
        int TempX= homeLocation.x-characterLocation.x;
        int TempY= homeLocation.y-characterLocation.y;
        if (TempX == 0 && TempY == 0) {
            return (int)DirectionMoved.NONE;
        }
        //Pick an axis based on a random number with the abs value distances as it's input
        int AxisChosen=Random.Range(0,(Math.Abs(TempX) + Math.Abs(TempY)))+1;
        if (Math.Abs(TempX) - AxisChosen >= 0)
        { 
            //go x direction
            if (TempX < 0) { return DirectionMoved.LEFT; }
            else { return DirectionMoved.RIGHT; }
        }
        else {
            //go y Direction
            if (TempY < 0) { return DirectionMoved.DOWN; }
            else { return DirectionMoved.UP; }


        }

      // return 0;

    }



    protected bool IsPlayerInView()
    {
        if (GameData.Instance.stealthed) {
            return false;
        }
        bool PlayerFound = false;
        //
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < SpottingDistance; j++) {
                //These remove squares from the search to make it more conic 1/3/5/5/5...
                if (i == 0 && (j == 0 ||j==1)) continue;
                if (i == 4 && (j == 0 || j == 1)) continue;
                if (i == 1 && (j == 0 )) continue;
                if (i == 3 && (j == 0 )) continue;
                switch (facedDirection) {
                    case DirectionMoved.UP:
                        if( IsThereAPlayer(characterLocation.x + i - 2, characterLocation.y + j + 1) != null)
                        {
                            ThePlayer = IsThereAPlayer(characterLocation.x + i - 2, characterLocation.y + j + 1);
                            return true;
                        }
                        //if there is not a player found AND spot is not passible, set j=SpottingDistance as a way of skipping further checks along that line. 
                        if (!IsMoveLocationFlyingMonsterChaseable(characterLocation.x + i - 2, characterLocation.y + j + 1)) { j = SpottingDistance; }
                            break;
                    case DirectionMoved.DOWN:
                        if (IsThereAPlayer(characterLocation.x + i - 2, characterLocation.y - j - 1) != null)
                        {
                            ThePlayer = IsThereAPlayer(characterLocation.x + i - 2, characterLocation.y - j - 1);
                            return true;
                        }
                        if (!IsMoveLocationFlyingMonsterChaseable(characterLocation.x + i - 2, characterLocation.y - j - 1)) { j = SpottingDistance; }
                        break;
                    case DirectionMoved.LEFT:
                        if (IsThereAPlayer(characterLocation.x - j - 1, characterLocation.y + i - 2) != null)
                        {
                            ThePlayer = IsThereAPlayer(characterLocation.x - j - 1, characterLocation.y + i - 2);
                            return true;
                        }
                        if (!IsMoveLocationFlyingMonsterChaseable(characterLocation.x - j - 1, characterLocation.y + i - 2)) { j = SpottingDistance; }
                        break;
                    case DirectionMoved.RIGHT:
                        if (IsThereAPlayer(characterLocation.x + j + 1, characterLocation.y + i - 2) != null)
                        {
                            ThePlayer = IsThereAPlayer(characterLocation.x + j + 1, characterLocation.y + i - 2);
                            return true;
                        }
                        if (!IsMoveLocationFlyingMonsterChaseable(characterLocation.x + j + 1, characterLocation.y + i - 2)) { j = SpottingDistance; }
                        break;
                }
            }
        }

        return PlayerFound;
    }

    internal bool AttemptPushMonster(DirectionMoved playerFacedDirection)
    {

        if (bossMonster)
        {
            return false;
        }

        bool isMonsterPushed = false;

        if (playerFacedDirection == DirectionMoved.UP || playerFacedDirection == DirectionMoved.DOWN) {
            //Debug.Log("I should be moving here "+playerFacedDirection);
           // Debug.Log("Monster Location Before Push " + characterLocation.x + ", " + characterLocation.y);
            if ((transform.position.x - (characterLocation.x+ mapZeroLocation.x))>0) {
                if (IsMoveLocationPassable(characterLocation.x + 1, characterLocation.y))
                {
                    PushMonster(characterLocation.x + 1, characterLocation.y, 1, 0);
                    facedDirection = DirectionMoved.RIGHT;
                    SetLookDirection();
                    isMonsterPushed = true;
                }
                else if (IsMoveLocationPassable(characterLocation.x - 1, characterLocation.y))
                {
                    PushMonster(characterLocation.x - 1, characterLocation.y, -1, 0);
                    facedDirection = DirectionMoved.LEFT;
                    SetLookDirection();
                    isMonsterPushed = true;
                }
            }
            else {
                if (IsMoveLocationPassable(characterLocation.x - 1, characterLocation.y))
                {
                    PushMonster(characterLocation.x - 1, characterLocation.y, -1, 0);
                    facedDirection = DirectionMoved.LEFT;
                    SetLookDirection();
                    isMonsterPushed = true;
                }
                else if (IsMoveLocationPassable(characterLocation.x + 1, characterLocation.y))
                {
                    PushMonster(characterLocation.x + 1, characterLocation.y, 1, 0);
                    facedDirection = DirectionMoved.RIGHT;
                    SetLookDirection();
                    isMonsterPushed = true;
                }
            }
            
        }
        else if (playerFacedDirection == DirectionMoved.RIGHT || playerFacedDirection == DirectionMoved.LEFT) {
            if ((transform.position.y - (characterLocation.y + mapZeroLocation.y)) > 0)
            {
                if (IsMoveLocationPassable(characterLocation.x, characterLocation.y + 1))
                {
                    PushMonster(characterLocation.x, characterLocation.y + 1, 0, 1);
                    facedDirection = DirectionMoved.UP;
                    SetLookDirection();
                    isMonsterPushed = true;
                }
                else if (IsMoveLocationPassable(characterLocation.x, characterLocation.y - 1))
                {
                    PushMonster(characterLocation.x, characterLocation.y - 1, 0, -1);
                    facedDirection = DirectionMoved.DOWN;
                    SetLookDirection();
                    isMonsterPushed = true;
                }
            }
            else {
                if (IsMoveLocationPassable(characterLocation.x, characterLocation.y - 1))
                {
                    PushMonster(characterLocation.x, characterLocation.y - 1, 0, -1);
                    facedDirection = DirectionMoved.DOWN;
                    SetLookDirection();
                    isMonsterPushed = true;
                }
                else if (IsMoveLocationPassable(characterLocation.x, characterLocation.y  +1))
                {
                    PushMonster(characterLocation.x, characterLocation.y + 1, 0, 1);
                    facedDirection = DirectionMoved.UP;
                    SetLookDirection();
                    isMonsterPushed = true;
                }
            }
                
        }
        return isMonsterPushed;
    }

    private void PushMonster(int LocX, int LocY, int jumpTargetX, int jumpTargetY)
    {
        SoundManager.Instance.PlaySound("monsterPunched", 1);
        forcedJumpHeight = .7f;
        timeLeftInForcedJump = .25f;
        totalTimeInForcedJump = .25f;
        currentlyMoving = false;
        jumpStartPos = transform.position;
       // Debug.Log("reverse zero location " + (mapZeroLocation.x + (float)characterLocation.x) + " actual transomr position " + transform.position +", jumpTarget "+jumpTargetX);
        jumpTarget.x = jumpTargetX - (transform.position.x - ((float)characterLocation.x + mapZeroLocation.x))  ;
        //Debug.Log("Jump target x " + jumpTarget.x);
        jumpTarget.y = jumpTargetY - (transform.position.y - ((float)characterLocation.y + mapZeroLocation.y));
        characterNextLocation.x = LocX;
        characterNextLocation.y = LocY;
        UpdateNewEntityGridLocation();    
        RemoveOldEntityGridLocation();
        characterLocation = characterNextLocation;
        //TiePositionToGrid();


       // Debug.Log("Monster moved "+LocX+", "+LocY);
        // if (IsInAForcedJump);
    }

    protected bool IsPlayerInViewByFlier()
    {
        if (gameData.stealthed)
        {
            return false;
        }
        bool PlayerFound = false;
        //
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < SpottingDistance; j++)
            {
                //These remove squares from the search to make it more conic 1/3/5/5/5...
                if (i == 0 && (j == 0 || j == 1)) continue;
                if (i == 4 && (j == 0 || j == 1)) continue;
                if (i == 1 && (j == 0)) continue;
                if (i == 3 && (j == 0)) continue;
                switch (facedDirection)
                {
                    case DirectionMoved.UP:
                        if (IsThereAPlayer(characterLocation.x + i - 2, characterLocation.y + j + 1) != null)
                        {
                            ThePlayer = IsThereAPlayer(characterLocation.x + i - 2, characterLocation.y + j + 1);
                            return true;
                        }
                        //if there is not a player found AND spot is not passible, set j=SpottingDistance as a way of skipping further checks along that line. 
                        if (!IsMoveLocationFlyingMonsterChaseable(characterLocation.x + i - 2, characterLocation.y + j + 1)) { j = SpottingDistance; }
                        break;
                    case DirectionMoved.DOWN:
                        if (IsThereAPlayer(characterLocation.x + i - 2, characterLocation.y - j - 1) != null)
                        {
                            ThePlayer = IsThereAPlayer(characterLocation.x + i - 2, characterLocation.y - j - 1);
                            return true;
                        }
                        if (!IsMoveLocationFlyingMonsterChaseable(characterLocation.x + i - 2, characterLocation.y - j - 1)) { j = SpottingDistance; }
                        break;
                    case DirectionMoved.LEFT:
                        if (IsThereAPlayer(characterLocation.x - j - 1, characterLocation.y + i - 2) != null)
                        {
                            ThePlayer = IsThereAPlayer(characterLocation.x - j - 1, characterLocation.y + i - 2);
                            return true;
                        }
                        if (!IsMoveLocationFlyingMonsterChaseable(characterLocation.x - j - 1, characterLocation.y + i - 2)) { j = SpottingDistance; }
                        break;
                    case DirectionMoved.RIGHT:
                        if (IsThereAPlayer(characterLocation.x + j + 1, characterLocation.y + i - 2) != null)
                        {
                            ThePlayer = IsThereAPlayer(characterLocation.x + j + 1, characterLocation.y + i - 2);
                            return true;
                        }
                        if (!IsMoveLocationFlyingMonsterChaseable(characterLocation.x + j + 1, characterLocation.y + i - 2)) { j = SpottingDistance; }
                        break;
                }
            }
        }

        return PlayerFound;
    }
    protected DirectionMoved GetRandomStep()
    {
        int dirOrdinal = Random.Range(1,5);
        return (DirectionMoved)dirOrdinal;
    }

    protected DirectionMoved GetNextStep()
    {
        DirectionMoved nexstp;

            if (CurrentStep == Pathing.Length)
                CurrentStep = 0;
            nexstp = Pathing[CurrentStep];
            CurrentStep++;
                
        return nexstp;
    }


}
