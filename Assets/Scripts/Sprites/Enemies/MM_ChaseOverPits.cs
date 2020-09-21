using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_ChaseOverPits : MonsterMovement
{

    // Update is called once per frame
    void Update()
    {
        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }

        HandleMonsterForcedJump();

        if (!currentlyMoving)
        {

            if (IsInAForcedJump())
            {
                return;
            }

            SetCurrentLocation();



                if (!CurrentlyChasingPlayer)
                {
                    ChangeMonsterFacing();
                }


                if (IsPlayerInViewByFlier() && !CurrentlyChasingPlayer)
                {
                    CurrentlyChasingPlayer = true;
                    ChaseStepNumber = 0;
                    hazardIcon.enabled = true;
                    waitTimer = SpotWaitTimer;
                }
                if (waitTimer >= 0)
                {
                    waitTimer -= Time.deltaTime;
                    return;
                }
                hazardIcon.enabled = false;

                if (CurrentlyChasingPlayer)
                {
                    NextStep = GetChaseStep();
                    SetNextLocation(NextStep);//
                    //CheckForFight(characterNextLocation.x, characterNextLocation.y);//
                    if (ChaseStepNumber >= ChaseRange)
                    {
                        NextStep = PathToHomeLocation();
                        if (NextStep == (int)DirectionMoved.NONE)
                        {
                            CurrentlyChasingPlayer = false;
                            currentlyMoving = false;
                            return;
                        }
                    }

                    SetNextLocation(NextStep);
                    facedDirection = NextStep;

                    if (!IsMoveLocationFlyingMonsterChaseable(characterNextLocation.x, characterNextLocation.y) && stuck >= 15)
                    {
                        NextStep = GetRandomStep();
                        SetNextLocation(NextStep);
                        facedDirection = NextStep;


                    }

                    if (IsMoveLocationFlyingMonsterChaseable(characterNextLocation.x, characterNextLocation.y))
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


   
}
