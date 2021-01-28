using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GabTextController : MonoBehaviour
{

    public Canvas gabTextCanvas;
    public Image gabBackgroundFader1;
    public Image gabBackgroundFader2;
    public Image playerPanel;
    public Image playerPanelBorder;
    public TextMeshProUGUI gabTextToPrint;
    public TextMeshProUGUI itemGabTextToPrint;
    public List<Gab> gabPlayList;
    public float gabDelay = 3f;
    public float delayBetweenGabs = .3f;
    private float gabDelayCounter;
    private bool playingGab;
    private bool playingDelay;
    public VideoPlayer player;

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

        if (GameState.isInBattle || (GameState.getFullPauseStatus()&&!currentGab.fullPause))
        {
            HideGabUi();
            return;
        }

        if (playingGab)
        {
            if (FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE)&& gabDelayCounter>FADE_TIME&&!currentGab.itemGab)
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
                    GameState.setFullPause(false);
                    GameData.Instance.playingTutorial = false;
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
                    gabBackgroundFader2.color = new Color(1, 1, 0, (currentGab.duration - gabDelayCounter) / FLASH_TIME);
                }
                else if ((currentGab.duration - gabDelayCounter) < FLASH_TIME * 2)
                {
                    float partThroughNumberTwo = ((currentGab.duration-FLASH_TIME) - gabDelayCounter)/FLASH_TIME;
                    gabBackgroundFader2.color = new Color(1 - partThroughNumberTwo / 1.5f, 1- partThroughNumberTwo / 1.5f , partThroughNumberTwo/3, 1 - partThroughNumberTwo/2);
                }
                else if (gabDelayCounter < FADE_TIME)
                {
                    gabBackgroundFader2.color = new Color(.33f, .33f, .33f, gabDelayCounter/FADE_TIME/2f);
                }
                else
                {
                    gabBackgroundFader2.color = new Color(.33f, .33f, .33f, .5f);
                }
            }
            else
            {
                /*if ((currentGab.duration - gabDelayCounter) < FLASH_TIME)
                {
                    gabBackgroundFader2.color = Color.Lerp(new Color(0, 0, 0, 0f), new Color(0, 0, 0, 1f), gabDelayCounter / FLASH_TIME);
                }
                else if (gabDelayCounter > (currentGab.duration - FADE_TIME))
                {
                    gabBackgroundFader1.color = new Color(0, 0, 0, Mathf.Lerp(0f, FADE_AMOUNT, (currentGab.duration - gabDelayCounter) / FADE_TIME));
                }
                else
                {
                    gabBackgroundFader2.color = new Color(0, 0, 0, 1);
                }*/
            }

            if (currentGab.clipToPlayForTutorial!=null)
            {
                if ((currentGab.duration - gabDelayCounter) < FLASH_TIME)
                {
                    playerPanel.color = new Color(1, 1, 1, (currentGab.duration-gabDelayCounter) / FLASH_TIME);
                    playerPanelBorder.color = playerPanel.color;
                }
                else if (gabDelayCounter < FADE_TIME)
                {
                    playerPanel.color = new Color(1, 1, 1, gabDelayCounter / FLASH_TIME);
                    playerPanelBorder.color = playerPanel.color;
                }
                else
                {
                    playerPanel.color = Color.white;
                    playerPanelBorder.color = playerPanel.color;
                }
            }
            else
            {
                playerPanel.color = new Color(1, 1, 1, 0);
            }
        }

        if (playingDelay)
        {
            gabDelayCounter -= Time.deltaTime;
            if (gabDelayCounter <= 0)
            {
                playingDelay = false;
                GameState.setFullPause(false);
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

    public void AddItemGabToPlay(String gabTextToAdd, float playTime=3f)
    {
        gabPlayList.Add(new Gab(gabTextToAdd, false, playTime, false, false, null, true));
        if (!playingGab)
        {
            PlayNextGabText();
        }
    }
    public void AddGabToPlay(String gabTextToAdd)
    {
        gabPlayList.Add(new Gab(gabTextToAdd, false, 3f, false, false,null,false));
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
        if (currentGab.itemGab)
        {
            itemGabTextToPrint.text = currentGab.gabText;
            gabTextToPrint.enabled = false;
            itemGabTextToPrint.enabled = true;
        }
        else {
            if (currentGab.fullPause) {
                //Debug.Log("Setting playing tutorial");
                GameData.Instance.playingTutorial = true; }
            gabTextToPrint.enabled = true;
            itemGabTextToPrint.enabled = false;
        }
        ShowGabUi();
        playingGab = true;
        playingDelay = false;
        gabDelayCounter = currentGab.duration==0?gabDelay:currentGab.duration;
        if (currentGab.clipToPlayForTutorial!=null)
        {
            player.clip = currentGab.clipToPlayForTutorial;
        }
        if (currentGab.fullPause)
        {
            GameState.setFullPause(true);
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
        public VideoClip clipToPlayForTutorial;
        public bool itemGab;

        public Gab(string gabText = "", bool fullPause = false, float duration = 0, bool fade = false, bool flash = false, VideoClip clipToPlayForTutorial = null, bool itemGab = false)
        {
            this.gabText = gabText;
            this.fullPause = fullPause;
            this.duration = duration;
            this.fade = fade;
            this.flash = flash;
            this.clipToPlayForTutorial = clipToPlayForTutorial;
            this.itemGab = itemGab;
        }
    }
}
