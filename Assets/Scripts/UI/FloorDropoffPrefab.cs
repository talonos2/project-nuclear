using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorDropoffPrefab : MonoBehaviour
{
    public float timeToFadeIn;
    private float timeSoFar;
    public Vector3 startScale = new Vector3 (.5f,2.5f,1);
    private bool animDone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    internal void Initialize()
    {
        this.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        this.GetComponent<RectTransform>().localScale = startScale;
    }
}
