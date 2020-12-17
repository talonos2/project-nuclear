using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePub : MonoBehaviour
{
    // Start is called before the first frame update
    public DoodadData exitSpace;
    public SpriteRenderer barredDoor;
    public bool pub;
    public bool manor;
    public bool searsHut;
    void Start()
    {
        if (GameData.Instance.RunNumber >= 16 && pub) {
            barredDoor.enabled = true;
            exitSpace.isExit = false;
            exitSpace.isBlockableTerrain = true;
        }
        if (GameData.Instance.RunNumber >= 30 && manor)
        {
            barredDoor.enabled = true;
            exitSpace.isExit = false;
            exitSpace.isBlockableTerrain = true;
        }
        if (GameData.Instance.RunNumber >= 30 && searsHut)
        {
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
