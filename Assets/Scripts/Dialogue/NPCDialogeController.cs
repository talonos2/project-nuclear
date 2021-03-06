﻿using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogeController : MonoBehaviour
{
    // Start is called before the first frame update
    public string DailyDialogue;
    void Start()
    {
        if (GameData.Instance.isCutscene|| GameData.Instance.RunNumber>=31) {
            
            return;
        }
        InitAndRunDiag();

/*#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(DailyDialogue);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
*/
    }

    public  async void InitAndRunDiag()
    {
        await RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(DailyDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
