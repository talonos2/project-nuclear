using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWindPusher : ObjectAnimator
{

    
    override public void AnimateObject()
    {
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= (1f / framesPerSecond))
        {
            timeSinceLastFrame = 0;
            currentFrame += 1;
            if (currentFrame == maxFrames)
            {
                currentFrame = 0;
                GameObject.Destroy(this);
                return;
            }
            sRender.material.SetFloat("_Frame", currentFrame + offsetFix);
        }


    }

}
