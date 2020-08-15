using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorShortcutController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject openedDoor;
    public GameObject door1toOpen;
    public GameObject door2toOpen;
    public bool onCutseneMap;
    void Start()
    {
        if (GameData.Instance.map3_2Shortcut) {
            Instantiate(openedDoor, this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void OpenDoors() {
            Instantiate(openedDoor, this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(this.gameObject);
        
    }

    public void OpenCutsceneDoors() {
        GameObject doorOpened =Instantiate(openedDoor, this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        doorOpened.GetComponent<DoodadData>().isOnCutsceneMap = true;
        doorOpened.GetComponentInChildren<SpriteShadowLoader>().isOnCutsceneMap = true;
        Destroy(this.gameObject);
    }

    internal void setupShortcut()
    {
        door1toOpen.GetComponent<DoorShortcutController>().OpenCutsceneDoors();
        door2toOpen.GetComponent<DoorShortcutController>().OpenCutsceneDoors();
    }
}
