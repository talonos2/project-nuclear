using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUIOn : MonoBehaviour
{
    public GameObject[] stuffToTurnOff;
    private bool uiOffScene;
    public GameObject lowerLeftAligned;
    public GameObject lowerRightAligned;

    public float flat = 12;
    public float scalar = 6.5f;


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
            uiToTurnOff.GetComponentInChildren<MissingVillagerDropdownController>().HideCharDeathUI();
            uiToTurnOff.GetComponentInChildren<FloorNameDropdownController>().HideFloorNameUI();
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
            uiToTurnOff.GetComponentInChildren<MissingVillagerDropdownController>().HideCharDeathUI();
            uiToTurnOff.GetComponentInChildren<FloorNameDropdownController>().HideFloorNameUI();
        }
    }

    public void turnOnUi()
    {
        GameData.Instance.UI_On = true;
        foreach (GameObject t in stuffToTurnOff)
        {
            t.SetActive(true);
        }
        GameObject uiToTurnOff = GameObject.Find("EscapeMenuUi");
        if (uiToTurnOff)
        {
            uiToTurnOff.GetComponentInChildren<ShowItemsInMenuController>().ShowItemUI();
            uiToTurnOff.GetComponentInChildren<MissingVillagerDropdownController>().ShowCharDeathUI();
            uiToTurnOff.GetComponentInChildren<FloorNameDropdownController>().ShowFloorNameUI();
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

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        lowerLeftAligned.transform.localPosition = new Vector3(flat - scalar*aspectRatio,0,0);
        lowerRightAligned.transform.localPosition = new Vector3(-8.65f - (1.777777778f*scalar) + scalar*aspectRatio, -4.742f, 1);
    }
}
