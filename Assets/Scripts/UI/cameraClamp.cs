using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraClamp : MonoBehaviour
{
    private float mapHeight;
    private float mapWidth;
    private GameObject ThePlayer;
    private Vector2 groundOffset;
    // Start is called before the first frame update
    void Start()
    {
        PassabilityGrid gridy = GameObject.Find("Grid").GetComponent<PassabilityGrid>();
        mapHeight = gridy.height;
        mapWidth = gridy.width;
        groundOffset = gridy.GetComponentInChildren<GroundShadow>().mapOffset;
        Debug.Log(groundOffset);
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

        if (GameData.Instance.FloorNumber == 0) {
            this.transform.localPosition = new Vector3(groundOffset.x, groundOffset.y, this.transform.localPosition.z);
            return;
        }
        

        float relativeYLowClamp = -mapHeight / 2 - ThePlayer.transform.position.y + 6f;
        float relativeYHighClamp = mapHeight / 2- ThePlayer.transform.position.y - 6f;
        float relativeXLowClamp = -mapWidth / 2 - ThePlayer.transform.position.x + 10.67f;
        float relativeXHighClamp = mapWidth / 2 - ThePlayer.transform.position.x - 10.67f;
        float offsetY = 0;
        float offsetX = 0;
        //Debug.Log(relativeY);
        if (relativeYLowClamp > 0+ groundOffset.y)
        {
            offsetY += relativeYLowClamp - groundOffset.y;

        }
        if (relativeYHighClamp < 0 + groundOffset.y) {
            offsetY +=  relativeYHighClamp - groundOffset.y;

        }
        if (relativeXLowClamp  > 0 + groundOffset.x)
        {
            offsetX += relativeXLowClamp - groundOffset.x;

        }
        if (relativeXHighClamp < 0 + groundOffset.x)
        {
            offsetX += relativeXHighClamp - groundOffset.x;

        }
        Debug.Log("offsetx "+offsetX + " " + groundOffset.x);
        Debug.Log("offsety " + offsetY + " " + groundOffset.y);

        this.transform.localPosition = new Vector3(offsetX+ groundOffset.x, offsetY+ groundOffset.y, this.transform.localPosition.z);
    }
}
