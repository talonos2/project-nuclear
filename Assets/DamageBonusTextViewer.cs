using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageBonusTextViewer : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro powerBonusToShow;
    private int currentPowerSelected;
    private int iceBuff;
    private int earthBuff;
    private int fireBuff;
    private int airBuff;
    private CharacterStats playerStats;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        currentPowerSelected = playerStats.currentPower;
        setBuffs();
        UpdateBonusText();
    }

    private void setBuffs() {
        airBuff = (int)playerStats.accessoryAirBonus;
        iceBuff = (int)playerStats.accessoryIceBonus;
        earthBuff = (int)playerStats.accessoryEarthBonus;
        fireBuff = (int)playerStats.accessoryFireBonus;
    }

    private void UpdateBonusText()
    {
        if (currentPowerSelected == 0) { powerBonusToShow.text = ""; }

        if (currentPowerSelected == 1) { powerBonusToShow.text = "+"+(25+ iceBuff) +"%\nDMG"; }
        if (currentPowerSelected == 2) { powerBonusToShow.text = "+" + (50 + earthBuff) + "%\nDMG"; }
        if (currentPowerSelected == 3) { powerBonusToShow.text = "+" + (75 + fireBuff) + "%\nDMG"; }
        if (currentPowerSelected == 4) { powerBonusToShow.text = "+" + (100 + airBuff) + "%\nDMG"; }

    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.currentPower != currentPowerSelected) {
            currentPowerSelected = playerStats.currentPower;
            UpdateBonusText();
        }
        if (playerStats.accessoryAirBonus != airBuff) {
            setBuffs();
            UpdateBonusText(); 
        }
        if (playerStats.accessoryEarthBonus != earthBuff) {
            setBuffs();
            UpdateBonusText();
        }
        if (playerStats.accessoryFireBonus != fireBuff) {
            setBuffs();
            UpdateBonusText();
        }
        if (playerStats.accessoryIceBonus != iceBuff) {
            setBuffs();
            UpdateBonusText();
        }





    }
}
