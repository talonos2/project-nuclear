using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GabTextController : MonoBehaviour
{

    public Canvas gabTextCanvas;
    public TextMeshProUGUI gabTextToPrint;
    public List<string> gabPlayList;
    public float gabDelay = 1.5f;
    public float delayBetweenGabs = .3f;
    private float gabDelayCounter;
    private bool playingGab;
    private bool playingDelay;

    // Start is called before the first frame update
    void Start()
    {
        gabPlayList = new List<string>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.isInBattle || GameState.fullPause) {
            HideGabUi();
            return;
        }
        if (gabPlayList.Count == 0) return;

        if (playingGab)
        {
            gabTextCanvas.enabled = true;
            gabDelayCounter -= Time.deltaTime;

            if (gabDelayCounter <= 0)
            {
                HideGabUi();
                RemoveGabPlayed();
                playingGab = false;

                if (gabPlayList.Count > 0) {
                    playingDelay = true;
                    gabDelayCounter = delayBetweenGabs;
                }
            }
        }

        if (playingDelay) {
            gabDelayCounter -= Time.deltaTime;
            if (gabDelayCounter <= 0)
            {
                playingDelay = false;
                PlayNextGabText();
            }
        }


    }

    public void AddGabToPlay(string gabTextToAdd) {
        gabPlayList.Add(gabTextToAdd);
        if (!playingGab)
        {
            PlayNextGabText();
        }
    }
    private void RemoveGabPlayed()
    {
        gabPlayList.RemoveAt(0);
    }

    private void PlayNextGabText()
    {
        gabTextToPrint.text = gabPlayList[0];
        ShowGabUi();
        playingGab = true;
        playingDelay = false;
        gabDelayCounter = gabDelay;
    }

    private void ShowGabUi()
    {
        gabTextCanvas.enabled = true;
    }

    private void HideGabUi()
    {
        gabTextCanvas.enabled = false;
    }
}
