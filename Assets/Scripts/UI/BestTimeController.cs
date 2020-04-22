using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestTimeController : MonoBehaviour
{
    public TextMeshProUGUI[] bestTimesTexts;
    public Image[] bestTimesMarkers;
    public Image bestTimeSlider;


    public float delay;
    public float duration;

    public float startTimeOffset;
    public float endTimeOffset;
    public float y;

    public int floorsDroppedOff;

    public FloorDropoffPrefab floorDropoffPrefab;

    private float timeSoFar;
    // Start is called before the first frame update
    void Start()
    {
        //Fill the thing with debug data if none exists. This is TEST CODE:
        if (GameData.Instance.timer==0)
        {
            //Died after 0 seconds? Should be impossible.
            Debug.LogError("WARNING: Either you're testing the BestTimeController by running the end run screen directly, or something had gone very wrong!");
            GameData.Instance.timesThisRun = new []{ 12,22,32, 50, 93.4f, 95.2f, 195.3f, 210,250,300,350,475,550,0,0,0,0,0,0,0};
        }
        //End test code.

        bestTimeSlider.transform.localPosition = new Vector3(startTimeOffset, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timeSoFar += Time.deltaTime;
        if (timeSoFar>delay)
        {
            if (timeSoFar < delay + duration)
            {
                float t = (timeSoFar - delay) / duration;
                bestTimeSlider.transform.localPosition = new Vector3(Mathf.Lerp(startTimeOffset, endTimeOffset, t), y, 0);
                float quickenedTimeTakenSoFar = (timeSoFar - delay) * (600 / duration);
                if (quickenedTimeTakenSoFar >= GameData.Instance.timesThisRun[floorsDroppedOff]&& GameData.Instance.timesThisRun[floorsDroppedOff]!=0)
                {
                    float tForDropoff = GameData.Instance.timesThisRun[floorsDroppedOff] / 600f;
                    float dropoffx = Mathf.Lerp(startTimeOffset, endTimeOffset, tForDropoff);
                    DropOffFloor(floorsDroppedOff, dropoffx);
                    floorsDroppedOff++;
                }
                
            }
        }
    }

    private void DropOffFloor(int floorsDroppedOff, float dropoffx)
    {
        Debug.Log("Floor dropped off!");
        FloorDropoffPrefab dropped = GameObject.Instantiate<FloorDropoffPrefab>(floorDropoffPrefab);
        dropped.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform>());
        dropped.GetComponent<RectTransform>().localPosition = new Vector3(dropoffx, y, 0);
        float timeTaken = (floorsDroppedOff == 0 ? GameData.Instance.timesThisRun[floorsDroppedOff] : GameData.Instance.timesThisRun[floorsDroppedOff] - GameData.Instance.timesThisRun[floorsDroppedOff - 1]);
        dropped.Initialize(floorsDroppedOff+1, timeTaken, timeTaken==GameData.Instance.bestTimes[floorsDroppedOff]);
    }
}
