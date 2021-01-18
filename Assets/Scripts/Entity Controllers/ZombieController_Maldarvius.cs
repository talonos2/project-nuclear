using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Maldarvius : Enemy
{
    new void Start()
    {
        base.Start();
        if (testMonster)
        {
            return;
        }
        if (GameData.Instance.Melvardius == 0 || GameData.Instance.bestTimes[12] > 600 || GameData.Instance.RunNumber <= 7)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.KILL_VILLAGER);
        GameData.Instance.Melvardius = 0;
        GameObject.Find("Canvas_VillagersMissing").GetComponent<MissingVillagerDropdownController>().SetAnimateUponVillagerDeath();

    }
}
