using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Norma : Enemy
{
    new void Start()
    {
        base.Start();
        if (testMonster)
        {
            return;
        }
        if (GameData.Instance.Norma == 0 || GameData.Instance.bestTimes[15] == Mathf.Infinity || GameData.Instance.RunNumber <= 5)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Norma = 0;

    }
}
