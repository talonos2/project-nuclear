using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameData : Singleton<GameData>

{

    public float timer;
    public int seconds;
    public int minutes;
    // Start is called before the first frame update
    public int HealhCrystalBonus;
    public int ManaCrystalBonus;
    public int AttackCrystalBonus;
    public int DefenseCrystalBonus;

    public int HealhCrystalTotal = 0;
    public int ManaCrystalTotal = 0;
    public int AttackCrystalTotal = 0;
    public int DefenseCrystalTotal = 0;

    public int FloorNumber = 0;

    public int RunNumber = 1;
    public int PowersGained = 0;
    public bool timerTrigger;
    public bool hasted;
    public bool stealthed;
    public bool dashing;
    public bool jumping;
    public bool isCutscene;
    public bool isInDialogue;
    public bool pauseTimer;
    internal int furthestFloorAchieved;

    public int Perfect = 1;
    public int Worst = 0;
    public int Douglass = 1;
    public int Sara = 1;
    public int McDermit = 1;
    public int Todd = 1;
    public int Norma = 1;
    public int Derringer = 1;
    public int Melvardius = 1;
    public int Mara = 1;
    public int Devon = 1;
    public int Pendleton = 1;




    public List<Weapon> townWeapons = new List<Weapon>();
    public List<Armor> townArmor = new List<Armor>();
    public List<Accessory> townAccessories = new List<Accessory>();

    public float[] bestTimes = new float[20];
    public float[] timesThisRun = new float[20];

    public string killer = "time";

    //    public bool Paused = false;

    //public bool Shortcut1 = false;

    //public bool RunSetupFinished = false;


    [HideInInspector]
    public Vector2Int nextLocaiton;
    [HideInInspector]
    public SpriteMovement.DirectionMoved nextFacing = SpriteMovement.DirectionMoved.LEFT;
    [HideInInspector]
    public bool nextLocationSet;

    public bool map1_3toMap2_3Shortcut;
    public bool map1_3Shortcut;
    public bool map2_2Shortcut;
    public bool map2_4Shortcut;
    public bool map3_2Shortcut;
    public bool map3_3Shortcut;
    public bool map3_4Shortcut;
    public bool map4_3Shortcut;
    public bool map4_4Shortcut;
    public bool map5_1Shortcut;
    public bool map5_2Shortcut;
    public bool map5_3Shortcut;
    public bool map5_4Shortcut;

    public bool iceBoss1;
    public bool earthBoss1;
    public bool fireBoss1;
    public bool airBoss1;
    public bool deathBoss1;
    public bool earthBoss2;
    public bool fireBoss2;
    public bool airBoss2;


    internal float deathTime;
    internal bool hiroDeathMonster;
    internal bool postRun1Cutscene;

    internal static Weapon bestWeaponFound;
    internal static Armor bestArmorFound;
    internal static Accessory bestAccessoryFound;

    internal bool statsSetup=false;

    internal bool victory;
    

    internal bool IsInTown()
    {
        return FloorNumber == 0;
    }

    internal void autoSaveStats()
    {
        LoadSaveController autoSaveControl = new LoadSaveController();
        autoSaveControl.autoSave();
        
        //.autoSave();
    }

    public void SetNextLocation(Vector2Int location, SpriteMovement.DirectionMoved facing)
    {
        nextLocaiton = location;
        nextFacing = facing;
        nextLocationSet = true;
    }

    public void ResetTimer()
    {
        timer = 0;
        deathTime = 0;
        killer = "time";
        for (int x = 0; x < 20; x++)
        {
            GameData.Instance.timesThisRun[x] = 0;
        }
        pauseTimer = false;
    }

    public bool addHealToTimer()
    {
        if (timer >= 570)
            return false;
        else
        {
            timer += 30;
            return true;
        }

    }

    void Update()
    {

        if (bestTimes.Length != 20) {
            GameState.fullPause = true;
            Debug.Log("Broken bestTimes thing");
        }

        if (GameState.fullPause || FloorNumber == 0 || pauseTimer || isInDialogue) { return; }

        int tempSeconds = (int)(timer + Time.deltaTime);
        if (tempSeconds > (int)timer)
        {
            timerTrigger = true;
        }
        else timerTrigger = false;
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);
        minutes = (int)(timer / 60);
        if (minutes == 10)
        {
            //Needs to really call the 'kill player' animation and then load deathscene from that script. That script should 'pause' the timer.
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>().deactivatePowers();
            GameState.isInBattle = false;
            SoundManager.Instance.PlayPersistentSound("TakenByCurse", 1f);
            MusicManager.instance.FadeOutMusic(-2, 3);
            SceneManager.LoadScene("DeathScene");
            pauseTimer = true;
        }
    }

    internal void DebugKillPlayer()
    {
        deathTime = timer;
        timer = 600;
    }


}
