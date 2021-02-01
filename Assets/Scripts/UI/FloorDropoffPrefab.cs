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

    public float pulseCycleDuration = 2.5f;
    public float pulseSize = 3;
    public Image pulseImage;

    private bool showing = false;
    private float textOpacity;

    public float showSpeed = 5;
    public float hideSpeed = 10;

    private string textToDisplay;
    private TextMeshProUGUI textToChange;

    private bool record;

    // Start is called before the first frame update
    void Start()
    {
        if (record)
        {
            Color colorToTurn = new Color(1, 1, 0, textOpacity);
            this.GetComponent<Image>().color = new Color(1, 1, 0, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (showing)
        {
            textOpacity = Mathf.Min(textOpacity + Time.deltaTime * showSpeed, 1.0f);
        }
        else
        {
            textOpacity = Mathf.Max(textOpacity - Time.deltaTime * hideSpeed, 0);
        }*/

        timeSoFar += Time.deltaTime;
        if (animDone)
        {
            if (record)
            {
                float t = (timeSoFar % pulseCycleDuration) / pulseCycleDuration;
                pulseImage.color = new Color(1, 1, 0, 1-t);
                pulseImage.rectTransform.localScale = new Vector3(pulseSize, pulseSize/2, 1)*t;
            }
            return;
        }
        if (timeSoFar>=timeToFadeIn)
        {
            animDone = true;
            timeSoFar = 0;
            this.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            this.GetComponent<RectTransform>().localScale = Vector3.one;
            return;
        }
        this.GetComponent<Image>().color = new Color(1, 1, 1, timeSoFar/timeToFadeIn);
        this.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, Vector3.one, timeSoFar / timeToFadeIn);
    }

    internal void Initialize(int floorNum, float timeTaken, bool record, float oldTimeTaken, TextMeshProUGUI textToChange)
    {
        this.textToChange = textToChange;
        this.record = record;
        
        this.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        this.GetComponent<RectTransform>().localScale = startScale;

        int seconds = (int)(timeTaken % 60);
        int minutes = (int)(timeTaken / 60);
        int subSeconds = (int)((timeTaken % 1) * 10);
        string floorName = getFloorName(floorNum);
        textToDisplay = "<i><size=18>"+floorName + "</size></i>\n";
        //textToDisplay += "Time Spent: " +minutes + ":" + ((seconds < 10) ? "0" + seconds : "" + seconds + "."+subSeconds.ToString("00"));
        
        if (record)
        {
            textToDisplay += "New Record! " + minutes + ":" + seconds.ToString("00") + "." + subSeconds.ToString("00");
            if (oldTimeTaken == Mathf.Infinity)
            {
                textToDisplay += "\nNew Record!";
            }
            else
            {
                seconds = (int)(oldTimeTaken % 60);
                minutes = (int)(oldTimeTaken / 60);
                subSeconds = (int)((oldTimeTaken % 1) * 10);
                //textToDisplay += "\nOld Record: " + minutes + ":" + ((seconds < 10) ? "0" + seconds : "" + seconds + "." + subSeconds.ToString("00"));
                textToDisplay += "\nOld Record: " + minutes + ":" + seconds.ToString("00") + "." + subSeconds.ToString("00");
            }
        }
        else {
            textToDisplay += "Time Spent: " + minutes + ":" + seconds.ToString("00") + "." + subSeconds.ToString("00");
            seconds = (int)(oldTimeTaken % 60);
            minutes = (int)(oldTimeTaken / 60);
            subSeconds = (int)((oldTimeTaken % 1) * 10);
            //textToDisplay += "\nOld Record: " + minutes + ":" + ((seconds < 10) ? "0" + seconds : "" + seconds + "." + subSeconds.ToString("00"));
            textToDisplay += "\nCurrent Record: " + minutes + ":" + seconds.ToString("00") + "." + subSeconds.ToString("00");
        }
    }

    private string getFloorName(int floor) {
        string floorName = "";
        //NOTE: if you edit this list also edit the list in FloorNameDropdownController
        switch (floor)
        {
            case 1:
                floorName = "Icy Portcullis";
                break;
            case 2:
                floorName = "Bleak Pit";
                break;
            case 3:
                floorName = "Dispatch Channel";
                break;
            case 4:
                floorName = "Stone Lung";
                break;
            case 5:
                floorName = "Sifting Claws";
                break;
            case 6:
                floorName = "Tangled Corridor";
                break;
            case 7:
                floorName = "Fickle Stanchion";
                break;
            case 8:
                floorName = "Trundle Gyre";
                break;
            case 9:
                floorName = "Slag Canal";
                break;
            case 10:
                floorName = "Reclamation Pool";
                break;
            case 11:
                floorName = "Sleeping Citadel";
                break;
            case 12:
                floorName = "Toothed Tower";
                break;
            case 13:
                floorName = "Fractured Theater";
                break;
            case 14:
                floorName = "Bellows";
                break;
            case 15:
                floorName = "Seeping Drain";
                break;
            case 16:
                floorName = "Precarious Bridge";
                break;
            case 17:
                floorName = "Residuary";
                break;
            case 18:
                floorName = "Vigilant Bypass";
                break;
            case 19:
                floorName = "Warded Hall";
                break;
            case 20:
                floorName = "Control Room";
                break;
        }

        return floorName;
    }

    public void ShowStuff()
    {
        textToChange.text = textToDisplay;
    }
    public void HideStuff()
    {
    }
}
