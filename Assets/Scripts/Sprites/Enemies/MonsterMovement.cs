using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MonsterMovement : SpriteMovement
{
    // Start is called before the first frame update

    private DirectionMoved NextStep;
    private float finishedMoving;

    public bool PathRandomly;
    public bool PathViaSteps;

    public DirectionMoved[] Pathing = {DirectionMoved.UP, DirectionMoved.UP, DirectionMoved.UP,
                                            DirectionMoved.RIGHT, DirectionMoved.RIGHT, DirectionMoved.RIGHT,
                                            DirectionMoved.DOWN, DirectionMoved.DOWN, DirectionMoved.DOWN,
                                            DirectionMoved.LEFT, DirectionMoved.LEFT, DirectionMoved.LEFT};

    public bool SpotterMonster;
    public int SpottingDistance = 5;//in squares
    public int LookDuration = 10; //in deciseconds
    public int ChaseRange = 15;
    public DirectionMoved[] LookPattern;

    private bool CurrentlyChasingPlayer=false;

    private int ChaseStepNumber = 0;
    private int CurrentStep = 0;
    private float LookTiming = 0;
    private int CurrentFacing = 0;
    private int stuck = 0;
    
    


    // Update is called once per frame
    void Update()
    {

        if (GameState.isInBattle==true) {
            return; 
        }


        if (!currentlyMoving)
        {

            SetCurrentLocation();

            if (PathViaSteps) {
                NextStep = GetNextStep();
                SetNextLocation(NextStep);
                facedDirection = NextStep;
                if (IsMoveLocationPassable(characterNextLocation.x, characterNextLocation.y))
                {
                    UpdateNewEntityGridLocation();
                    RemoveOldEntityGridLocation();
                    characterLocation = characterNextLocation;
                    currentlyMoving = true;
                }
                CheckForFight(characterNextLocation.x, characterNextLocation.y);
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
                    UpdateNewEntityGridLocation();
                    RemoveOldEntityGridLocation();
                    characterLocation = characterNextLocation;
                    currentlyMoving = true;
                }
                else {
                    waitTimer = .25f;
                    SetLookDirection();
                }

                if (IsPlayerInMonsterTerritory(characterNextLocation.x, characterNextLocation.y))
                    CheckForFight(characterNextLocation.x, characterNextLocation.y);
            }


            if (SpotterMonster) {

                if (!CurrentlyChasingPlayer) {
                    ChangeMonsterFacing();
                }


                if (IsPlayerInView() && !CurrentlyChasingPlayer) {
                    CurrentlyChasingPlayer = true;
                    ChaseStepNumber = 0;
                    waitTimer = .25f;
                }
                if (waitTimer >= 0)
                {
                    waitTimer -= Time.deltaTime;
                    return;
                }

                if (CurrentlyChasingPlayer) {
                    NextStep = GetChaseStep();
                    SetNextLocation(NextStep);//
                    CheckForFight(characterNextLocation.x, characterNextLocation.y);//
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
                        UpdateNewEntityGridLocation();
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
                tiePositionToGrid();
            }
        }


    }

    private void CheckForFight(int locX, int locY)
    {
        if (!IsLocationDoodadMonsterPassible(locX, locY)) {
            return;
        }
        if (gameData.dashing||gameData.stealthed){
            return;
        }
        GameObject EnemyToFight = isThereAPlayer(locX, locY);
        if (EnemyToFight != null)
        {
            Combat.InitiateFight(EnemyToFight, this.gameObject);
        }
    }

    private void ChangeMonsterFacing()
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

    private DirectionMoved PathToHomeLocation()
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

    

    private bool IsPlayerInView()
    {
        if (gameData.stealthed) {
            return false;
        }
        bool PlayerFound = false;
        //
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < SpottingDistance; j++) {
                if (i == 0 && j == 0) continue;
                if (i == 2 && j == 0) continue;
                switch (facedDirection) {
                    case DirectionMoved.UP:
                        if( isThereAPlayer(characterLocation.x + i - 1, characterLocation.y + j + 1) != null)
                        {
                            ThePlayer = isThereAPlayer(characterLocation.x + i - 1, characterLocation.y + j + 1);
                            return true;
                        }
                        //if there is not a player found AND spot is not passible, set j=SpottingDistance as a way of skipping further checks along that line. 
                        if (!IsMoveLocationMonsterChaseable(characterLocation.x + i - 1, characterLocation.y + j + 1)) { j = SpottingDistance; }
                            break;
                    case DirectionMoved.DOWN:
                        if (isThereAPlayer(characterLocation.x + i - 1, characterLocation.y - j - 1) != null)
                        {
                            ThePlayer = isThereAPlayer(characterLocation.x + i - 1, characterLocation.y - j - 1);
                            return true;
                        }
                        if (!IsMoveLocationMonsterChaseable(characterLocation.x + i - 1, characterLocation.y - j - 1)) { j = SpottingDistance; }
                        break;
                    case DirectionMoved.LEFT:
                        if (isThereAPlayer(characterLocation.x - j - 1, characterLocation.y + i - 1) != null)
                        {
                            ThePlayer = isThereAPlayer(characterLocation.x - j - 1, characterLocation.y + i - 1);
                            return true;
                        }
                        if (!IsMoveLocationMonsterChaseable(characterLocation.x - j - 1, characterLocation.y + i - 1)) { j = SpottingDistance; }
                        break;
                    case DirectionMoved.RIGHT:
                        if (isThereAPlayer(characterLocation.x + j + 1, characterLocation.y + i - 1) != null)
                        {
                            ThePlayer = isThereAPlayer(characterLocation.x + j + 1, characterLocation.y + i - 1);
                            return true;
                        }
                        if (!IsMoveLocationMonsterChaseable(characterLocation.x + j + 1, characterLocation.y + i - 1)) { j = SpottingDistance; }
                        break;
                }
            }
        }

        return PlayerFound;
    }

    private DirectionMoved GetRandomStep()
    {
        //DirectionMoved nexstp = 0;
        System.Random rand = new System.Random();
        int dirOrdinal = rand.Next(8)+1;
        if (dirOrdinal > 4) dirOrdinal = (int)facedDirection;
        return (DirectionMoved)dirOrdinal;
    }

    private DirectionMoved GetNextStep()
    {
        DirectionMoved nexstp;

            if (CurrentStep == Pathing.Length)
                CurrentStep = 0;
            nexstp = Pathing[CurrentStep];
            CurrentStep++;
                
        return nexstp;
    }


}
