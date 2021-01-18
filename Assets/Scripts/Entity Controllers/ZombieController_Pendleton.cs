using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Pendleton : Enemy
{
    new void Start()
    {
        base.Start();
        if (testMonster)
        {
            return;
        }
        if (GameData.Instance.Pendleton == 0 || !GameData.Instance.map4_3Shortcut || GameData.Instance.RunNumber <= 10)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.KILL_VILLAGER);
        GameData.Instance.Pendleton = 0;
        GameObject.Find("Canvas_VillagersMissing").GetComponent<MissingVillagerDropdownController>().SetAnimateUponVillagerDeath();

    }
}
