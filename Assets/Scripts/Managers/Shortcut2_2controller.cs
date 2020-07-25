using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcut2_2controller : MonoBehaviour
{

    void Start()
    {
        if (!GameData.Instance.map2_2Shortcut)
        {
            ExitController exit=gameObject.GetComponent<ExitController>();
            if (exit) {
                exit.removeExit();
            }
            Destroy(this.gameObject);
        }

    }

}
