using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    enum DirectionMoved { UP, DOWN, LEFT, RIGHT }

    private float MovedSoFar = 0;
    private int MovedDirection = (int)DirectionMoved.LEFT;
    
    

    void Update()
    {
        float directionChangeHorizontal = 0;
        float directionChangeVertical = 0;


        if (Input.GetAxis("Vertical") > 0)
        {
            directionChangeVertical = 1;

        }
        if (Input.GetAxis("Vertical") < 0)
        {
            directionChangeVertical = -1;

        }

        if (Input.GetAxis("Horizontal")>0) {
            directionChangeHorizontal = 1;

        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            directionChangeHorizontal = -1;

        }
        Vector3 getHorizontalMotion = new Vector3(directionChangeHorizontal, 0.0f, 0.0f);
        Vector3 getVerticalMotion = new Vector3(0.0f, directionChangeVertical, 0.0f);

        Debug.Log(directionChangeHorizontal+" "+ directionChangeVertical);
        if (directionChangeHorizontal != 0) transform.position = transform.position + getHorizontalMotion*Time.deltaTime * 2.2f;
        if (directionChangeVertical != 0) transform.position = transform.position + getVerticalMotion * Time.deltaTime * 2.2f;


    }
}
