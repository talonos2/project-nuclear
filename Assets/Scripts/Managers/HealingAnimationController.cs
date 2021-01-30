using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAnimationController : MonoBehaviour
{


    public GameObject healthHealer;
    public GameObject manaHealer;
    public GameObject bothHealer;

    public ParticleSystem healingParticles;
    public SpriteRenderer healingPenalty;
    private float delayToShowPenalty = 0;

    internal void PlayHealingAnimation(HealingType healingType)
    {
        //0 = health
        //1 = mana
        //2 = both

        /*if(healingType==0) Instantiate(healthHealer, this.transform.GetChild(0).GetChild(0).position + new Vector3(0, 0, -.01f), Quaternion.identity, this.transform.GetChild(0).GetChild(0));
        if (healingType==1) Instantiate(manaHealer, this.transform.GetChild(0).GetChild(0).position + new Vector3(0, 0, -.01f), Quaternion.identity, this.transform.GetChild(0).GetChild(0));
        if (healingType==2) Instantiate(bothHealer, this.transform.GetChild(0).GetChild(0).position + new Vector3(0, 0, -.01f), Quaternion.identity, this.transform.GetChild(0).GetChild(0));*/

        ParticleSystem.MainModule main = healingParticles.main;

        switch (healingType)
        {
            case HealingType.HEALTH:
                main.startColor = Color.green;
                break;
            case HealingType.MANA:
                main.startColor = Color.cyan;
                break;
            case HealingType.BOTH:
                main.startColor = new Color(0,1,.5f);
                break;
        }

        delayToShowPenalty = 1f;
        if (healingType==HealingType.BOTH) {
            healingPenalty.enabled = true;
        }
        

        healingParticles.Stop();
        healingParticles.Play();
    }

    private void Update()
    {
        if (GameState.getFullPauseStatus()) return;
        if (delayToShowPenalty > 0) {
            delayToShowPenalty -= Time.deltaTime;
            if (delayToShowPenalty < 0) {
                healingPenalty.enabled = false;
            }
        }

    }

}

internal enum HealingType { HEALTH, MANA, BOTH }
