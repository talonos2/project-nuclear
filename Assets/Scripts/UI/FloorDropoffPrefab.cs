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

        textToDisplay = minutes + ":" + ((seconds < 10) ? "0" + seconds : "" + seconds + "."+subSeconds ) + " on Floor "+floorNum;
        if (record)
        {
            if (oldTimeTaken == Mathf.Infinity)
            {
                textToDisplay += "\n(Old Record: None.)";
            }
            else
            {
                seconds = (int)(oldTimeTaken % 60);
                minutes = (int)(oldTimeTaken / 60);
                subSeconds = (int)((oldTimeTaken % 1) * 10);
                textToDisplay += "\n(Old Record: " + minutes + ":" + ((seconds < 10) ? "0" + seconds : "" + seconds + "." + subSeconds) + ")";
            }
        }
    }

    public void ShowStuff()
    {
        textToChange.text = textToDisplay;
    }
    public void HideStuff()
    {
    }
}
