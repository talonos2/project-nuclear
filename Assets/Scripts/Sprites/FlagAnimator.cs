using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagAnimator : ObjectAnimator
{

    private float maxRandomDelay=2;
    private float minDelay = .25f;
    private bool startingFrame = true;
    override public void AnimateObject()
    {
        if (startingFrame) {
            timeSinceLastFrame -= Random.Range(minDelay, maxRandomDelay);
            startingFrame = false;
        }
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= (1f / framesPerSecond))
        {
            timeSinceLastFrame = 0;
            currentFrame += 1;
            if (currentFrame == maxFrames)
            {
                currentFrame = 0;
                timeSinceLastFrame -= Random.Range(minDelay, maxRandomDelay);
            }
            sRender.material.SetFloat("_Frame", currentFrame + offsetFix);
        }


    }
}
