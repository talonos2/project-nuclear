using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shortcut4_4controller : MonoBehaviour
{
    public bool shortcutHolder;
    public shortcut4_4controller bridge1;
    public shortcut4_4controller bridge2;
    public shortcut4_4controller bridge3;
    void Start()
    {
        if (shortcutHolder) return;
        if (GameData.Instance.map4_4Shortcut)
        {
            this.gameObject.GetComponent<BridgeController>().AddPlatform();
        }
    }

    public void setupShortcut() {
        bridge1.gameObject.GetComponent<BridgeController>().AddPlatform();
        bridge2.gameObject.GetComponent<BridgeController>().AddPlatform();
        bridge3.gameObject.GetComponent<BridgeController>().AddPlatform();
    }

}
