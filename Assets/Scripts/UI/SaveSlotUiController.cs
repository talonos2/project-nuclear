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

        
        if (runNumber==1) characterPortrait.sprite = portraits.bustList[0];
        if (runNumber == 2) characterPortrait.sprite = portraits.bustList[2];  
        if (runNumber >= 3 && runNumber <= 5) characterPortrait.sprite = portraits.bustList[5];  
        if (runNumber >= 6 && runNumber <= 10) characterPortrait.sprite = portraits.bustList[10];  
        if (runNumber >= 11 && runNumber <= 12) characterPortrait.sprite = portraits.bustList[12];  
        if (runNumber >= 13 && runNumber <= 14) characterPortrait.sprite = portraits.bustList[14];  
        if (runNumber >= 15 && runNumber <= 17) characterPortrait.sprite = portraits.bustList[17]; 
        if (runNumber == 18) characterPortrait.sprite = portraits.bustList[18]; 
        if (runNumber >= 19 && runNumber <= 24) characterPortrait.sprite = portraits.bustList[24]; 
        if (runNumber >= 25 && runNumber <= 29) characterPortrait.sprite = portraits.bustList[29];  
        if (runNumber == 30) characterPortrait.sprite = portraits.bustList[30]; 



    }

}
