using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCaroselManager : MonoBehaviour
{
    public Sprite[] offSprites;
    public Sprite[] onSprites;

    public Sprite[] dashAnimSprites;
    public Sprite[] jumpAnimSprites;
    public Sprite[] fireAnimSprites;
    public Sprite[] stealthAnimSprites;
    public ParticleSystem useParticles;

    public SpriteRenderer[] elementalSymbols;

    int elementIThinkIsSelected = 0;

    int elementsIThinkIhave = 0;

    int symbolsOn;

    private float degreesRotated;
    private float targetDegreesRotated;

    float timeSinceLastFrame = 0;
    float frameTime = .08f;
    int currentFrame = 0;
    int framesSinceLastJump = 1000; //Any large number.

    CharacterStats stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        UpdateNumberOfPowers();
    }

    private void UpdateNumberOfPowers()
    {
        for (int x = 0; x < 10; x++)
        {
            elementalSymbols[x].gameObject.SetActive(false);
            elementalSymbols[x].enabled = false;

        }
        for (int x = 0; x <= stats.powersGained; x++)
        {
            elementalSymbols[x].gameObject.SetActive(true);
            elementalSymbols[x + stats.powersGained + 1].gameObject.SetActive(true);
            elementalSymbols[x].enabled = true;
            elementalSymbols[x + stats.powersGained + 1].enabled = true;
        }
        symbolsOn = (stats.powersGained + 1) * 2;

        if (stats.powersGained == 1)
        {
            symbolsOn = 6;
            elementalSymbols[4].gameObject.SetActive(true);
            elementalSymbols[4].enabled = true;
            elementalSymbols[5].gameObject.SetActive(true);
            elementalSymbols[5].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        CheckAdvanceFrame();

        if (elementsIThinkIhave != stats.powersGained)
        {
            elementsIThinkIhave = stats.powersGained;
            UpdateNumberOfPowers();
            elementIThinkIsSelected = -1;
        }

        if (elementIThinkIsSelected != stats.currentPower)
        {
            float i = targetDegreesRotated;
            while (i > (stats.powersGained + 1))
            {
                i -= (stats.powersGained + 1);
            }
            while (i <= 0)
            {
                i += (stats.powersGained + 1);
            }
            float difference = stats.currentPower-i;

            if (difference < (-.5*(stats.powersGained + 1)))
            {
                difference += (stats.powersGained + 1);
            }

            targetDegreesRotated += difference;

            elementIThinkIsSelected = stats.currentPower;
        }

        for (int x = 0; x < symbolsOn; x++)
        {
            int elemNum = x % (stats.powersGained + 1);
            elementalSymbols[x].sprite = GetImageDisplayedForPower(elemNum);
        }

        degreesRotated = Mathf.Lerp(degreesRotated, targetDegreesRotated, 1-Mathf.Pow(.000001f, Time.deltaTime));

        float degreesPerSymbol = Mathf.PI / (stats.powersGained==1?3:stats.powersGained+1);
        float inRadiansDegreesRotated = -degreesRotated * (degreesPerSymbol);

        for (int x = 0; x < symbolsOn; x++)
        {
            elementalSymbols[x].transform.localPosition = new Vector3(Mathf.Sin(inRadiansDegreesRotated + x * degreesPerSymbol)*4.5f, Mathf.Cos(inRadiansDegreesRotated + x * degreesPerSymbol)*1.4f - 6.5f, 0);
        }
    }

    private bool wasJumpingLastFrame = false;

    private void CheckAdvanceFrame()
    {
        if (GameData.Instance.jumping && !wasJumpingLastFrame)
        {
            framesSinceLastJump = 0;
        }
        wasJumpingLastFrame = GameData.Instance.jumping;

        /*if (!GameData.Instance.dashing &&
            !GameData.Instance.stealthed &&
            !GameData.Instance.hasted &&
            !GameData.Instance.jumping)
        {
            timeSinceLastFrame = 0;
            currentFrame = 0;
            return;
        }*/
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame > frameTime)
        {
            timeSinceLastFrame -= frameTime;
            currentFrame++;
            framesSinceLastJump++;
            wasDashingLastFrame = GameData.Instance.dashing;
            wasHastedLastFrame = GameData.Instance.hasted;
        }
    }

    private bool wasDashingLastFrame = false;
    private bool wasHastedLastFrame = false;


    private Sprite GetImageDisplayedForPower(int power)
    {
        if (stats.currentPower == 2 && GameData.Instance.stealthed && !useParticles.isPlaying)
        {
            useParticles.Play();
        }

        if ((stats.currentPower != 2 || !GameData.Instance.stealthed) && useParticles.isPlaying)
        {
            useParticles.Stop();
        }

        if (power == 0)
        {
            return offSprites[0];
        }

        else if (power == 1)
        {
            if (GameData.Instance.dashing&&!wasDashingLastFrame||!GameData.Instance.dashing && wasDashingLastFrame)
            {
                return dashAnimSprites[8];
            }
            else if (GameData.Instance.dashing)
            {
                return dashAnimSprites[currentFrame % 6];
            }
            else if (stats.currentPower==1)
            {
                return onSprites[1];
            }
            else
            {
                return offSprites[1];
            }
        }

        else if (power == 2)
        {
            if (stats.currentPower == 2&&GameData.Instance.stealthed)
            {
                return stealthAnimSprites[0];
            }
            if (stats.currentPower == 2)
            {
                return onSprites[2];
            }
            else
            {
                return offSprites[2];
            }
        }

        else if (power == 3)
        {
            if (GameData.Instance.hasted && !wasHastedLastFrame || !GameData.Instance.hasted && wasHastedLastFrame)
            {
                return fireAnimSprites[6];
            }
            else if (GameData.Instance.hasted)
            {
                return fireAnimSprites[currentFrame % 6];
            }
            else if (stats.currentPower == 3)
            {
                return onSprites[3];
            }
            else
            {
                return offSprites[3];
            }
        }

        else if (power == 4)
        {
            if (framesSinceLastJump < 6)
            {
                return jumpAnimSprites[framesSinceLastJump];
            }
            else if (stats.currentPower == 4)
            {
                return onSprites[4];
            }
            else
            {
                return offSprites[4];
            }
        }

        return null;
    }

    private bool IsPowerBeingUsed()
    {
        switch (elementIThinkIsSelected)
        {
            case 0:
                return false;
            case 1:
                return GameData.Instance.dashing;
            case 2:
                return GameData.Instance.stealthed;
            case 3:
                return GameData.Instance.hasted;
            case 4:
                return GameData.Instance.jumping;
            default:
                Debug.LogWarning("Something went wrong: Power is not 0-4. HAX!"); //Honestly we've probs already thrown an AOoB Exception somewhere...
                return false;
        }

    }
}
