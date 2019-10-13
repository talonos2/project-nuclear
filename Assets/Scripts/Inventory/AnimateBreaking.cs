using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBreaking : MonoBehaviour
{
    // Start is called before the first frame update
    private Renderer sRender;
    public int maxFrames;
    public int framesPerSecond = 6;
    private float timeSinceLastFrame = 0;
    public int currentFrame = 0;
    void Start()
    {
        this.sRender = this.GetComponentInChildren<MeshRenderer>();
        this.sRender.material = new Material(this.sRender.material);
        sRender.material.SetInt("_Frame", currentFrame);
    }

    // Update is called once per frame
    public void AnimateBreak() {
        if (currentFrame == maxFrames - 1) return;
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= (1f / framesPerSecond))
        {
            timeSinceLastFrame = 0;
                currentFrame += 1;
        }
        sRender.material.SetInt("_Frame", currentFrame);

    }

    void Update()
    {
        AnimateBreak();
    }
}
