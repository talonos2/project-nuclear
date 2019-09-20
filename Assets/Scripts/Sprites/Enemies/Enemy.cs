using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Stats
{

    public string enemyName = "Name me please";
    public int ExpGiven;
    public ElementalPower weakness = ElementalPower.NULL;
    public CrystalType crystalType = CrystalType.NULL;
    public int crystalDropAmount;

    public Sprite[] combatSprites;

    //public Vector2 expectedPositionOnScreen = new Vector2(-;

    public
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
