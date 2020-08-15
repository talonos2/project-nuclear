using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcut2_2controller : MonoBehaviour
{
    public bool onShortcutMap;
    public Shortcut2_2controller shortcutToActivate;
    void Start()
    {
        if (onShortcutMap) return;
        if (!GameData.Instance.map2_2Shortcut)
        {
            ExitController exit = gameObject.GetComponent<ExitController>();
            if (exit)
            {
                exit.removeExit();
            }
            Destroy(this.gameObject);
        }
        else { this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true; }

    }

    public void setupShortcut() {
        shortcutToActivate.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
    }

}
