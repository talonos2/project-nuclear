using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterMovement : SpriteMovement
{
    // Start is called before the first frame update

    private int NextStep;
    private float finishedMoving;

    public bool PathRandomly;
    public bool PathViaSteps;
    public int[] Pathing = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4,4,4 };
    public bool SpotterMonster;
    public int SpottingDistance = 4;//in squares
    public int LookDuration = 5; //in deciseconds
    public int[] LookPattern; //1 for N, 2 for E, 3 for S, 4 for W

    private bool CurrentlyChasingPlayer=false;
    private int ChaseRange = 5;
    private int ChaseStepNumber = 0;
    private int CurrentStep = 0;
    private float LookTiming = 0;
    private int CurrentFacing = 0;


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
                    CurrentlyMoving = true;
                }
            }

            if (PathRandomly) {
                NextStep = GetRandomStep();
                SetNextLocation(NextStep);
                FacedDirection = NextStep;
                if (IsRandomMoveLocationPassable(CharacterNextLocation.x, CharacterNextLocation.y))
                {
                    UpdateNewEntityGridLocation();
                    RemoveOldEntityGridLocation();
                    CurrentlyMoving = true;
                }
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
                    CurrentlyMoving = true;
                    NextStep = GetChaseStep();
                    if (ChaseStepNumber >= ChaseRange)
                    {                       
                        NextStep = PathToHomeLocatioin();
                        if (NextStep == (int)DirectionMoved.NONE) {
                            CurrentlyChasingPlayer = false;
                            CurrentlyMoving = false;
                            return;
                        }
                    }

                    SetNextLocation(NextStep);
                    FacedDirection = NextStep;
                    if (IsMoveLocationPassable(CharacterNextLocation.x, CharacterNextLocation.y))
                    {
                        UpdateNewEntityGridLocation();
                        RemoveOldEntityGridLocation();                        
                    }
                    ChaseStepNumber += 1;                   
                }

            }

            //Check if there is a fight about to happen.
            GameObject EnemyToFight = isThereAPlayer(CharacterNextLocation.x, CharacterNextLocation.y);
            if (EnemyToFight != null)
            {
                Combat.initiateFight(EnemyToFight, this.gameObject);
            }                
        }

        if (CurrentlyMoving == true)
        {
            float finishedMoving = MoveToNextSquare();
            if (finishedMoving == 0)
                CurrentlyMoving = false;
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

    private int PathToHomeLocatioin()
    {
        //Returns DirectionMoved.NONE if they are at home. 
        throw new NotImplementedException();
    }

    private int GetChaseStep()
    {
        throw new NotImplementedException();
    }

    private bool IsPlayerInView()
    {
        bool PlayerFound = false;
        //Needs access to player game object, which should be universal to the scene. In fact, it should be found at Start by the SpriteMovement script
        //uses faceddirection
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < SpottingDistance; j++) {
                switch (FacedDirection) {
                    case (int)DirectionMoved.UP:
                        if( isThereAPlayer(CharacterLocation.x + i - 1, CharacterLocation.y + j + 1)!=null)
                            return true;
                        if (!IsMoveLocationMonsterChaseable(CharacterLocation.x + i - 1, CharacterLocation.y + j + 1)) { j = SpottingDistance; }
                        //if there is not a player found AND spot is not passible, set j=SpottingDistance as a way of skipping further checks along that line. 
                            break;
                    case (int)DirectionMoved.DOWN:
                        if (isThereAPlayer(CharacterLocation.x + i - 1, CharacterLocation.y - j - 1) != null)
                            return true;
                        if (!IsMoveLocationMonsterChaseable(CharacterLocation.x + i - 1, CharacterLocation.y - j - 1)) { j = SpottingDistance; }
                        break;
                    case (int)DirectionMoved.LEFT:
                        if (isThereAPlayer(CharacterLocation.x - j - 1, CharacterLocation.y + i - 1) != null)
                            return true;
                        if (!IsMoveLocationMonsterChaseable(CharacterLocation.x - j - 1, CharacterLocation.y + i - 1)) { j = SpottingDistance; }
                        break;
                    case (int)DirectionMoved.RIGHT:
                        if (isThereAPlayer(CharacterLocation.x + j + 1, CharacterLocation.y + i - 1) != null)
                            return true;
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
