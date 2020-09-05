using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentSwitchEntityData : SwitchEntityData
{
    protected GameData gameData;
    private BobPrim primToCheck;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        primToCheck = GameObject.Find("Grid").GetComponent<BobPrim>();
        gameData = GameData.Instance;
        if (gameData.map5_2Shortcut) {
            ToggleTiedObjects();
        }

    }

    public override void ToggleTiedObjects()
    {
        if (activeSwitch)
        {
            gameData.map5_2Shortcut = true;
            foreach (GameObject tiedEntity in TiedEntities)
            {
                SpikeController spikeControlled = tiedEntity.GetComponent<SpikeController>();
                BridgeController bridgeControlled = tiedEntity.GetComponent<BridgeController>();
                if (spikeControlled != null)
                {
                    spikeControlled.Open(false);
                }
                if (bridgeControlled != null)
                {
                    bridgeControlled.RunPrimAlgorythm(primToCheck);
                }
            }
            activeSwitch = false;
            SwitchTrailMover trail = GameObject.Instantiate<SwitchTrailMover>(mover);
            trail.gameObject.transform.position = new Vector3(Mathf.RoundToInt(sRender.transform.position.x * 2f) / 2f, Mathf.RoundToInt(sRender.transform.position.y * 2f) / 2f, -.001f); ;
            trail.InitStart();
            trail.path = particlePath;
            SwitchAnimation();
        }       
    }


}
