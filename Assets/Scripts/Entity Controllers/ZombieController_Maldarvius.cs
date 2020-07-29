using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Maldarvius : Enemy
{
    new void Start()
    {
        base.Start();
        if (GameData.Instance.Melvardius == 0 || GameData.Instance.bestTimes[11] == 0 || GameData.Instance.RunNumber <= 7)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Melvardius = 0;

    }
}
