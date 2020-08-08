using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GabTextController : MonoBehaviour
{

    public Canvas gabTextCanvas;
    public Image gabBackgroundFader1;
    public Image gabBackgroundFader2;
    public TextMeshProUGUI gabTextToPrint;
    public List<Gab> gabPlayList;
    public float gabDelay = 1.5f;
    public float delayBetweenGabs = .3f;
    private float gabDelayCounter;
    private bool playingGab;
    private bool playingDelay;

    private static readonly float FADE_TIME = .3f;
    private static readonly float FADE_AMOUNT = .3f;

    private static readonly float FLASH_TIME = .2f;

    // Start is called before the first frame update
    void Start()
    {
        gabPlayList = new List<Gab>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gabPlayList.Count == 0) return;

        Gab currentGab = gabPlayList[0];

        if (GameState.isInBattle || (GameState.fullPause&&!currentGab.fullPause))
        {
            HideGabUi();
            return;
        }

        if (playingGab)
        {
            if (Input.GetButtonDown("Submit")&&gabDelayCounter>FADE_TIME)
            {
                gabDelayCounter = FADE_TIME;
            }
            gabTextCanvas.enabled = true;
            gabDelayCounter -= Time.deltaTime;

            if (gabDelayCounter <= 0)
            {
                HideGabUi();
                gabBackgroundFader1.color = new Color(0, 0, 0, 0);
                RemoveGabPlayed();
                playingGab = false;
                if (currentGab.fullPause)
                {
                    GameState.fullPause = false;
                }

                if (gabPlayList.Count > 0)
                {
                    playingDelay = true;
                    gabDelayCounter = delayBetweenGabs;
                }
            }
            else
            {
                if (currentGab.fade)
                {
                    if (gabDelayCounter <= FADE_TIME)
                    {
                        gabBackgroundFader1.color = new Color(0, 0, 0, Mathf.Lerp(0, FADE_AMOUNT, gabDelayCounter / FADE_TIME));
                    }
                    else if (gabDelayCounter > (currentGab.duration - FADE_TIME))
                    {
                        gabBackgroundFader1.color = new Color(0, 0, 0, Mathf.Lerp(0, FADE_AMOUNT, (currentGab.duration - gabDelayCounter) / FADE_TIME));
                    }
                    else
                    {
                        gabBackgroundFader1.color = new Color(0, 0, 0, FADE_AMOUNT);
                    }
                }
                else
                {
                    gabBackgroundFader1.color = new Color(0, 0, 0, 0);
                }
            }

            if (currentGab.flash)
            {
                if ((currentGab.duration - gabDelayCounter) < FLASH_TIME)
                {
                    gabBackgroundFader2.color = Color.Lerp(new Color(1, 1, 0, 0), new Color(1, 1, 0, 1), gabDelayCounter / FLASH_TIME);
                }
                else if ((currentGab.duration - gabDelayCounter) < FLASH_TIME * 2)
                {
                    gabBackgroundFader2.color = Color.Lerp(new Color(1, 1, 0, 1), new Color(1, 1, 1, .5f), (gabDelayCounter - FLASH_TIME) / FLASH_TIME);
                }
                else
                {
                    gabBackgroundFader2.color = new Color(0, 0, 0, 1);
                }
            }
            else
            {
                if ((currentGab.duration - gabDelayCounter) < FLASH_TIME)
                {
                    gabBackgroundFader2.color = Color.Lerp(new Color(1, 1, 0, 0), new Color(1, 1, 0, 1), gabDelayCounter / FLASH_TIME);
                }
                else if ((currentGab.duration - gabDelayCounter) < FLASH_TIME * 2)
                {
                    gabBackgroundFader2.color = Color.Lerp(new Color(1, 1, 0, 1), new Color(1, 1, 1, .5f), (gabDelayCounter - FLASH_TIME) / FLASH_TIME);
                }
                else
                {
                    gabBackgroundFader2.color = new Color(0, 0, 0, 1);
                }
            }
        }

        if (playingDelay)
        {
            gabDelayCounter -= Time.deltaTime;
            if (gabDelayCounter <= 0)
            {
                playingDelay = false;
                GameState.fullPause = false;
                PlayNextGabText();
            }
        }
    }

    public void AddGabToPlay(Gab gabTextToAdd) {
        gabPlayList.Add(gabTextToAdd);
        if (!playingGab)
        {
            PlayNextGabText();
        }
    }

    public void AddGabToPlay(String gabTextToAdd)
    {
        gabPlayList.Add(new Gab(gabTextToAdd, false));
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
        Gab currentGab = gabPlayList[0];
        gabBackgroundFader1.color = new Color(0, 0, 0, 0);
        gabTextToPrint.text = currentGab.gabText;
        ShowGabUi();
        playingGab = true;
        playingDelay = false;
        gabDelayCounter = gabDelay;
        if (currentGab.fullPause)
        {
            GameState.fullPause = true;
        }
    }

    private void ShowGabUi()
    {
        gabTextCanvas.enabled = true;
    }

    private void HideGabUi()
    {
        gabTextCanvas.enabled = false;
    }

    public class Gab
    {
        public string gabText;
        public bool fullPause;
        public bool fade;
        public bool flash;
        public float duration;

        public Gab(string gabText = "", bool fullPause = false, float duration = 0, bool fade = false, bool flash = false)
        {
            this.gabText = gabText;
            this.fullPause = fullPause;
            this.duration = duration;
            this.fade = fade;
            this.flash = flash;
        }
    }
}
