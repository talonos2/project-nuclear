using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUIOn : MonoBehaviour
{
    public GameObject[] stuffToTurnOff;
    private bool uiOffScene;

    void Start()
    {
        if (GameData.Instance.IsInTown())
        {
            turnOffUi();
        }
    }
    public void turnOffUi() {
        GameData.Instance.UI_On = false;
        foreach (GameObject t in stuffToTurnOff)
        {
            t.SetActive(false);
        }
        GameObject uiToTurnOff = GameObject.Find("EscapeMenuUi");
        if (uiToTurnOff) {
            uiToTurnOff.GetComponentInChildren<ShowItemsInMenuController>().HideItemUI();
        }
    }

    public void turnOffUiScene() {
        uiOffScene = true;
        GameData.Instance.UI_On = false;
        foreach (GameObject t in stuffToTurnOff)
        {
            t.SetActive(false);
        }
        GameObject uiToTurnOff = GameObject.Find("EscapeMenuUi");
        if (uiToTurnOff)
        {
            uiToTurnOff.GetComponentInChildren<ShowItemsInMenuController>().HideItemUI();
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
        if (uiOffScene) {
            return;
        }
        if (GameData.Instance.isInDialogue && !GameData.Instance.IsInTown() && GameData.Instance.UI_On && !GameData.Instance.ManualUIToggleOff) {
            //GameState.fullPause = true;
            turnOffUi();
        }

        if (!GameData.Instance.IsInTown() && !GameData.Instance.isInDialogue && !GameData.Instance.UI_On && !GameData.Instance.ManualUIToggleOff) {
            //GameState.fullPause = false;
            turnOnUi();
        }
        //Debug.Log("is in town "+ GameData.Instance.IsInTown() + "UIOn " + GameData.Instance.UI_On);
        if (GameData.Instance.IsInTown() && GameData.Instance.UI_On) {
            turnOffUi();
        }



    }
}
