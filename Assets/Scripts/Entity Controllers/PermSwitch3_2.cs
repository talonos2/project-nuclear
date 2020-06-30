using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PermSwitch3_2 : SwitchEntityData
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (GameData.Instance.map3_2Shortcut)
        {
            ToggleTiedObjects();
        }
    }

    public override void ToggleTiedObjects()
    {

        if (activeSwitch)
        {
  
            SwitchAnimation();
            if (GameData.Instance.map3_2Shortcut == false) {
                GameData.Instance.map3_2Shortcut = true; //Set to True by the cutsceneplayerscript on scMap3-2

                //SceneManager.LoadScene("ScMap3-2", LoadSceneMode.Additive);
            }
            activeSwitch = false;

        }

    }
}
