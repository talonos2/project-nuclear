using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike2EntityData : SpikeController
{

    private int totalFrames = 7;
    private float offsetFix = .00001f;
    new void Update()
    {
        base.Update();
        if (!isAnimating || GameState.isInBattle || GameState.getFullPauseStatus()) { return; }
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= 1 / AnimationSpeed)
        {
            frameNumber += 1;
            if (animateRise) sRender.material.SetFloat("_Frame", totalFrames - frameNumber-1+ offsetFix) ;
                else sRender.material.SetFloat("_Frame", frameNumber+ offsetFix);
            timeSinceLastFrame = 0;
            if (frameNumber == totalFrames - 1) { isAnimating = false;
                frameNumber = 0;
                if (!animateRise) this.sRender.material.SetInt("_HasEmissive", 0);
            }
        }
    }
}