using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewCrystalLevelController : MonoBehaviour
{
    public bool healthCrystal;
    public bool manaCrystal;
    public bool attackCrystal;
    public bool defenseCrystal;
    public GameObject crystalPanel;
    public Text startingCrystals;
    public Text crystalBonusText;
    // Start is called before the first frame update

    private static int[] crystalTiers = { 50, 150, 350, 650, 1075, 1625, 2300, 3100, 4025, 5075, 6250, 7550, 8975, 10525, 12200, 14000, 15925, 17975, 20150, 22450, 31451 };
    private static int[] crystalUpgrades = { 50, 100, 200, 300, 425, 550, 675, 800, 925, 1050, 1175, 1300, 1425, 1550, 1675, 1800, 1925, 2050, 2175, 2300, 9001 };

    private float timerUntilAnimation=2f;
    private CharacterStats savedStats;
    private int crystalBonusValue;
    private int cTier;
    private int oldCrystals;
    private int newCrystals;
    //Creates a full bar every 1 second. To increase the time to fill, lower barAnimationPerFrame or increase timerAnimationReset
    private float barAnimationPerFrame = .025f;
    private float timerAnimationReset = .015f;
    private float timerAnimation = 0;
    private bool animationFinished;

    void Start()
    {
        savedStats = GameObject.Find("GameStateData").GetComponent<CharacterStats>();
        if (healthCrystal) {
            crystalBonusValue = 20;
            SetCrystalLevels(GameData.Instance.HealhCrystalTotal);
            oldCrystals = GameData.Instance.HealhCrystalTotal;
            newCrystals = savedStats.HealthCrystalsGained;
                }
        if (manaCrystal)
        {
            crystalBonusValue = 20;
            SetCrystalLevels(GameData.Instance.ManaCrystalTotal);
            oldCrystals = GameData.Instance.ManaCrystalTotal;
            newCrystals = savedStats.ManaCrystalsGained;
        }
        if (attackCrystal)
        {
            crystalBonusValue = 4;
            SetCrystalLevels(GameData.Instance.AttackCrystalTotal);
            oldCrystals = GameData.Instance.AttackCrystalTotal;
            newCrystals = savedStats.AttackCrystalsGained;
        }
        if (defenseCrystal)
        {
            crystalBonusValue = 2;
            SetCrystalLevels(GameData.Instance.DefenseCrystalTotal);
            oldCrystals = GameData.Instance.DefenseCrystalTotal;
            newCrystals = savedStats.defenseCrystalsGained;

        }
        //startingCrystals.text = "0/100";
        // crystalBonusText.text = "+60";
        // crystalPanel.transform.localScale = new Vector3(.5f,1,1) ;
        // if (manaCrystal) crystalBonusText.text = "+40";

        //Make functions in CrystalBuffManager to pull the next bonus level from crystals and apply them as needed. 
        //Make something to slowly fill bar once determined the difference in levels, including when doing more than one level at once. Maybe it does one level at a time, 
        //aka recalc bar size upon 'load' and upon hitting a new 'tier'. 
    }

    // Update is called once per frame
    void Update()
    {
        if (animationFinished) { return; }
        timerUntilAnimation -= Time.deltaTime;
        if (timerUntilAnimation >= 0) { return; }
        AnimateCrystalBarsFilling();
    }

    private void AnimateCrystalBarsFilling()
    {
        if (timerAnimation >= 0) {
            timerAnimation -= Time.deltaTime;
            return;
        }
        else { timerAnimation = timerAnimationReset; }

        if (newCrystals > 0) {
            int crystalsToAdd = (int)(crystalUpgrades[cTier] * barAnimationPerFrame);
            if (crystalsToAdd+ oldCrystals > crystalTiers[cTier]) {
                crystalsToAdd = crystalTiers[cTier] - oldCrystals;
            }
            if (crystalsToAdd > newCrystals) {
                crystalsToAdd = newCrystals;
            }
            oldCrystals += crystalsToAdd;
            newCrystals -= crystalsToAdd;
            SetCrystalLevels(oldCrystals);
        }
        else { animationFinished = true; }
        
    }

    private void SetCrystalLevels(int crystalTotal)
    {
        cTier = GetCrystalTier(crystalTotal);
        string bonusText = "+" + cTier* crystalBonusValue;
        crystalBonusText.text = bonusText;
        int crystalsInTier = crystalTotal-(crystalTiers[cTier]- crystalUpgrades[cTier]);
        int crystalsNeededToLevel = crystalUpgrades[cTier];
        string crystalTotalText = "" + crystalsInTier + "/" + crystalsNeededToLevel;
        startingCrystals.text = crystalTotalText;
        crystalPanel.transform.localScale = new Vector3((float)crystalsInTier/crystalsNeededToLevel, 1, 1);

    }

    //Setup Crystal Buffs

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
        GameData.Instance.DefenseCrystalBonus = 2 * i;
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
        GameData.Instance.AttackCrystalBonus = 4 * i;

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
        GameData.Instance.ManaCrystalBonus = 20 * i;
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
        GameData.Instance.HealhCrystalBonus = 20 * i;
    }

}
