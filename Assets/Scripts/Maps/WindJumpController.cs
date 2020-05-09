using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindJumpController : DoodadData
{
    // Variables used in Character Movement script
    public Vector2Int jumpDestOffset;
    public float jumpSpeedMultiplier = 2;
    public bool hiddenJumpter = false;

    public void EnableWindJumper() {
        hiddenJumpter = false;
        isWindShifter = true;
        foreach (GameObject childObj in this.transform){
            childObj.GetComponent<MeshRenderer>().enabled = true;
        }

    }



}
