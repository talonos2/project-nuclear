using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perm_Switch2_2 : SwitchEntityData
{
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
                GameData.Instance.map2_2Shortcut = true; //Set to True by the cutsceneplayerscript on scMap3-2

                //SceneManager.LoadScene("ScMap3-2", LoadSceneMode.Additive);
            }
            activeSwitch = false;

        }

    }
}
