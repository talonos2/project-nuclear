using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perm_Switch2_2 : SwitchEntityData
{

    public shortcutCutsceneMap4_4to2_2s shortcutCutscene;
    new void Start()
    {
        base.Start();
        if (GameData.Instance.map2_2Shortcut)
        {
            ToggleTiedObjects();
        }
    }

    public override void ToggleTiedObjects()
    {

        if (activeSwitch)
        {

            SwitchAnimation();
            if (GameData.Instance.map2_2Shortcut == false)
            {
                shortcutCutscene.initialiseShortcutCutscene();

                //SceneManager.LoadScene("ScMap3-2", LoadSceneMode.Additive);
            }
            activeSwitch = false;

        }

    }
}
