using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpriteMovement;

public class CharacterInputController : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterMovement characterController;
    EntityData characterEntityData;
    private bool moveable;
    private bool waitFrameAfterDialogue;
    private int rightKeyFrame;
    private int leftKeyFrame;
    private int upKeyFrame;
    private int downKeyFrame;
    private bool rightKeyPressedLast;
    private bool leftKeyPressedLast;
    private bool upKeyPressedLast;
    private bool downKeyPressedLast;

    void Start()
    {
        characterController = this.gameObject.GetComponent<CharacterMovement>();
        characterEntityData = this.gameObject.GetComponent<EntityData>();
        downKeyFrame = 0;
        upKeyFrame = 0;
        rightKeyFrame = 0;
        leftKeyFrame = 0;

        if (characterEntityData.isMainCharacter) moveable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.isInDialogue) { waitFrameAfterDialogue = true; }

        if (!moveable || GameState.getFullPauseStatus() || GameData.Instance.isInDialogue) { return; }

        if (waitFrameAfterDialogue)
        {
            waitFrameAfterDialogue = false;
            return;
        }


        if (FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE))
        {
            characterController.ActivateKeyReceived();

        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.ROTATE_LEFT))
        {
            characterController.PowerToggleLeftKeyReceived();
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.ROTATE_RIGHT))
        {
            characterController.PowerToggleRightKeyReceived();
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.USE_POWER))
        {
            characterController.PowerActivateKeyReceived();
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.POWER_0))
        {
            characterController.SwitchToPowerKeyRecieved(0);
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.POWER_1))
        {
            characterController.SwitchToPowerKeyRecieved(1);
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.POWER_2))
        {
            characterController.SwitchToPowerKeyRecieved(2);
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.POWER_3))
        {
            characterController.SwitchToPowerKeyRecieved(3);
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.POWER_4))
        {
            characterController.SwitchToPowerKeyRecieved(4);
        }


#if UNITY_EDITOR

        if (Input.GetButtonDown("ToggleUICamera"))
        {
            if (GameData.Instance.UI_On)
            {
                GameData.Instance.ManualUIToggleOff = true;
                characterController.uiController.turnOffUi();
            }
            else
            {
                GameData.Instance.ManualUIToggleOff = false;
                characterController.uiController.turnOnUi();
            }
        }


        if (Input.GetButtonDown("MurderPlayer"))
        {
            characterController.MurderPlayer();
        }
        if (Input.GetButtonDown("PowerUp"))
        {
            characterController.PowerUpCheat();
        }
        if (Input.GetButtonDown("PowerDown"))
        {
            characterController.PowerDownCheat();
        }
#endif

        if (FWInputManager.Instance.GetKeyDown(InputAction.REST))
        {
            characterController.AttemptRest();
        }
        characterController.MoveKeyReceived(GetInputDirection());



    }

    private DirectionMoved GetInputDirection()
    {

        DirectionMoved NextInputDirection = DirectionMoved.NONE;

        if (FWInputManager.Instance.GetKeyDown(InputAction.RIGHT))
        {
            rightKeyFrame = Time.frameCount;
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.LEFT))
        {
            leftKeyFrame = Time.frameCount;
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.UP))
        {
            upKeyFrame = Time.frameCount;
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.DOWN))
        {
            downKeyFrame = Time.frameCount;
        }

        if (FWInputManager.Instance.GetKeyUp(InputAction.RIGHT))
        {
            rightKeyFrame = 0;
        }
        if (FWInputManager.Instance.GetKeyUp(InputAction.LEFT))
        {
            leftKeyFrame = 0;
        }
        if (FWInputManager.Instance.GetKeyUp(InputAction.UP))
        {
            upKeyFrame = 0;
        }
        if (FWInputManager.Instance.GetKeyUp(InputAction.DOWN))
        {
            downKeyFrame = 0;
        }

        setMostRecentDirectionPushed();

        if (FWInputManager.Instance.GetKey(InputAction.RIGHT) && rightKeyPressedLast)
        {
            NextInputDirection = DirectionMoved.RIGHT;            
        }
        if (FWInputManager.Instance.GetKey(InputAction.LEFT) && leftKeyPressedLast)
        {
            NextInputDirection = DirectionMoved.LEFT;
        }
        if (FWInputManager.Instance.GetKey(InputAction.UP) && upKeyPressedLast)
        {
            NextInputDirection = DirectionMoved.UP;
        }
        if (FWInputManager.Instance.GetKey(InputAction.DOWN) && downKeyPressedLast)
        {
            NextInputDirection = DirectionMoved.DOWN;
        }
        return NextInputDirection;
        //ToDo Have a variable for key last pressed and use that one. Use timestamps. reset timestamps if key not pressed. 
    }

    private void setMostRecentDirectionPushed()
    {
        downKeyPressedLast = true;


        if (rightKeyFrame > leftKeyFrame && rightKeyFrame > upKeyFrame && rightKeyFrame > downKeyFrame) {
            rightKeyPressedLast = true;
            leftKeyPressedLast = false;
            upKeyPressedLast = false;
            downKeyPressedLast = false;
        }
        if (leftKeyFrame > rightKeyFrame && leftKeyFrame > upKeyFrame && leftKeyFrame > downKeyFrame) {
            rightKeyPressedLast = false;
            leftKeyPressedLast = true;
            upKeyPressedLast = false;
            downKeyPressedLast = false;
        }
        if (upKeyFrame > rightKeyFrame && upKeyFrame > leftKeyFrame && upKeyFrame > downKeyFrame)
        {
            rightKeyPressedLast = false;
            leftKeyPressedLast = false;
            upKeyPressedLast = true;
            downKeyPressedLast = false;
        }
        if (downKeyFrame > rightKeyFrame && downKeyFrame > leftKeyFrame && downKeyFrame > upKeyFrame)
        {
            rightKeyPressedLast = false;
            leftKeyPressedLast = false;
            upKeyPressedLast = false;
            downKeyPressedLast = true;
        }



    }
}
