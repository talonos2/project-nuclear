using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystalSwitch : SwitchEntityData
{
    public bool lightning;
    public bool fire;
    public bool earth;
    public bool water;
    public Material cleanSwitchMaterial;
    private bool crystalActivated;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        sRender = this.GetComponentInChildren<MeshRenderer>();
        sRender.material = new Material(this.sRender.material);

    }

   

    override public void ProcessClick(CharacterStats stats)
    {
        if (crystalActivated == true)return;
        if (lightning)
        {
            stats.powersGained = 3;
            if (stats.currentPower == 4) { stats.currentPower = 3; }
        }
        if (fire)
        {
            stats.powersGained = 2;
            if (stats.currentPower == 3) { stats.currentPower = 2; }
            stats.gameObject.GetComponent<CharacterMovement>().TurnHasteOff();
        }
        if (earth)
        {
            stats.powersGained = 1;
            if (stats.currentPower == 2) { stats.currentPower = 1; }
            stats.gameObject.GetComponent<CharacterMovement>().TurnStealthOff();

        }
        if (water)
        {
            stats.powersGained = 0;
            if (stats.currentPower == 1) { stats.currentPower = 0; }
        }        

        SoundManager.Instance.PlaySound("losingPower", 1f);
        crystalActivated = true;
        ToggleTiedObjects();
        brightenCrystal();

    }

    public override void ToggleTiedObjects()
    {
        //bool movedABridge = false;
        if (activeSwitch)
        {
            for (int x = 0; x < TiedEntities.Length; x++)
            {
                GameObject tiedEntity = TiedEntities[x];
                BridgeController bridgeControlled = tiedEntity.GetComponent<BridgeController>();
                WindJumpController windJumpControlled = tiedEntity.GetComponent<WindJumpController>();
                if (bridgeControlled != null)
                {
                    bridgeControlled.SwapPlatform();
                }
                if (windJumpControlled != null)
                {
                    windJumpControlled.EnableWindJumper();

                }


            }
            activeSwitch = false;

        }

        //if (movedABridge)
      //  {
      //      SoundManager.Instance.PlaySound("Environment/Bridge", 1);
      //  }

    }

        private void brightenCrystal()
    {
        sRender.material = cleanSwitchMaterial;
    }
}
