using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissingVillagerDropdownController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inDungeonUi;
    public float animateSpeed = 10;
    private float delayBeforeClose = 0f;
    public bool animateOpen;
    public bool animateClose;
    public bool inPauseMenu;
    public TextMeshProUGUI villagersStatus;
    public GameObject panelToDropDown;
    private float speedToAnimate;
    void Start()
    {
        updateVillagerStatusText();
        if (GameData.Instance.RunNumber == 1)
        {
            panelToDropDown.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.fullPause || GameState.isInBattle||GameData.Instance.isInDialogue||GameData.Instance.isCutscene) {
            return;
        }
        speedToAnimate = animateSpeed * Time.deltaTime * 30;
        HandleAnimateOpen();
        HandleAnimateClose();
    }

    private void HandleAnimateOpen()
    {
        //Open is 307
        if (animateOpen)
        {
            panelToDropDown.transform.localPosition = new Vector3(panelToDropDown.transform.localPosition.x, panelToDropDown.transform.localPosition.y- speedToAnimate, panelToDropDown.transform.localPosition.z);
            if (panelToDropDown.transform.localPosition.y < 307)
            {
                panelToDropDown.transform.localPosition = new Vector3(panelToDropDown.transform.localPosition.x, 307, panelToDropDown.transform.localPosition.z);
                animateOpen = false;
            }
        }
    }

    private void HandleAnimateClose()
    {
        //closed is 373
        if (delayBeforeClose > 0) {
            delayBeforeClose -= Time.deltaTime;
            if (delayBeforeClose <= 0) {
                animateClose = true;
            }
            return;
        }
        if (animateClose)
        {
            panelToDropDown.transform.localPosition = new Vector3(panelToDropDown.transform.localPosition.x , panelToDropDown.transform.localPosition.y + speedToAnimate, panelToDropDown.transform.localPosition.z);
            if (panelToDropDown.transform.localPosition.y > 373)
            {
                panelToDropDown.transform.localPosition = new Vector3(panelToDropDown.transform.localPosition.x, 373, panelToDropDown.transform.localPosition.z);
                animateClose = false;
            }
        }
    }

    public void updateVillagerStatusText() {
        int totalVillagersLost = GameData.Instance.RunNumber - 1;
        int villagersDead = GameData.Instance.VillagersDead();
        int villagersMissing=totalVillagersLost-villagersDead;
        villagersStatus.text = "Villagers Missing: " + villagersMissing + "\nVillagers Dead: " + villagersDead;
    }

    public void SetAnimateUponVillagerDeath() {
        updateVillagerStatusText();
        animateOpen = true;
        animateClose = false;
        delayBeforeClose = 2f;
    }
    public void SetToAnimateOpen()
    {
        //onMouseEnter
        animateOpen = true;
        animateClose = false;
    }
    
    public void SetToAnimateClose()
    {
        //onMouseExit
        animateClose = true;
        animateOpen = false;
    }
}
