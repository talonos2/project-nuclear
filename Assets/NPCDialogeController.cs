using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogeController : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] DailyDialogues;
    void Start()
    {
        RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(DailyDialogues[GameData.Instance.RunNumber]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
