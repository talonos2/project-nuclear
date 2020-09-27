using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Sara : Enemy
{
    new void Start()
    {
        base.Start();
        if (GameData.Instance.Sara == 0 || GameData.Instance.bestTimes[13] == 0 || GameData.Instance.RunNumber <= 2)
       {
            Destroy(this.gameObject);
       }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Sara = 0;

    }
}
