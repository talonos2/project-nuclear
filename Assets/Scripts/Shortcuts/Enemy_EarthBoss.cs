using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_EarthBoss : Enemy
{
    public ShortcutCutscene1_3 shortcutScene;
    public override void doUponDeath()
    {
        if (earthBoss2) {
            shortcutScene.initialiseShortcutCutscene();
        }
        
    }
}
