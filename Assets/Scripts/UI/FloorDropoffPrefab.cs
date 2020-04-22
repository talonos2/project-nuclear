using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloorDropoffPrefab : MonoBehaviour
{
    public float timeToFadeIn;
    private float timeSoFar;
    public Vector3 startScale = new Vector3 (.5f,2.5f,1);
    private bool animDone = false;

    public TextMeshProUGUI floorText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI recordText;

    private bool showing = false;
    private float textOpacity;

    public float showSpeed = 5;
    public float hideSpeed = 10;

    private bool record;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (showing)
        {
            textOpacity = Mathf.Min(textOpacity + Time.deltaTime * showSpeed, 1.0f);
        }
        else
        {
            textOpacity = Mathf.Max(textOpacity - Time.deltaTime * hideSpeed, 0);
        }

        if (record)
        {
            Color colorToTurn = new Color(1, 1, 0, textOpacity);
            timeText.color = colorToTurn;
            floorText.color = colorToTurn;
            recordText.color = colorToTurn;
        }
        else
        {
            Color colorToTurn = new Color(1, 1, 1, textOpacity);
            timeText.color = colorToTurn;
            floorText.color = colorToTurn;
        }


        if (animDone)
        {
            return;
        }
        timeSoFar += Time.deltaTime;
        if (timeSoFar>=timeToFadeIn)
        {
            animDone = true;
            this.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            this.GetComponent<RectTransform>().localScale = Vector3.one;
            return;
        }
        this.GetComponent<Image>().color = new Color(1, 1, 1, timeSoFar/timeToFadeIn);
        this.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, Vector3.one, timeSoFar / timeToFadeIn);
    }

    internal void Initialize(int floorNum, float timeTaken, bool record)
    {
        this.record = record;
        this.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        this.GetComponent<RectTransform>().localScale = startScale;

        floorText.text = "Floor  " + floorNum;

        float seconds = (int)(timeTaken % 60);
        float minutes = (int)(timeTaken / 60);
        int min = (seconds == 0 ? 10 : 9) - GameData.Instance.minutes;
        int sec = (seconds == 0 ? 0 : 60 - GameData.Instance.seconds);
        int subSeconds = (int)((timeTaken * 10) / 10);
        timeText.text = min + ":" + ((sec < 10) ? "0" + sec : "" + sec+(min==0?"."+subSeconds:""));
    }

    public void ShowStuff()
    {
        showing = true;
    }
    public void HideStuff()
    {
        showing = false;
    }
}
