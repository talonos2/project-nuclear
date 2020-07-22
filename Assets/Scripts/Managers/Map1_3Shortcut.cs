using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1_3Shortcut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameData.Instance.map1_3Shortcut) {
            Destroy(this.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        if (GameData.Instance == null) return;
        if (GameData.Instance.earthBoss2 && !GameData.Instance.map1_3Shortcut) {
            GameData.Instance.map1_3Shortcut = true;
            //Play cutscene
        }
    }
}
