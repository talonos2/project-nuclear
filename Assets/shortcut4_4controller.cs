using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shortcut4_4controller : MonoBehaviour
{
    void Start()
    {
        if (!GameData.Instance.map4_4Shortcut)
        {
            this.gameObject.GetComponent<BridgeController>().RemovePlatform();
        }
    }

}
