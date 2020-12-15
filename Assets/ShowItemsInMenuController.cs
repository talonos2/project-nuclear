using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowItemsInMenuController : MonoBehaviour
{
    public ItemHolderUI weaponUIPrefab;
    public ItemHolderUI armorUIPrefab;
    public ItemHolderUI accessoryUIPrefab;
    public TextMeshProUGUI itemDetailsDescription;
    public GameObject picToMove;
    private RectTransform recTranOfPic;
    private bool openForItemSelection;
    private bool flashFinished;
    private CharacterStats savedStats;
    public bool inDungeonUi;
    public float animateSpeed=10;
    private float delayBeforeClose = 0f;
    private bool animateOpen;
    private bool animateClose;

    // Start is called before the first frame update
    void Start()
    {
        savedStats = GameObject.Find("GameStateData").GetComponent<CharacterStats>();
        weaponUIPrefab.SetItem(savedStats.weapon, false);
        armorUIPrefab.SetItem(savedStats.armor, false);
        accessoryUIPrefab.SetItem(savedStats.accessory, false);
        if (!inDungeonUi) { setDescriptionText(0); }
        if (inDungeonUi) { recTranOfPic = picToMove.GetComponent<RectTransform>(); }
       
    }

    public void setDescriptionText(int slotNum) {
        weaponUIPrefab.flashingBackground.enabled = false;
        armorUIPrefab.flashingBackground.enabled = false;
        accessoryUIPrefab.flashingBackground.enabled = false;

        if (slotNum == 0) {
            itemDetailsDescription.text = weaponUIPrefab.getItemDetails()+" "+weaponUIPrefab.itemStatText.text;
            weaponUIPrefab.flashingBackground.enabled = true;
        }
        if (slotNum == 1) {
            itemDetailsDescription.text = armorUIPrefab.getItemDetails() + " " + armorUIPrefab.itemStatText.text;
            armorUIPrefab.flashingBackground.enabled = true;
        }
        if (slotNum == 2)
        {
            itemDetailsDescription.text = accessoryUIPrefab.getItemDetails() + " " + accessoryUIPrefab.itemStatText.text;
            accessoryUIPrefab.flashingBackground.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!inDungeonUi || GameState.fullPause) {
            return;
        }
        if (delayBeforeClose > 0) {
            delayBeforeClose -= Time.deltaTime;
            if (delayBeforeClose < 1 && !flashFinished) {
                weaponUIPrefab.flashingBackground.enabled = false;
                armorUIPrefab.flashingBackground.enabled = false;
                accessoryUIPrefab.flashingBackground.enabled = false;
                flashFinished = true;
            }
            if (delayBeforeClose < 0) {
                openForItemSelection = false;
            }

        }
        if (openForItemSelection) { return; }
        //-610 to -435 (open)
        float speedToAnimate = animateSpeed * Time.deltaTime*10;
        if (animateClose) {
            picToMove.transform.localPosition = new Vector3(picToMove.transform.localPosition.x - speedToAnimate, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
            if (picToMove.transform.localPosition.x < -610)
            {
                picToMove.transform.localPosition = new Vector3(-610, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
                animateClose = false;
                weaponUIPrefab.flashingBackground.enabled = false;
                armorUIPrefab.flashingBackground.enabled = false;
                accessoryUIPrefab.flashingBackground.enabled = false;
            }
        }
        if (animateOpen) {
            picToMove.transform.localPosition = new Vector3(picToMove.transform.localPosition.x + speedToAnimate, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
            if (picToMove.transform.localPosition.x > -435) {
                picToMove.transform.localPosition = new Vector3(-435, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
                animateOpen = false;
            }
        }
        
    }

    public void SetToAnimateOpen() {

            animateOpen = true;
            animateClose = false;       

    }
    public void SetToAnimateClose() {

            animateClose = true;
            animateOpen = false;

    }
    public void OpenForFoundItemSelection() {
        picToMove.transform.localPosition = new Vector3(-435, picToMove.transform.localPosition.y, picToMove.transform.localPosition.z);
        openForItemSelection = true;
    }
    public void ShowSelectedItemAndClose(int itemSelected) {

        delayBeforeClose = 1.5f;
        flashFinished=false;
        animateClose = true;
        if (itemSelected ==0) {
            weaponUIPrefab.SetItem(savedStats.weapon, false);
            weaponUIPrefab.flashingBackground.enabled = true;
        }
        if (itemSelected == 1) {
            armorUIPrefab.SetItem(savedStats.armor, false);
            armorUIPrefab.flashingBackground.enabled = true;
        }
        if (itemSelected == 2)
        {
            accessoryUIPrefab.SetItem(savedStats.accessory, false);
            accessoryUIPrefab.flashingBackground.enabled = true;
        }

    }


}
