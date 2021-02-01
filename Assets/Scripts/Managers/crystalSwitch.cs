using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystalSwitch : SwitchEntityData
{
    public ElementalPower type;
    public Material cleanSwitchMaterial;
    private bool crystalActivated;
    public GameObject [] bubbleSpwanersToClose;
    public LosePowerInBossRoomEffect effect;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        sRender = this.GetComponentInChildren<MeshRenderer>();
        sRender.material = new Material(this.sRender.material);

    }



    override public void ProcessClick(CharacterStats stats)
    {
        if (crystalActivated)
        {
            return;
        }
        switch (type)
        {
            case ElementalPower.AIR:
                stats.powersGained = 3;
                if (stats.currentPower == 4) { stats.currentPower = 3; }
                break;
            case ElementalPower.FIRE:
                stats.powersGained = 2;
                if (stats.currentPower == 3) { stats.currentPower = 2; }
                stats.gameObject.GetComponent<CharacterMovement>().TurnHasteOff();
                break;
            case ElementalPower.EARTH:
                stats.powersGained = 1;
                if (stats.currentPower == 2) { stats.currentPower = 1; }
                stats.gameObject.GetComponent<CharacterMovement>().TurnStealthOff();
                break;
            case ElementalPower.ICE:
                stats.powersGained = 0;
                if (stats.currentPower == 1) { stats.currentPower = 0; }
                foreach (GameObject i in bubbleSpwanersToClose) { i.SetActive(false); }
                break;
        }

        SoundManager.Instance.PlaySound("crystalAbsorbingPower", 1f);
        CrystalSpawner.SpawnLosePowerParticles(100, .01f, stats.gameObject, effect, this.gameObject, type, ()=>
        {
            crystalActivated = true;
            ToggleTiedObjects();
            brightenCrystal();
        });

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
