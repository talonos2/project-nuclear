using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Todd : Enemy
{
    new void Start()
    {
        base.Start();
        if (testMonster)
        {
            return;
        }
        if (GameData.Instance.Todd == 0 || GameData.Instance.bestTimes[9] == Mathf.Infinity || GameData.Instance.RunNumber <= 4)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Todd = 0;

    }
}
