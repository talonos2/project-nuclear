using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map3_3ShortcutController : MonoBehaviour
{
    // Start is called before the first frame update
    public BridgeController bridgeShortcut;
    void Start()
    {
        if (GameData.Instance.map3_3Shortcut) {
            bridgeShortcut.SwapPlatform();
        }
        else {
            bridgeShortcut.RemovePlatform();
        }
        
    }

    internal void setupShortcutForCutscene()
    {
        if (GameData.Instance.map3_3Shortcut)
        {
            bridgeShortcut.SwapPlatform();
        }
        else
        {
            bridgeShortcut.RemovePlatform();
        }
    }
}
