using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName = "Name me please";
    public int HP = 0;
    public int attack = 0;
    public int defense = 0;
    public ElementalPower weakness = ElementalPower.NULL;
    public CrystalType crystalType = CrystalType.NULL;
    public int crystalDropAmount;
    public int ExpGiven;

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
