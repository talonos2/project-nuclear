﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamWorksChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I get here right?");
        FinalWinterAchievementManager.Instance.SetupSteamWorksConnection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
