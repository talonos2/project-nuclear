using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcut2_2controller : MonoBehaviour
{
    public bool onShortcutMap;
    public bool alwaysOn = false;
    public bool exitTeleporter;
    public Shortcut2_2controller shortcutToActivate;
    public GameObject sprite1;
    public GameObject sprite2;
    void Start()
    {
        if (onShortcutMap) return;
        if (!GameData.Instance.map2_2Shortcut&&!alwaysOn&&!exitTeleporter)
        {
            ExitController exit = gameObject.GetComponent<ExitController>();
            if (exit)
            {
                exit.removeExit();
            }            
            //Destroy(this.gameObject);
        }

        if (GameData.Instance.map2_2Shortcut) {
            sprite1.SetActive(true);
            sprite2.SetActive(true);
        }

    }

    public void setupShortcut() {
        shortcutToActivate.sprite1.SetActive(true);
        shortcutToActivate.sprite2.SetActive(true);// = true;
    }

}
