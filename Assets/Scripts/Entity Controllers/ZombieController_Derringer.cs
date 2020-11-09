using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Derringer : Enemy
{
    new void Start()
    {
        base.Start();
        if (testMonster)
        {
            return;
        }
        if (GameData.Instance.Derringer == 0 || !GameData.Instance.map3_3Shortcut || GameData.Instance.RunNumber <= 6)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Derringer = 0;

    }
}
