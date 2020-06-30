using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perm_Switch_to_4_4 : SwitchEntityData
{
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
                GameData.Instance.map4_4Shortcut = true; //Set to True by the cutsceneplayerscript on scMap3-2

                //SceneManager.LoadScene("ScMap3-2", LoadSceneMode.Additive);
            }
            activeSwitch = false;

        }

    }
}
