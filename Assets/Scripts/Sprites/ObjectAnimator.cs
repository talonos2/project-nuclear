using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimator : MonoBehaviour
{
    protected Renderer sRender;
    public int maxFrames;
    public float framesPerSecond = 6;
    protected float timeSinceLastFrame = 0;
    protected float offsetFix = .00001f;
    protected int currentFrame = 0;
    void Start()
    {
        this.sRender = this.GetComponentInChildren<MeshRenderer>();
        this.sRender.material = new Material(this.sRender.material);
        sRender.material.SetFloat("_Frame", currentFrame + offsetFix);
    }

    // Update is called once per frame
    public virtual void AnimateObject()
    {
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= (1f / framesPerSecond))
        {
            timeSinceLastFrame = 0;
            currentFrame += 1;
            if (currentFrame == maxFrames) {
                currentFrame = 0;
            }
            sRender.material.SetFloat("_Frame", currentFrame + offsetFix);
        }


    }

    void Update()
    {
        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }
        AnimateObject();
    }
}
