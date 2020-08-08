using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermSwitch4_3 : SwitchEntityData
{

    public ShortcutCutsceneMap4_3 shortcutCS4_3;
    new void Start()
    {
        base.Start();
        if (GameData.Instance.map4_3Shortcut)
        {
            ToggleTiedObjects();
        }
    }

    public override void ToggleTiedObjects()
    {

        if (activeSwitch)
        {

            SwitchAnimation();
            if (GameData.Instance.map4_3Shortcut == false)
            {
                GameData.Instance.map4_3Shortcut = true; //Set to True by the cutsceneplayerscript on scMap3-2
                shortcutCS4_3.initialiseShortcutCutscene();
                //SceneManager.LoadScene("ScMap3-2", LoadSceneMode.Additive);
            }
            activeSwitch = false;

        }

    }
}
