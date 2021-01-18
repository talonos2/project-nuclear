using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Sara : Enemy
{
    new void Start()
    {
        base.Start();
        if (testMonster)
        {
            return;
        }
        if (GameData.Instance.Sara == 0 || GameData.Instance.bestTimes[13] > 600 || GameData.Instance.RunNumber <= 2)
       {
            Destroy(this.gameObject);
       }
    }

    override public void doUponDeath()
    {
        FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.KILL_VILLAGER);
        GameData.Instance.Sara = 0;
        GameObject.Find("Canvas_VillagersMissing").GetComponent<MissingVillagerDropdownController>().SetAnimateUponVillagerDeath();
    }
}
