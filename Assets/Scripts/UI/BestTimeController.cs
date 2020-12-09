using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestTimeController : MonoBehaviour
{
    public Image[] bestTimesMarkers;
    public Image bestTimeSlider;
    public Image skull;
    public RectTransform containerToShakeOnSkullDrop;

    public TextMeshProUGUI bestTimeText;


    public float delay;
    public float duration;

    public float startTimeOffset;
    public float endTimeOffset;
    public float y;
    public float maxYBend=20;
    public float curveStrength = 0;

    public int floorsDroppedOff;

    public FloorDropoffPrefab floorDropoffPrefab;

    private float timeSoFar;
    private bool deathSequencePlaying;

    public float durationOfDeathEffect = .7f;

    public float intensityOfDeathShake = 100f;
    public float intensityOfScreenShake = 20f;

    private Vector3 deathPosition;
    private Vector3 targetSkullPosition;
    public Vector3 skullDropPosition;

    public float durationOfSkullDrop = .3f;
    private float deathTime;
    private bool deathSequenceOver;

    // Start is called before the first frame update
    void Start()
    {
        bestTimeText.text = "Mouse over lines for more info.";
        bestTimeText.color = new Color(1, 1, 1, 0);
        //Fill the thing with debug data if none exists. This is TEST CODE:
        if (GameData.Instance.timer==0)
        {
            //Died after 0 seconds? Should be impossible.
            Debug.LogError("WARNING: Either you're testing the BestTimeController by running the end run screen directly, or something had gone very wrong!");
            GameData.Instance.timesThisRun = new []{ 12,22,32, 50, 93.4f, 95.2f, 195.3f, 210,250,300,350,475,515,0,0,0,0,0,0,0};
            GameData.Instance.deathTime = 535;
        }
        //End test code.

        deathTime = (GameData.Instance.deathTime == 0 ? 2000 : GameData.Instance.deathTime);
        bestTimeSlider.transform.localPosition = new Vector3(startTimeOffset, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timeSoFar += Time.deltaTime;
        if (deathSequenceOver)
        {
            float t = Mathf.Clamp01(timeSoFar - durationOfDeathEffect);
            bestTimeText.color = new Color(1, 1, 1, t);
        }
        else if (deathSequencePlaying)
        {
            PlayDeathSequence();
        }
        else
        {
            PlaySlideAnimation();
        }

    }

    private void PlayDeathSequence()
    {
        float t = Mathf.Max(durationOfDeathEffect - timeSoFar, 0) / durationOfDeathEffect;
        bestTimeSlider.color = Color.red;
        UnityEngine.Random r = new UnityEngine.Random();
        Vector3 offset = UnityEngine.Random.insideUnitCircle*t*intensityOfDeathShake;
        bestTimeSlider.transform.localPosition = deathPosition + offset;

        t = Mathf.Max(durationOfSkullDrop - timeSoFar, 0) / durationOfSkullDrop;
        {
            skull.transform.localPosition = skullDropPosition * (t) + targetSkullPosition;
            skull.color = new Color(1, .25f, .25f, 1 - t);
        }

        float timeSinceSkullStoppedDropping = timeSoFar - durationOfSkullDrop;
        if (timeSinceSkullStoppedDropping > 0)
        {
            float timeScreenWillShake = durationOfDeathEffect - durationOfSkullDrop;
            t = Mathf.Max(timeScreenWillShake - timeSinceSkullStoppedDropping, 0) / timeScreenWillShake;
            offset = UnityEngine.Random.insideUnitCircle * t * intensityOfScreenShake;
            containerToShakeOnSkullDrop.localPosition = offset;
            if (t==0)
            {
                deathSequenceOver = true;
            }
        }
    }

    private void PlaySlideAnimation()
    {
        if (timeSoFar > delay)
        {
            if (timeSoFar < delay + duration)
            {
                float t = (timeSoFar - delay) / duration;
                float realY = y - ((2 * t - 1) * (2 * t - 1) - 1) * -maxYBend;
                bestTimeSlider.transform.localPosition = new Vector3(Mathf.Lerp(startTimeOffset, endTimeOffset, t), realY, 0);
                float quickenedTimeTakenSoFar = (timeSoFar - delay) * (600 / duration);
                if (quickenedTimeTakenSoFar >= GameData.Instance.timesThisRun[floorsDroppedOff] && GameData.Instance.timesThisRun[floorsDroppedOff] != 0)
                {
                    float tForDropoff = GameData.Instance.timesThisRun[floorsDroppedOff] / 600f;
                    float dropoffx = Mathf.Lerp(startTimeOffset, endTimeOffset, tForDropoff);
                    float dropoffy = y - ((2 * tForDropoff - 1) * (2 * tForDropoff - 1) - 1) * -maxYBend; ;
                    DropOffFloor(floorsDroppedOff, dropoffx, dropoffy);
                    floorsDroppedOff++;
                }
                if (quickenedTimeTakenSoFar >= deathTime|| quickenedTimeTakenSoFar >=600)
                {
                    deathSequencePlaying = true;
                    SoundManager.Instance.PlaySound("EndScreenDie", 1f);
                    timeSoFar = 0;
                    deathPosition = bestTimeSlider.transform.localPosition;
                    targetSkullPosition = deathPosition + new Vector3(0, -60,0);
                }
            }
        }
    }

    private void DropOffFloor(int floorsDroppedOff, float dropoffx, float dropoffy)
    {
        SoundManager.Instance.PlaySound("EndScreenLevel", 1f);
        FloorDropoffPrefab dropped = GameObject.Instantiate<FloorDropoffPrefab>(floorDropoffPrefab);
        dropped.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform>());
        dropped.GetComponent<RectTransform>().localPosition = new Vector3(dropoffx, dropoffy, 0);
        float timeTaken = (floorsDroppedOff == 0 ? GameData.Instance.timesThisRun[floorsDroppedOff] : GameData.Instance.timesThisRun[floorsDroppedOff] - GameData.Instance.timesThisRun[floorsDroppedOff - 1]);
       // Debug.Log(timeTaken + ", " + GameData.Instance.bestTimes[floorsDroppedOff]);
        dropped.Initialize(floorsDroppedOff+1, timeTaken, timeTaken<GameData.Instance.bestTimes[floorsDroppedOff], GameData.Instance.bestTimes[floorsDroppedOff], bestTimeText);
    }
}
