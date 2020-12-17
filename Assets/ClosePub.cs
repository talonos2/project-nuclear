using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePub : MonoBehaviour
{
    // Start is called before the first frame update
    public DoodadData exitSpace;
    public SpriteRenderer barredDoor;
    void Start()
    {
        if (GameData.Instance.RunNumber >= 15) {
            barredDoor.enabled = true;
            exitSpace.isExit = false;
            exitSpace.isBlockableTerrain = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
