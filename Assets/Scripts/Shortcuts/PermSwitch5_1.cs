using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermSwitch5_1 : SwitchEntityData
{
    private bool alreadyActivated;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (GameData.Instance.map5_1Shortcut)
        {
            alreadyActivated = true;
            ToggleTiedObjects();
        }
    }

    public override void ToggleTiedObjects()
    {

        if (activeSwitch)
        {
            if (!alreadyActivated) SoundManager.Instance.PlaySound("Bridge", 1);
            GameData.Instance.map5_1Shortcut = true;
            alreadyActivated = true;
            foreach (GameObject tiedEntity in TiedEntities)
            {
                SpikeController spikeControlled = tiedEntity.GetComponent<SpikeController>();
                if (spikeControlled != null)
                {
                    spikeControlled.isPassable = true;
                    spikeControlled.LowerSpikeAnimation();
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
