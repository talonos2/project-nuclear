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
    private int[] Pathing= {3,3,3,1,1,1, 2, 2,2, 0,0,0 };
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
           
        

        if (CurrentlyMoving == true)
        {
            float finishedMoving = MoveToNextSquare();
            if (finishedMoving == 0)
                CurrentlyMoving = false;
        }

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
            else if (isThereAPlayer())
            {

                //InitiateCombat();
                SceneManager.LoadScene("Combat Scene", LoadSceneMode.Additive);

            }
        }


        }

    

    private int GetNextStep()
    {
        if (CurrentStep == 12)
            CurrentStep = 0;
        int nexstp= Pathing[CurrentStep];
        CurrentStep++;
        return nexstp;
    }
}
