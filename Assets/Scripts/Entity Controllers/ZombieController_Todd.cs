using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Todd : Enemy
{
    new void Start()
    {
        base.Start();
        if (testMonster)
        {
            return;
        }
        if (GameData.Instance.Todd == 0 || GameData.Instance.bestTimes[9] > 600 || GameData.Instance.RunNumber <= 4)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.KILL_VILLAGER);
        GameData.Instance.Todd = 0;
        GameObject.Find("Canvas_VillagersMissing").GetComponent<MissingVillagerDropdownController>().SetAnimateUponVillagerDeath();
        if (GameData.Instance.RunNumber==14)
        {
            FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.KILL_TODD_AS_TEDD);
        }
    }
}
