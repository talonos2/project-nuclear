using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcut4_3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject shortcut1;
    public GameObject shortcut2;
    void Start()
    {
        if (shortcut1 != null) return;
        if (!GameData.Instance.map4_3Shortcut) { this.gameObject.SetActive(false);
            gameObject.GetComponent<DoodadData>().removeDoodadFromMap();
        }
    }

    public void setupShortcut() {

        if (shortcut1 == null) {
            return;
        }
        shortcut1.SetActive(true);
        shortcut2.SetActive(true);

    }

 
}
