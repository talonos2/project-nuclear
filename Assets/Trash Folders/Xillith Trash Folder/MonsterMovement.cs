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
    private int CurrentStep = 0;

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
            NextStep = GetNextStep();
            if (NextStep == (int)DirectionMoved.NONE)
            {
                SetLookDirection();
                return;
            }

            SetNextLocation(NextStep);
            FacedDirection = NextStep;
            if (IsMoveLocationPassable() && IsLocationEntityFree())
            {
                UpdateNewEntityGridLocation();
                RemoveOldEntityGridLocation();
                CurrentlyMoving = true;
            }
            else
            {
                GameObject EnemyToFight = null;
                EnemyToFight = isThereAPlayer();
                if (EnemyToFight != null)
                {
                    //Combat Combat = new Combat();
                    Combat.initiateFight(EnemyToFight, this.gameObject);
                    //InitiateFight();
                    //SceneManager.LoadScene("Combat Scene", LoadSceneMode.Additive);

                }
            }
        }

        if (CurrentlyMoving == true)
        {
            float finishedMoving = MoveToNextSquare();
            if (finishedMoving == 0)
                CurrentlyMoving = false;
        }


    }

    

    private int GetNextStep()
    {
        int nexstp=0;
        if (PathViaSteps) {
            if (CurrentStep == Pathing.Length)
                CurrentStep = 0;
            nexstp = Pathing[CurrentStep];
            CurrentStep++;
        }
        if (PathRandomly) {
            System.Random rand = new System.Random();
            int direction = rand.Next(4) + 1;
            //CanIMoveRandomly(direction);
        }
        
        return nexstp;
    }


}
