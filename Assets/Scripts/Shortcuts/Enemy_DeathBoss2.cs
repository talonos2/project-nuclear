using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBoss2 : Enemy
{
    public ShortcutCutscene4_1 shortcutToRun;
    public override void doUponDeath()
    {
        shortcutToRun.initialiseShortcutCutscene();
        //Will do nothing unless a child script changes this. Called by Combat. 
    }

}
