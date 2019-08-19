using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // Start is called before the first frame update
    public int HealhCrystalBonus;
    public int ManaCrystalBonus;
    public int AttackCrystalBonus;
    public int DefenseCrystalBonus;
    public int FloorNumber = 1;
    public int RunNumber = 1;
    public int PowersGained = 0;

    public List<GameObject> townWeapons=new List<GameObject>();
    public List<GameObject> townArmor = new List<GameObject>();
    public List<GameObject> townAccessories = new List<GameObject>();

    public bool Shortcut1 = false;

    public bool RunSetupFinished = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
