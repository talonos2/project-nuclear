﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Devon : Enemy
{
    new void Start()
    {
        base.Start();
        if (GameData.Instance.Devon == 0 || GameData.Instance.bestTimes[16] == 0 || GameData.Instance.RunNumber <= 9)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Devon = 0;

    }
}