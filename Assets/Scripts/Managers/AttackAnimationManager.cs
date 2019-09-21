﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : Singleton<AttackAnimationManager>
{

    public Enemy monsterStats;
    public CharacterStats playerStats;

    public float initialHopDuration = .35f;
    public float enemyKnockBackStart = .35f;
    public float enemyKnockBackDuration=.1f;
    public float enemySpringbackStart=.45f;
    public float enemySpringbackDuration=.05f;
    public float enemyMoveBackStart=.7f;
    public float enemyMoveBackDuration=.2f;
    public float returnHopStart=.6f;
    public float returnHopDuration=.6f;

    public float initialHopHeight = .5f;
    public float returnHopHeight = .05f;
    public float knockBackXOffset = .15f;
    public float springBackXOffset = .1f;

    public float defaultAttackFrameTime = .1f;
    public float timeWarp = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCombatPawns(Enemy monsterStats, CharacterStats playerStats)
    {
        this.monsterStats = monsterStats;
        this.playerStats = playerStats;
    }
}