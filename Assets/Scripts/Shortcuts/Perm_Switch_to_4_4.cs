using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perm_Switch_to_4_4 : SwitchEntityData
{
    public ShortcutCutsceneMap4_4 shortcutToLoad;
    new void Start()
    {
        base.Start();
        if (GameData.Instance.map4_4Shortcut)
        {
            ToggleTiedObjects();
        }
    }

    public override void ToggleTiedObjects()
    {

        if (activeSwitch)
        {

            SwitchAnimation();
            if (GameData.Instance.map4_4Shortcut == false)
            {
                shortcutToLoad.initialiseShortcutCutscene();
                //GameData.Instance.map4_4Shortcut = true;

                //SceneManager.LoadScene("ScMap3-2", LoadSceneMode.Additive);
            }
            activeSwitch = false;

        }

    }
}
