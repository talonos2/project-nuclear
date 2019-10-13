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
    void Start()
    {
        characterController = this.gameObject.GetComponent<CharacterMovement>();
        characterEntityData=this.gameObject.GetComponent<EntityData>();
        if (characterEntityData.isMainCharacter) moveable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveable) { return; }

        if (Input.GetButtonDown("Submit")){
            characterController.ActivateKeyReceived();
        }
        if (Input.GetButtonDown("PowerToggleLeft")) {
            characterController.PowerToggleLeftKeyReceived();
        }
        if (Input.GetButtonDown("PowerToggleRight"))
        {
            characterController.PowerToggleRightKeyReceived();
        }
        if (Input.GetButtonDown("PowerActivate")) {
            characterController.PowerActivateKeyReceived();
        }
        if (Input.GetButtonDown("PowerUp")) {
            characterController.PowerUpCheat();
        }
        if (Input.GetButtonDown("PowerDown"))
        {
            characterController.PowerDownCheat();
        }
        characterController.MoveKeyReceived(GetInputDirection());
       
    }

    private DirectionMoved GetInputDirection()
    {

        DirectionMoved NextInputDirection = DirectionMoved.NONE;

        if (Input.GetAxisRaw("Horizontal") > .1)
        {
            NextInputDirection = DirectionMoved.RIGHT;
        }
        if (Input.GetAxisRaw("Horizontal") < -.1)
        {
            NextInputDirection = DirectionMoved.LEFT;
        }
        if (Input.GetAxisRaw("Vertical") > .1)
        {
            NextInputDirection = DirectionMoved.UP;
        }
        if (Input.GetAxisRaw("Vertical") < -.1)
        {
            NextInputDirection = DirectionMoved.DOWN;
        }
        return NextInputDirection;
        //ToDo Have a variable for key last pressed and use that one. Use timestamps. reset timestamps if key not pressed. 
    }



}
