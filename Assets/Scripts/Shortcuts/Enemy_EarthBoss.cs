using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_EarthBoss : Enemy
{
    public ShortcutCutsceneMap1_3to2_3 shortcutScene;

    private new void Start()
    {
        base.Start();
        if (GameData.Instance.earthBoss2 ) {
            Destroy(this.gameObject);
        }

    }
    public override void doUponDeath()
    {
        
        if (earthBoss2) {
            GameData.Instance.earthBoss2 = true;
            shortcutScene.initialiseShortcutCutscene();
        }
        
    }
}
