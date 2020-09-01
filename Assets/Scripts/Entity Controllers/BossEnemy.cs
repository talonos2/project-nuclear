using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public override void doUponDeath()
    {
        GameObject bossController = GameObject.Find("BossSpawnController");
        if (bossController != null)
        {
            bossController.GetComponent<bossSpawnController>().spawnNextBoss();
        }
    }
}
