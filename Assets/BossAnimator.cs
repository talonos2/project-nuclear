using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public int totalFrames;
    public float framesPerSecond;
    public bool fireBoss;
    private float frameCounter = 0;
    private int currentFrame=0;
    protected Renderer sRender;

    void Start()
    {
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
       
}

    // Update is called once per frame
    void Update()
    {
        frameCounter += Time.deltaTime;
        if (frameCounter > 1 / framesPerSecond)
        {
            currentFrame += 1;
            if (currentFrame >= totalFrames) {
                currentFrame = 0;
                if (fireBoss) currentFrame = 1;
            }
                

            sRender.material.SetInt("_Frame", currentFrame);
            frameCounter = 0;
        }
    }
}
