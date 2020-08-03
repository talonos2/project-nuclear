using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermSwitch5_4 : SwitchEntityData
{

    public MeshRenderer shortcutBridge1;
    public MeshRenderer shortcutBridge2;
    public MeshRenderer shortcutBridge3;
    private bool alreadyActivated;
    new void Start()
    {
        base.Start();
        if (GameData.Instance.map5_4Shortcut)
        {
            ToggleTiedObjects();

        }
    }

    public override void ToggleTiedObjects()
    {

        if (activeSwitch)
        {
            if (!alreadyActivated) SoundManager.Instance.PlaySound("Bridge", 1);
            alreadyActivated = true;
            shortcutBridge1.enabled = true;
            shortcutBridge2.enabled = true;
            shortcutBridge3.enabled = true;
            GameData.Instance.map5_4Shortcut = true;
            foreach (GameObject tiedEntity in TiedEntities)
            {
                BridgeController bridgeControlled = tiedEntity.GetComponent<BridgeController>();

                if (bridgeControlled != null)
                {
                    if (bridgeControlled.isPlatformTerrain) { bridgeControlled.RemovePlatform(); }
                    else { bridgeControlled.AddPlatform(); }

                }
            }
            activeSwitch = false;
            SwitchAnimation();
        }

    }
}
