using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FYI: The wind jump is controlled in CharacterMovement. This "Controller" just stores some data. :P
public class WindJumpController : DoodadData
{
    // Variables used in Character Movement script
    public Vector2Int jumpDestOffset;
    public float timeItTakesToJump = 2;
    public bool hiddenJumpter = false;
    public float jumpHeight = 1;

    public void EnableWindJumper() {
        hiddenJumpter = false;
        isWindShifter = true;
        foreach (MeshRenderer childObj in this.transform.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            childObj.enabled = true;
        }

    }



}
