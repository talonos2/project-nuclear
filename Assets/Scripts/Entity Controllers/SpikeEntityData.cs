using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEntityData : SpikeController
{

    private int totalFrames = 8;
    private float offsetFix = .00001f;

    void Update()
    {
        if (!isAnimating|| GameState.isInBattle) { return; }
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= 1 / AnimationSpeed)
        {
            frameNumber += 1;
            if (animateRise) sRender.material.SetFloat("_Frame", totalFrames - frameNumber - 1+ offsetFix);
            else sRender.material.SetFloat("_Frame", frameNumber+ offsetFix);
            timeSinceLastFrame = 0;
            if (frameNumber == totalFrames - 1) { isAnimating = false;
                frameNumber = 0;
            }
        }
    }

}
