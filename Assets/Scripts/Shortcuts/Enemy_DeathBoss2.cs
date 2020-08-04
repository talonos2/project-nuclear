using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBoss2 : Enemy
{
    public virtual void doUponDeath()
    {
        GameData.Instance.map4_1Shortcut = true;
        //Will do nothing unless a child script changes this. Called by Combat. 
    }

}
