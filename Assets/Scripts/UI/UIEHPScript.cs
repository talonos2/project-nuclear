using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEHPScript : MonoBehaviour
{
    public Image bar;

    public Enemy enemyStats;
    public bool shouldBeTracking;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBeTracking)
        {
            bar.fillAmount = ((float)enemyStats.HP / (float)enemyStats.MaxHP);
            if (enemyStats.HP <= 0)
            {
                shouldBeTracking = false;
            }
        }
    }

    internal void BindToMonster(Enemy monsterStats)
    {
        this.enemyStats = monsterStats;
        this.shouldBeTracking = true;
    }
}