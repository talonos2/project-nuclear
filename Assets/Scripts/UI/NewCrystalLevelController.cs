﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewCrystalLevelController : MonoBehaviour
{
    public CrystalType crystalType;
    public GameObject crystalPanel;
    public Text startingCrystals;
    public TextMeshProUGUI crystalBonusText;
    public SimpleTooltip crystalTooltip;
    // Start is called before the first frame update

    private static int[] crystalTiers = {0, 50, 150, 350, 650, 1075, 1625, 2300, 3100, 4025, 5075, 6250, 7550, 8975, 10525, 12200, 14000, 15925, 17975, 20150, 22450, 31451 };

    private CharacterStats savedStats;
    private float crystalBonusValue;
    private int oldCrystals;
    private int newCrystals;

    //How long does it take to fill a bar? A*B^C seconds.
    public float baseTimePerBar = 2;
    public float multipierTimePerBar = 1;
    public float exponentTimePerBar = .65f;
    public float barAnimationCurveStrength = .65f;

    private bool animationFinished;

    public float timerUntilAnimationStarts;

    private float startNumberOfBars;
    private float targetNumberOfBars;

    private float durationOfFill;
    private float timeSoFar;

    private static readonly float BARS_PER_TING = .4f;
    private int lastTingAt;

    void Start()
    {
        savedStats = GameObject.Find("GameStateData").GetComponent<CharacterStats>();

        switch (crystalType)
        {
            case CrystalType.HEALTH:
                oldCrystals = GameData.Instance.HealhCrystalTotal;
                newCrystals = savedStats.HealthCrystalsGained;
                crystalBonusValue = 20;
                break;
            case CrystalType.MANA:
                oldCrystals = GameData.Instance.ManaCrystalTotal;
                newCrystals = savedStats.ManaCrystalsGained;
                crystalBonusValue = 20;
                break;
            case CrystalType.ATTACK:
                oldCrystals = GameData.Instance.AttackCrystalTotal;
                newCrystals = savedStats.AttackCrystalsGained;
                crystalBonusValue = 4;
                break;
            case CrystalType.DEFENSE:
                oldCrystals = GameData.Instance.DefenseCrystalTotal;
                newCrystals = savedStats.defenseCrystalsGained;
                crystalBonusValue = 2f;
                break;

        }

        crystalTooltip.infoLeft = oldCrystals+ newCrystals + "/" + crystalTiers[GetCrystalTier(oldCrystals+newCrystals)] + " Crystals Until Next Tier";

        //Debug.Log("Total " + oldCrystals+", new "+ newCrystals);
        startNumberOfBars = GetNumberOfBars(oldCrystals);
        lastTingAt = (int)(startNumberOfBars / BARS_PER_TING);
        targetNumberOfBars = GetNumberOfBars(oldCrystals+newCrystals);
        //Debug.Log("Start " + startNumberOfBars + ", End " + targetNumberOfBars);
        float barIncrease = targetNumberOfBars-startNumberOfBars;

        SetBarLevel(startNumberOfBars);

        if (oldCrystals==newCrystals)
        {
            animationFinished = true;
            return;
        }

        durationOfFill = baseTimePerBar * (Mathf.Pow(multipierTimePerBar* barIncrease, exponentTimePerBar));
    }

    private float GetNumberOfBars(int numCrystals)
    {
        int baseCrystalTier = GetCrystalTier(numCrystals);
        return (float)(numCrystals - crystalTiers[baseCrystalTier-1]) / (float)(crystalTiers[baseCrystalTier]- crystalTiers[baseCrystalTier-1])+baseCrystalTier;
    }

    // Update is called once per frame
    void Update()
    {
        if (animationFinished)
        {
            return;
        }
        timerUntilAnimationStarts -= Time.deltaTime;
        if (timerUntilAnimationStarts >= 0)
        {
            return;
        }
        AnimateCrystalBarsFilling();
    }

    private void AnimateCrystalBarsFilling()
    {
        if (durationOfFill == 0)
            return;
        timeSoFar += Time.deltaTime;
        if (timeSoFar >= durationOfFill)
        {
            animationFinished = true;
            timeSoFar = durationOfFill;
        }

        float amountThrough = timeSoFar/durationOfFill;

        amountThrough = Mathf.Pow(amountThrough, barAnimationCurveStrength);

        float currentBarAmount = startNumberOfBars + (targetNumberOfBars - startNumberOfBars) * amountThrough;
        int currentTingLevel = (int)(currentBarAmount / BARS_PER_TING);
        if (currentTingLevel != lastTingAt)
        {
            SoundManager.Instance.PlaySound("EndScreenTick"+(UnityEngine.Random.value>.5f?"":"2"), 1f);
        }
        lastTingAt = currentTingLevel;

        SetBarLevel(currentBarAmount);
    }

    private void SetBarLevel(float barAmount)
    {
        int fullBars = Mathf.FloorToInt(barAmount);
        float remainderBars = barAmount % 1.0f;

        int crystalAmount = Mathf.RoundToInt(Mathf.Lerp(crystalTiers[fullBars], crystalTiers[fullBars + 1], remainderBars));

        SetBarText(fullBars);


        int crystalsSoFarInTier = crystalAmount - (crystalTiers[fullBars]);
        int crystalsNeededToLevel = crystalTiers[fullBars+1]- crystalTiers[fullBars];
        string crystalTotalText = "" + crystalsSoFarInTier + "/" + crystalsNeededToLevel;
        startingCrystals.text = crystalTotalText;
        crystalPanel.transform.localScale = new Vector3(remainderBars, 1, 1);
    }

    private void SetBarText(int fullBars)
    {
        string bonusText = "+" + (int)((fullBars-1) * crystalBonusValue);
        crystalBonusText.text = bonusText;
    }

    //Setup Crystal Buffs

    public static void AddCrystalsPostRun()
    {
        CharacterStats savedStats = GameObject.Find("GameStateData").GetComponent<CharacterStats>();
        GameData gameData = GameData.Instance;
        gameData.AttackCrystalTotal += savedStats.AttackCrystalsGained;
        savedStats.AttackCrystalsGained = 0;
        gameData.DefenseCrystalTotal += savedStats.defenseCrystalsGained;
        savedStats.defenseCrystalsGained = 0;
        gameData.HealhCrystalTotal += savedStats.HealthCrystalsGained;
        savedStats.HealthCrystalsGained = 0;
        gameData.ManaCrystalTotal += savedStats.ManaCrystalsGained;
        savedStats.ManaCrystalsGained = 0;
        SetHealthBuff();
        SetManaBuff();
        SetAttackBuff();
        SetDefenseBuff();
    }

    public static void SetCrystalBuffs()
    {
        SetHealthBuff();
        SetManaBuff();
        SetAttackBuff();
        SetDefenseBuff();
    }

    public static int GetCrystalTier(int crystalAmount)
    {
        int i = 0;
        while (i < crystalTiers.Length)
        {
            if (crystalAmount < crystalTiers[i])
            {
                break;
            }
            i++;
        }
        return i;
    }




    private static void SetDefenseBuff()
    {

        int i = 0;
        while (i < crystalTiers.Length)
        {
            if (GameData.Instance.DefenseCrystalTotal < crystalTiers[i])
            {
                break;
            }
            i++;
        }
        GameData.Instance.DefenseCrystalBonus = (int)(2f * (i-1));
    }

    private static void SetAttackBuff()
    {
        int i = 0;
        while (i < crystalTiers.Length)
        {
            if (GameData.Instance.AttackCrystalTotal < crystalTiers[i])
            {
                break;
            }
            i++;
        }
        GameData.Instance.AttackCrystalBonus = 4 * (i - 1);

    }

    private static void SetManaBuff()
    {
        int i = 0;
        while (i < crystalTiers.Length)
        {
            if (GameData.Instance.ManaCrystalTotal < crystalTiers[i])
            {
                break;
            }
            i++;
        }
        GameData.Instance.ManaCrystalBonus = 20 * (i - 1);
    }

    private static void SetHealthBuff()
    {
        int i = 0;
        while (i < crystalTiers.Length)
        {
            if (GameData.Instance.HealhCrystalTotal < crystalTiers[i])
            {
                break;
            }
            i++;
        }
        GameData.Instance.HealhCrystalBonus = 20 * (i - 1);//Crystal tiers now starting at 0 rather then 50. 
    }

}
