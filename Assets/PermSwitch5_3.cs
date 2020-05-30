using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermSwitch5_3 : SwitchEntityData
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (GameData.Instance.map5_3Shortcut) {
            ToggleTiedObjects();
        }
    }

     public override void ToggleTiedObjects() {
        
        if (activeSwitch)
        {
            GameData.Instance.map5_3Shortcut = true;
            foreach (GameObject tiedEntity in TiedEntities)
            {
                SpikeController spikeControlled = tiedEntity.GetComponent<SpikeController>();
                BridgeController bridgeControlled = tiedEntity.GetComponent<BridgeController>();
                if (spikeControlled != null)
                {
                    spikeControlled.isPassable = true;
                    spikeControlled.LowerSpikeAnimation();
                }
                if (bridgeControlled != null)
                {
                    if (bridgeControlled.isPlatformTerrain) { bridgeControlled.removePlatform(); }
                        else { bridgeControlled.addPlatform(); }
                    
                }
            }
            activeSwitch = false;
            SwitchAnimation();
        }

    }

}
