using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Pendleton : Enemy
{
    new void Start()
    {
        base.Start();
        if (GameData.Instance.Pendleton == 0 || !GameData.Instance.map4_3Shortcut || GameData.Instance.RunNumber <= 10)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        GameData.Instance.Pendleton = 0;

    }
}
