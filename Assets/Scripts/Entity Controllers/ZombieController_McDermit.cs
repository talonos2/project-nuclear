using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_McDermit : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (GameData.Instance.McDermit == 0 || GameData.Instance.bestTimes[6]==0 || GameData.Instance.RunNumber <=3) {
            Destroy(this.gameObject);
        }   
    }

    override public void doUponDeath()
    {
        GameData.Instance.McDermit = 0;

    }



}
