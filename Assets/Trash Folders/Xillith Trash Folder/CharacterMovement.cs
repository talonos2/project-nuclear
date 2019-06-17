using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : SpriteMovement
{
    // Start is called before the first frame update



    
    private int MovedDirection = (int)DirectionMoved.LEFT;
    
    private bool CurrentlyMoving = false;
 



    // Update is called once per frame
    void Update()
    {
        float directionChangeHorizontal = 0;
        float directionChangeVertical = 0;
        float finishedMoving = 0;


        //
        if (!CurrentlyMoving && (MovedDirection == (int)DirectionMoved.RIGHT || MovedDirection == (int)DirectionMoved.LEFT))
        {
            //Get next input if character isn't currently moving

            if (Input.GetAxisRaw("Horizontal") > .1)
            {
                directionChangeHorizontal = 1;
                MovedDirection = (int)DirectionMoved.RIGHT;
                CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Horizontal") < -.1)
            {
                directionChangeHorizontal = -1;
                MovedDirection = (int)DirectionMoved.LEFT;
                CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Vertical") > .1)
            {
                directionChangeVertical = 1;
                MovedDirection = (int)DirectionMoved.UP;
                CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Vertical") < -.1)
            {
                directionChangeVertical = -1;
                MovedDirection = (int)DirectionMoved.DOWN;
                CurrentlyMoving = true;
            }

        }

        if (!CurrentlyMoving)
        {
            //Get next input if character isn't currently moving          
            if (Input.GetAxisRaw("Vertical") > .1)
            {
                directionChangeVertical = 1;
                MovedDirection = (int)DirectionMoved.UP;
                CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Vertical") < -.1)
            {
                directionChangeVertical = -1;
                MovedDirection = (int)DirectionMoved.DOWN;
                CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Horizontal") > .1)
            {
                directionChangeHorizontal = 1;
                MovedDirection = (int)DirectionMoved.RIGHT;
                CurrentlyMoving = true;
            }
            if (Input.GetAxisRaw("Horizontal") < -.1)
            {
                directionChangeHorizontal = -1;
                MovedDirection = (int)DirectionMoved.LEFT;
                CurrentlyMoving = true;
            }

        }


        if (!CurrentlyMoving)
        {
            //replace sprite to be the sprite number of holding still based on direction last moved, 
            //Assuming that you haven't moved at all recently.
            if (MovedDirection == (int)DirectionMoved.DOWN)
                FaceDown();
            if (MovedDirection == (int)DirectionMoved.UP)
                FaceUp();
            if (MovedDirection == (int)DirectionMoved.LEFT)
                FaceLeft();
            if (MovedDirection == (int)DirectionMoved.RIGHT)
                FaceRight();
        }

        if (CurrentlyMoving) {
            if (MovedDirection == (int)DirectionMoved.UP)
            {
                finishedMoving = MoveUp(MoveSpeed);
                if (finishedMoving == 0)
                    CurrentlyMoving = false;
            }
            if (MovedDirection == (int)DirectionMoved.DOWN)
            {
                finishedMoving = MoveDown(MoveSpeed);
                if (finishedMoving == 0)
                    CurrentlyMoving = false;
            }
            if (MovedDirection == (int)DirectionMoved.LEFT)
            {
                finishedMoving = MoveLeft(MoveSpeed);
                if (finishedMoving == 0)
                    CurrentlyMoving = false;
            }
            if (MovedDirection == (int)DirectionMoved.RIGHT)
            {
                finishedMoving = MoveRight(MoveSpeed);
                if (finishedMoving == 0)
                    CurrentlyMoving = false;
            }

        }



    }
}
