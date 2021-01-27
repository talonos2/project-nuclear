using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateAudioListener : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioListener listenActivate;
    private bool setOnlyOnce;
    void Start()
    {
        GameData.Instance.inDungeon = false;
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.startSceneLoaded &&!setOnlyOnce) {
            listenActivate.enabled = true;
            setOnlyOnce = true;
        }
    }
}
