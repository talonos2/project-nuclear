using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Mara : Enemy
{
    new void Start()
    {
        base.Start();
        if (GameData.Instance.Mara == 0 || GameData.Instance.bestTimes[15] == 0 || GameData.Instance.RunNumber <= 8)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Mara = 0;

    }
}
