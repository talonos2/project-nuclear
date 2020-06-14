using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrailMover : MonoBehaviour
{
    public SpriteMovement.DirectionMoved[] path;
    public float speed = 20;

    private float t;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int intT = (int)t;
        float remainder = t % 1.0f;
        Vector3 targetPos = Vector3.zero;
        for (int x = 0; x < intT; x++)
        {
            targetPos += path[x].GetDirectionVector();
        }

        //Debug.Log(intT);
        if (t >= path.Length)
        {
            ParticleSystem.EmissionModule e = GetComponent<ParticleSystem>().emission;
            e.enabled = false;
            GameObject.Destroy(this);
            return;
        }
        targetPos += path[intT].GetDirectionVector() * remainder;

        this.transform.position = startPos + targetPos;

        t += speed * Time.deltaTime;
       
    }

    internal void InitStart()
    {
        this.startPos = this.transform.position + Vector3.up * -.75f;
    }
}
