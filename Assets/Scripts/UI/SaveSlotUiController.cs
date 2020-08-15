using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUiController : MonoBehaviour
{

    public Image iceOn;
    public Image earthOn;
    public Image fireOn;
    public Image airOn;
    public Image characterPortrait;
    public TextMeshProUGUI saveSlotText;
    public TextMeshProUGUI dayNumberText;
    public PortraitHolder portraits;
    public GameObject savingOverlay;

    public void SetupSaveSlotUI(bool ice, bool earth, bool fire, bool air, int runNumber, int saveSlotNumber) {
        if (!ice)
        {
            iceOn.enabled = false;
        }
        else iceOn.enabled = true;

        if (!earth)
        {
            earthOn.enabled = false;
        }
        else earthOn.enabled = true;

        if (!fire)
        {
            fireOn.enabled = false;
        }
        else fireOn.enabled = true;

        if (!air)
        {
            airOn.enabled = false;
        }
        else airOn.enabled = true;
        if (runNumber != 0) dayNumberText.text = "Day " + runNumber;
        else dayNumberText.text = "";
        if (saveSlotNumber == 0)
        {
            saveSlotText.text = "Auto Save";
        }
        else {
            saveSlotText.text = "Save Slot " + saveSlotNumber;
        }
        if (runNumber==0) characterPortrait.sprite = portraits.bustList[runNumber];
        else characterPortrait.sprite = portraits.bustList[runNumber];

    }

}
