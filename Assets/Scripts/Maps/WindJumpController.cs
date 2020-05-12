﻿using System.Collections;
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
        foreach (MeshRenderer childObj in this.transform.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            childObj.enabled = true;
        }

    }



}
