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
        if (NextStep == (int)DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(MoveSpeed);
            if (finishedMoving == 0)
                CurrentlyMoving = false;
        }
        if (NextStep == (int)DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(MoveSpeed);
            if (finishedMoving == 0)
                CurrentlyMoving = false;
        }
        if (NextStep == (int)DirectionMoved.UP)
        {
            finishedMoving = MoveUp(MoveSpeed);
            if (finishedMoving == 0)
                CurrentlyMoving = false;
        }
        if (NextStep == (int)DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(MoveSpeed);
            if (finishedMoving == 0)
                CurrentlyMoving = false;
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
