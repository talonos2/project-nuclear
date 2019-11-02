using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike2EntityData : SpikeController
{

    private int totalFrames = 7;
    void Update()
    {

        if (!isAnimating || GameState.isInBattle) { return; }
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= 1 / AnimationSpeed)
        {
            frameNumber += 1;
            if (animateRise) sRender.material.SetInt("_Frame", totalFrames - frameNumber-1) ;
                else sRender.material.SetInt("_Frame", frameNumber);
            timeSinceLastFrame = 0;
            if (frameNumber == totalFrames - 1) { isAnimating = false;
                frameNumber = 0;
            }
        }
    }
}