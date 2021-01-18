using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_Hiro : Enemy
{
    new void Start()
    {
        base.Start();
        if (testMonster) {
            return;
        }

        if (GameData.Instance.Douglass == 0 || !GameData.Instance.map5_2Shortcut || GameData.Instance.RunNumber <= 10)
        {
            Destroy(this.gameObject);
        }
    }

    override public void doUponDeath()
    {
        FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.KILL_VILLAGER);
        GameData.Instance.Douglass = 0;
        GameObject.Find("Canvas_VillagersMissing").GetComponent<MissingVillagerDropdownController>().SetAnimateUponVillagerDeath();

    }
}
