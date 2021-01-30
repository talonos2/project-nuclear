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

    void Start()
    {
        characterController = this.gameObject.GetComponent<CharacterMovement>();
        characterEntityData = this.gameObject.GetComponent<EntityData>();


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

        if (FWInputManager.Instance.GetKey(InputAction.RIGHT))
        {
            NextInputDirection = DirectionMoved.RIGHT;
        }
        if (FWInputManager.Instance.GetKey(InputAction.LEFT))
        {
            NextInputDirection = DirectionMoved.LEFT;
        }
        if (FWInputManager.Instance.GetKey(InputAction.UP))
        {
            NextInputDirection = DirectionMoved.UP;
        }
        if (FWInputManager.Instance.GetKey(InputAction.DOWN))
        {
            NextInputDirection = DirectionMoved.DOWN;
        }
        return NextInputDirection;
        //ToDo Have a variable for key last pressed and use that one. Use timestamps. reset timestamps if key not pressed. 
    }



}
