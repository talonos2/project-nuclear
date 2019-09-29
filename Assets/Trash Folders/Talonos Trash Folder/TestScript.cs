using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayScript();
    }

    private async void PlayScript()
    {
        await RuntimeInitializer.InitializeAsync();
        await Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync("02Sara");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
