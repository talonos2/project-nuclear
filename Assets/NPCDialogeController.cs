using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogeController : MonoBehaviour
{
    // Start is called before the first frame update
    public string DailyDialogue;
    void Start()
    {
        RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(DailyDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
