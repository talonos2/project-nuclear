using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Todd : Enemy
{
    new void Start()
    {
        base.Start();
        if (GameData.Instance.Todd == 0 || GameData.Instance.bestTimes[8] == 0 || GameData.Instance.RunNumber <= 4)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Todd = 0;

    }
}
