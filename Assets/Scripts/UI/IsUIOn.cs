using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUIOn : MonoBehaviour
{
    public GameObject[] stuffToTurnOff;

    public void turnOffUi() {
        GameData.Instance.UI_On = false;
        foreach (GameObject t in stuffToTurnOff)
        {
            t.SetActive(false);
        }
    }

    public void turnOnUi() {
        GameData.Instance.UI_On = true;
        foreach (GameObject t in stuffToTurnOff)
        {
            t.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.isInDialogue && !GameData.Instance.IsInTown() && GameData.Instance.UI_On && !GameData.Instance.ManualUIToggleOff) {
            //GameState.fullPause = true;
            turnOffUi();
        }

        if (!GameData.Instance.IsInTown() && !GameData.Instance.isInDialogue && !GameData.Instance.UI_On && !GameData.Instance.ManualUIToggleOff) {
            //GameState.fullPause = false;
            turnOnUi();
        }

        if (GameData.Instance.IsInTown() && !GameData.Instance.UI_On) {
            turnOffUi();
        }

    }
}
