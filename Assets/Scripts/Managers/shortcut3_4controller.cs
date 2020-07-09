using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shortcut3_4controller : MonoBehaviour
{
    public BridgeController bridgeToControl;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameData.Instance.map3_4Shortcut) {
            bridgeToControl.RemovePlatform();
        }   
    }


}
