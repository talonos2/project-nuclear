using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PawnMover : SpriteMovement
{

    Queue<MovementWrapper> movementQueue = new Queue<MovementWrapper>();

    // Update is called once per frame
    void Update()
    {
        if (gameData.Paused == true)
        {
            return;
        }

        //If in the process of moving, keep moving and do nothing else

        if (currentlyMoving)
        {
            float finishedMoving = ContinueMoving();
            if (finishedMoving == 0)
            {
                //Debug.Log("Done moving!");
                currentlyMoving = false;
                SetCurrentLocation();
            }
            else
            {
                //Debug.Log("Already Moving!");
            }
        }
        else
        {
            if (movementQueue.Count > 0)
            {
                MovementWrapper move = movementQueue.Dequeue();
                switch (move.t)
                {
                    case Task.MOVE:
                        MoveKeyReceived(move.d);
                        break;
                    case Task.TURN:
                        TurnKeyReceived(move.d);
                        break;
                }
            }
            else
            {
                //Debug.Log("No direction to move to!");
            }
        }
    }

    internal void EnqueueTurn(string direction)
    {
        DirectionMoved d = GetDirectionFromString(direction);
        direction = direction.ToLower();
        movementQueue.Enqueue(new MovementWrapper(d, Task.TURN));
    }

    //Key command received from CharacterInputController script
    private void MoveKeyReceived(DirectionMoved inputDirection)
    {
        if (inputDirection == (int)DirectionMoved.NONE)
        {
            SetLookDirection();
            return;
        }

        facedDirection = inputDirection;
        SetNextLocation(inputDirection);
        if (IsPlayerMoveLocationPassable(characterNextLocation.x, characterNextLocation.y))
        {
            UpdateNewEntityGridLocation();
            RemoveOldEntityGridLocation();
            characterLocation = characterNextLocation;
            currentlyMoving = true;

        }
    }

    //Key command received from CharacterInputController script
    private void TurnKeyReceived(DirectionMoved inputDirection)
    {
        if (inputDirection != (int)DirectionMoved.NONE)
        {
            facedDirection = inputDirection;
            SetLookDirection();
            return;
        }
    }

    private float ContinueMoving()
    {
        float finishedMoving = 0;
        if (facedDirection == DirectionMoved.UP)
        {
            finishedMoving = MoveUp(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(MoveSpeed);
        }
        return finishedMoving;
    }

    public void EnqueueMovement(String direction)
    {
        DirectionMoved d = GetDirectionFromString(direction);
        direction = direction.ToLower();
        movementQueue.Enqueue(new MovementWrapper(d, Task.MOVE));
    }

    private DirectionMoved GetDirectionFromString(string direction)
    {
        switch (direction)
        {
            case "up":
                return DirectionMoved.UP;
            case "u":
                return DirectionMoved.UP;
            case "north":
                return DirectionMoved.UP;
            case "n":
                return DirectionMoved.UP;
            case "down":
                return DirectionMoved.DOWN;
            case "d":
                return DirectionMoved.DOWN;
            case "south":
                return DirectionMoved.DOWN;
            case "s":
                return DirectionMoved.DOWN;
            case "left":
                return DirectionMoved.LEFT;
            case "l":
                return DirectionMoved.LEFT;
            case "west":
                return DirectionMoved.LEFT;
            case "w":
                return DirectionMoved.LEFT;
            case "right":
                return DirectionMoved.RIGHT;
            case "r":
                return DirectionMoved.RIGHT;
            case "east":
                return DirectionMoved.RIGHT;
            case "e":
                return DirectionMoved.RIGHT;
            default:
                Debug.LogWarning("Unrecognized movement direction: " + direction + " Passed in via the MovePawn command.");
                return DirectionMoved.NONE;
                break;
        }
    }

    internal struct MovementWrapper
    {
        internal readonly DirectionMoved d;
        internal readonly Task t;

        public MovementWrapper(DirectionMoved d, Task t) : this()
        {
            this.d = d;
            this.t = t;
        }
    }

    internal enum Task {MOVE,TURN};
}
