using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : SpriteMovement
{
    // Start is called before the first frame update

    private bool CurrentlyMoving = false;
    private int NextStep;
    private float finishedMoving;
    private int[] Pathing= {3,3,3,1,1,1, 2, 2,2, 0,0,0 };
    private int CurrentStep = 0;

    // Update is called once per frame
    void Update()
    {
        if (!CurrentlyMoving) {
           NextStep= GetNextStep();
           CurrentlyMoving = true;
        }

        if (CurrentlyMoving == true)
        {
            float finishedMoving = MoveToNextStep();
            if (finishedMoving == 0)
                CurrentlyMoving = false;
        }

        
    }

    private float MoveToNextStep()
    {

        float finishedMoving = 0;
        if (NextStep == (int)DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(MoveSpeed);
        }
        if (NextStep == (int)DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(MoveSpeed);
        }
        if (NextStep == (int)DirectionMoved.UP)
        {
            finishedMoving = MoveUp(MoveSpeed);
        }
        if (NextStep == (int)DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(MoveSpeed);
        }
        return finishedMoving;
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
