using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gainPowerDialogue : MonoBehaviour
{
    public string gainedPowerScript;

    public async void playPowerGainedDialogueAsync() {

        GameData.Instance.isInDialogue = true;
        await RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(gainedPowerScript);


    }


    
}
