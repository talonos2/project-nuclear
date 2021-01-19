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
    private CharacterStats playerStats;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        currentPowerSelected = playerStats.currentPower;
        UpdateBonusText();
    }

    private void UpdateBonusText()
    {
        if (currentPowerSelected == 0) { powerBonusToShow.text = ""; }
        if (currentPowerSelected == 1) { powerBonusToShow.text = "+25%\nDMG"; }
        if (currentPowerSelected == 2) { powerBonusToShow.text = "+50%\nDMG"; }
        if (currentPowerSelected == 3) { powerBonusToShow.text = "+75%\nDMG"; }
        if (currentPowerSelected == 4) { powerBonusToShow.text = "+100%\nDMG"; }

    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.currentPower != currentPowerSelected) {
            currentPowerSelected = playerStats.currentPower;
            UpdateBonusText();
        }
    }
}
