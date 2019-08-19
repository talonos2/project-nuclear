using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MonsterMovement : SpriteMovement
{
    // Start is called before the first frame update

    private int NextStep;
    private float finishedMoving;

    public bool PathRandomly;
    public bool PathViaSteps;
    public int[] Pathing = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4,4,4 };
    public bool SpotterMonster;
    public int SpottingDistance = 5;//in squares
    public int LookDuration = 10; //in deciseconds
    public int ChaseRange = 15;
    public int[] LookPattern; //1 for N, 2 for E, 3 for S, 4 for W

    private bool CurrentlyChasingPlayer=false;

    private int ChaseStepNumber = 0;
    private int CurrentStep = 0;
    private float LookTiming = 0;
    private int CurrentFacing = 0;
    private int stuck = 0;
    


    // Update is called once per frame
    void Update()
    {

        //if (MoveLocationIsMonster()) {

        // }
        //Test: will next step run into a player 
        //Test: will next step run into a wall or monster
        //Test: will next step run into a forbidden zone?
        //Test: are There no locations to go? if not skip this movement phase
        //If none of the above, update





        if (!CurrentlyMoving)
        {

            SetCurrentLocation();

            if (PathViaSteps) {
                NextStep = GetNextStep();
                SetNextLocation(NextStep);
                FacedDirection = NextStep;
                if (IsMoveLocationPassable(CharacterNextLocation.x, CharacterNextLocation.y))
                {
                    UpdateNewEntityGridLocation();
                    RemoveOldEntityGridLocation();
                    CharacterLocation = CharacterNextLocation;
                    CurrentlyMoving = true;
                }
                CheckForFight(CharacterNextLocation.x, CharacterNextLocation.y);
            }

            if (PathRandomly) {
                NextStep = GetRandomStep();
                SetNextLocation(NextStep);
                FacedDirection = NextStep;
                if (IsRandomMoveLocationPassable(CharacterNextLocation.x, CharacterNextLocation.y))
                {
                    UpdateNewEntityGridLocation();
                    RemoveOldEntityGridLocation();
                    CharacterLocation = CharacterNextLocation;
                    CurrentlyMoving = true;
                }
                CheckForFight(CharacterNextLocation.x, CharacterNextLocation.y);
            }


            if (SpotterMonster) {

                if (!CurrentlyChasingPlayer) {
                    ChangeMonsterFacing();
                }

                if (IsPlayerInView() && !CurrentlyChasingPlayer) {
                    CurrentlyChasingPlayer = true;
                    ChaseStepNumber = 0;
                }

                if (CurrentlyChasingPlayer) {
                    NextStep = GetChaseStep();
                    SetNextLocation(NextStep);//
                    CheckForFight(CharacterNextLocation.x, CharacterNextLocation.y);//
                    if (ChaseStepNumber >= ChaseRange)
                    {                       
                        NextStep = PathToHomeLocation();
                        if (NextStep == (int)DirectionMoved.NONE) {
                            CurrentlyChasingPlayer = false;
                            CurrentlyMoving = false;
                            return;
                        }
                    }

                    SetNextLocation(NextStep);
                    FacedDirection = NextStep;

                    if (!IsMoveLocationMonsterChaseable(CharacterNextLocation.x, CharacterNextLocation.y) && stuck >= 15)
                    {
                        NextStep = GetRandomStep();
                        SetNextLocation(NextStep);
                        FacedDirection = NextStep;
                        Debug.Log("MonsterIsStuck");

                    }

                    if (IsMoveLocationMonsterChaseable(CharacterNextLocation.x, CharacterNextLocation.y))
                    {
                        UpdateNewEntityGridLocation();
                        RemoveOldEntityGridLocation();
                        CharacterLocation = CharacterNextLocation;
                        CurrentlyMoving = true;
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




        if (CurrentlyMoving == true)
        {
            float finishedMoving = MoveToNextSquare();
            if (finishedMoving == 0)
                CurrentlyMoving = false;
        }


    }

    private void CheckForFight(int locX, int locY)
    {
        GameObject EnemyToFight = isThereAPlayer(locX, locY);
        if (EnemyToFight != null)
        {
            Combat.initiateFight(EnemyToFight, this.gameObject);
        }
    }

    private void ChangeMonsterFacing()
    {

        LookTiming += Time.deltaTime;
        if (LookTiming >= (.1 * LookDuration)) {
            LookTiming = 0;
            if (CurrentFacing == LookPattern.Length)
                CurrentFacing = 0;
            FacedDirection = LookPattern[CurrentFacing];
            CurrentFacing++;
            SetLookDirection();
        }
            
    }

    private int PathToHomeLocation()
    {
        //Returns DirectionMoved.NONE if they are at home. 
        int TempX= HomeLocation.x-CharacterLocation.x;
        int TempY= HomeLocation.y-CharacterLocation.y;
        if (TempX == 0 && TempY == 0) {
            return (int)DirectionMoved.NONE;
        }
        //Pick an axis based on a random number with the abs value distances as it's input
        int AxisChosen=Random.Range(0,(Math.Abs(TempX) + Math.Abs(TempY)))+1;
        if (Math.Abs(TempX) - AxisChosen >= 0)
        { 
            //go x direction
            if (TempX < 0) { return (int)DirectionMoved.LEFT; }
            else { return (int)DirectionMoved.RIGHT; }
        }
        else {
            //go y Direction
            if (TempY < 0) { return (int)DirectionMoved.DOWN; }
            else { return (int)DirectionMoved.UP; }


        }

      // return 0;

    }

    

    private bool IsPlayerInView()
    {
        bool PlayerFound = false;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < SpottingDistance; j++) {
                switch (FacedDirection) {
                    case (int)DirectionMoved.UP:
                        if( isThereAPlayer(CharacterLocation.x + i - 1, CharacterLocation.y + j + 1) != null)
                        {
                            ThePlayer = isThereAPlayer(CharacterLocation.x + i - 1, CharacterLocation.y + j + 1);
                            return true;
                        }
                        //if there is not a player found AND spot is not passible, set j=SpottingDistance as a way of skipping further checks along that line. 
                        if (!IsMoveLocationMonsterChaseable(CharacterLocation.x + i - 1, CharacterLocation.y + j + 1)) { j = SpottingDistance; }
                            break;
                    case (int)DirectionMoved.DOWN:
                        if (isThereAPlayer(CharacterLocation.x + i - 1, CharacterLocation.y - j - 1) != null)
                        {
                            ThePlayer = isThereAPlayer(CharacterLocation.x + i - 1, CharacterLocation.y - j - 1);
                            return true;
                        }
                        if (!IsMoveLocationMonsterChaseable(CharacterLocation.x + i - 1, CharacterLocation.y - j - 1)) { j = SpottingDistance; }
                        break;
                    case (int)DirectionMoved.LEFT:
                        if (isThereAPlayer(CharacterLocation.x - j - 1, CharacterLocation.y + i - 1) != null)
                        {
                            ThePlayer = isThereAPlayer(CharacterLocation.x - j - 1, CharacterLocation.y + i - 1);
                            return true;
                        }
                        if (!IsMoveLocationMonsterChaseable(CharacterLocation.x - j - 1, CharacterLocation.y + i - 1)) { j = SpottingDistance; }
                        break;
                    case (int)DirectionMoved.RIGHT:
                        if (isThereAPlayer(CharacterLocation.x + j + 1, CharacterLocation.y + i - 1) != null)
                        {
                            ThePlayer = isThereAPlayer(CharacterLocation.x + j + 1, CharacterLocation.y + i - 1);
                            return true;
                        }
                        if (!IsMoveLocationMonsterChaseable(CharacterLocation.x + j + 1, CharacterLocation.y + i - 1)) { j = SpottingDistance; }
                        break;
                }
            }
        }

        return PlayerFound;
    }

    private int GetRandomStep()
    {
        int nexstp = 0;
        System.Random rand = new System.Random();
        nexstp = rand.Next(4) + 1;
        return nexstp;
    }

    private int GetNextStep()
    {
        int nexstp=0;

            if (CurrentStep == Pathing.Length)
                CurrentStep = 0;
            nexstp = Pathing[CurrentStep];
            CurrentStep++;
                
        return nexstp;
    }


}
