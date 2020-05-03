﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraClamp : MonoBehaviour
{
    private float mapHeight;
    private float mapWidth;
    private GameObject ThePlayer;
    // Start is called before the first frame update
    void Start()
    {
        PassabilityGrid gridy = GameObject.Find("Grid").GetComponent<PassabilityGrid>();
        mapHeight = gridy.height;
        mapWidth = gridy.width;
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log("map zero " + mapZeroLocation + " playerloc " + ThePlayer.transform.position);
       // float newXLocation = ThePlayer.transform.position.x - mapZeroLocation.x -8f;
        //float newYlocation = ThePlayer.transform.position.y - mapZeroLocation.y - 5.5f;

       // Debug.Log("New Locations (" + newXLocation + ", " + newYlocation);
        //this.transform.position = new Vector3(ThePlayer.transform.position.x- mapZeroLocation.x-,0,this.transform.position.z);


    }

    // Update is called once per frame
    void Update()
    {
        // float relativeX = mapZeroLocation.x - ThePlayer.transform.position.x - 8;

        if (GameData.Instance.FloorNumber == 0) return;

        float relativeYLowClamp = -mapHeight / 2 - ThePlayer.transform.position.y + 6f;
        float relativeYHighClamp = mapHeight / 2- ThePlayer.transform.position.y - 6f;
        float relativeXLowClamp = -mapWidth / 2 - ThePlayer.transform.position.x + 10.67f;
        float relativeXHighClamp = mapWidth / 2 - ThePlayer.transform.position.x - 10.67f;
        float offsetY = 0;
        float offsetX = 0;
        //Debug.Log(relativeY);
        if (relativeYLowClamp > 0)
        {
            offsetY += relativeYLowClamp;
        }
        if (relativeYHighClamp < 0) {
            offsetY +=  relativeYHighClamp;
        }
        if (relativeXLowClamp > 0)
        {
            offsetX += relativeXLowClamp;
        }
        if (relativeXHighClamp < 0)
        {
            offsetX += relativeXHighClamp;
        }

        this.transform.localPosition = new Vector3(offsetX,offsetY,this.transform.localPosition.z);
    }
}