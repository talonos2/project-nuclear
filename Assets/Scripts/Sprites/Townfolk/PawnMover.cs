using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PawnMover : SpriteMovement
{

    public Queue<DirectionMoved> movementQueue = new Queue<DirectionMoved>();

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
                //Debug.Log("Moving!");
                MoveKeyReceived(movementQueue.Dequeue());
            }
            else
            {
                //Debug.Log("No direction to move to!");
            }
        }
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

    private float ContinueMoving()
    {
        float finishedMoving=0;
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
        direction = direction.ToLower();
        switch (direction)
        {
            case "up":
                movementQueue.Enqueue(DirectionMoved.UP);
                break;
            case "u":
                movementQueue.Enqueue(DirectionMoved.UP);
                break;
            case "north":
                movementQueue.Enqueue(DirectionMoved.UP);
                break;
            case "n":
                movementQueue.Enqueue(DirectionMoved.UP);
                break;
            case "down":
                movementQueue.Enqueue(DirectionMoved.DOWN);
                break;
            case "d":
                movementQueue.Enqueue(DirectionMoved.DOWN);
                break;
            case "south":
                movementQueue.Enqueue(DirectionMoved.DOWN);
                break;
            case "s":
                movementQueue.Enqueue(DirectionMoved.DOWN);
                break;
            case "left":
                movementQueue.Enqueue(DirectionMoved.LEFT);
                break;
            case "l":
                movementQueue.Enqueue(DirectionMoved.LEFT);
                break;
            case "west":
                movementQueue.Enqueue(DirectionMoved.LEFT);
                break;
            case "w":
                movementQueue.Enqueue(DirectionMoved.LEFT);
                break;
            case "right":
                movementQueue.Enqueue(DirectionMoved.RIGHT);
                break;
            case "r":
                movementQueue.Enqueue(DirectionMoved.RIGHT);
                break;
            case "east":
                movementQueue.Enqueue(DirectionMoved.RIGHT);
                break;
            case "e":
                movementQueue.Enqueue(DirectionMoved.RIGHT);
                break;
            default:
                Debug.LogWarning("Unrecognized movement direction: "+direction+" Passed in via the MovePawn command.");
                break;
        }
    }
}
