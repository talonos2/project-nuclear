using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpriteMovement;

public class CharacterInputController : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterMovement characterToMove;
    EntityData characterEntityData;
    private bool moveable;
    void Start()
    {
        characterToMove = this.gameObject.GetComponent<CharacterMovement>();
        characterEntityData=this.gameObject.GetComponent<EntityData>();
        if (characterEntityData.isMainCharacter) moveable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveable) { return; }

        if (Input.GetButtonDown("Submit")){
            characterToMove.ActivateKeyReceived();
        }
        characterToMove.MoveKeyReceived(GetInputDirection());
       
    }

    private int GetInputDirection()
    {

        int NextInputDirection = -1;

        if (Input.GetAxisRaw("Horizontal") > .1)
        {
            NextInputDirection = (int)DirectionMoved.RIGHT;
        }
        if (Input.GetAxisRaw("Horizontal") < -.1)
        {
            NextInputDirection = (int)DirectionMoved.LEFT;
        }
        if (Input.GetAxisRaw("Vertical") > .1)
        {
            NextInputDirection = (int)DirectionMoved.UP;
        }
        if (Input.GetAxisRaw("Vertical") < -.1)
        {
            NextInputDirection = (int)DirectionMoved.DOWN;
        }

        if (NextInputDirection == -1)
        {
            NextInputDirection = (int)DirectionMoved.NONE;
        }
        return NextInputDirection;
        //ToDo Have a variable for key last pressed and use that one. Use timestamps. reset timestamps if key not pressed. 
    }



}
