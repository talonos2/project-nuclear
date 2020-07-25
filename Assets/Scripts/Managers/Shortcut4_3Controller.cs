using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcut4_3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!GameData.Instance.map4_3Shortcut) { this.gameObject.SetActive(false);
            gameObject.GetComponent<DoodadData>().removeDoodadFromMap();
        }
    }

 
}
