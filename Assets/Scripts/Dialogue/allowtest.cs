using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class Allowtest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync("03Cut");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
