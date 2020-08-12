using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAnimationController : MonoBehaviour
{


    public GameObject healthHealer;
    public GameObject manaHealer;
    public GameObject bothHealer;

    internal void PlayHealingAnimation(int healingType)
    {
        //0 = health
        //1 = mana
        //2 = both

       if(healingType==0) Instantiate(healthHealer, this.transform.GetChild(0).GetChild(0).position + new Vector3(0, 0, -.01f), Quaternion.identity, this.transform.GetChild(0).GetChild(0));
       if (healingType==1) Instantiate(manaHealer, this.transform.GetChild(0).GetChild(0).position + new Vector3(0, 0, -.01f), Quaternion.identity, this.transform.GetChild(0).GetChild(0));
       if (healingType==2) Instantiate(bothHealer, this.transform.GetChild(0).GetChild(0).position + new Vector3(0, 0, -.01f), Quaternion.identity, this.transform.GetChild(0).GetChild(0));

    }
}
