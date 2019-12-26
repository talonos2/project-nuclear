using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentSwitchEntityData : SwitchEntityData
{
    protected GameData gameData;
    public bool map5_2Shortcut;
    public bool map5_3Shortcut;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        gameData = GameData.Instance;
        if (map5_3Shortcut&&gameData.map5_3Shortcut) {
            ToggleTiedObjects();
        }
        if (map5_2Shortcut&& gameData.map5_2Shortcut) {
            ToggleTiedObjects();
        }
    }

    // Update is called once per frame


    private void ToggleTiedObjects()
    {
        if (activeSwitch)
        {
            if (map5_2Shortcut) gameData.map5_2Shortcut = true;
            if (map5_3Shortcut) gameData.map5_3Shortcut = true;
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
                    if (map5_2Shortcut) { bridgeControlled.RunPrimAlgorythm();
                    }
                    else {
                        if (bridgeControlled.isPlatformTerrain) { bridgeControlled.removePlatform(); }
                        else { bridgeControlled.addPlatform(); }
                    }



                }
            }
            activeSwitch = false;
            SwitchAnimation();
        }       
    }

    override public void ProcessClick(CharacterStats stats)
    {

        if (isAnimating) { return; }

        ToggleTiedObjects();

    }

}
