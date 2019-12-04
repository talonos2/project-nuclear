using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>

{

    private float timer;
    public int seconds;
    public int minutes;
    // Start is called before the first frame update
    public int HealhCrystalBonus;
    public int ManaCrystalBonus;
    public int AttackCrystalBonus;
    public int DefenseCrystalBonus;
    public int FloorNumber = 0;
    public int RunNumber = 1;
    public int PowersGained = 0;
    public bool timerTrigger;
    public bool hasted;
    public bool stealthed;
    public bool dashing;
    



    public List<GameObject> townWeapons = new List<GameObject>();
    public List<GameObject> townArmor = new List<GameObject>();
    public List<GameObject> townAccessories = new List<GameObject>();

    public bool Paused = false;

    //public bool Shortcut1 = false;

    public bool RunSetupFinished = false;


      [HideInInspector]
    public Vector2Int nextLocaiton;
      [HideInInspector]
    public SpriteMovement.DirectionMoved nextFacing = SpriteMovement.DirectionMoved.LEFT;
      [HideInInspector]
    public bool nextLocationSet;

    public bool map5_2Shortcut;
    public bool map5_3Shortcut;

    public void SetNextLocation(Vector2Int location, SpriteMovement.DirectionMoved facing) {
        nextLocaiton = location;
        nextFacing = facing;
        nextLocationSet = true;
    }

    void Update()
    {

        int tempSeconds = (int)(timer + Time.deltaTime);
        if (tempSeconds > (int)timer)
        {
            timerTrigger = true;
        }
        else timerTrigger = false;
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);
        minutes = (int)(timer / 60);
        if (minutes == 10) { Debug.Log("Run Over. You are dead."); }
    }
}
