using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



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

    public int HealhCrystalTotal=0;
    public int ManaCrystalTotal=0;
    public int AttackCrystalTotal=0;
    public int DefenseCrystalTotal=0;

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
    internal List <float> bestRunTimes;
    public List<Weapon> townWeapons = new List<Weapon>();
    public List<Armor> townArmor = new List<Armor>();
    public List<Accessory> townAccessories = new List<Accessory>();

//    public bool Paused = false;

    //public bool Shortcut1 = false;

    //public bool RunSetupFinished = false;


      [HideInInspector]
    public Vector2Int nextLocaiton;
      [HideInInspector]
    public SpriteMovement.DirectionMoved nextFacing = SpriteMovement.DirectionMoved.LEFT;
      [HideInInspector]
    public bool nextLocationSet;

    public bool Map3_2Shortcut;
    public bool map5_2Shortcut;
    public bool map5_3Shortcut;

    public bool iceBoss1;
    public bool earthBoss1;
    public bool fireBoss1;
    public bool airBoss1;
    public bool deathBoss;

    internal bool hiroDeathMonster;
    internal bool postRun1Cutscene;


    internal bool IsInTown()
    {
        return FloorNumber == 0;
    }

    internal void autoSaveStats()
    {
        LoadSaveController.autoSave(RunNumber, furthestFloorAchieved);
    }

    public void SetNextLocation(Vector2Int location, SpriteMovement.DirectionMoved facing) {
        nextLocaiton = location;
        nextFacing = facing;
        nextLocationSet = true;
    }

    public void resetTimer() {
        timer = 0;
        pauseTimer = false;
    }

    public bool addHealToTimer() {
        if (timer >= 570)
            return false;
        else {
            timer += 30;
            return true;
        }

    }

   

    void Update()
    {
        if (GameState.fullPause || FloorNumber==0 || pauseTimer ) { return; }

        int tempSeconds = (int)(timer + Time.deltaTime);
        if (tempSeconds > (int)timer)
        {
            timerTrigger = true;
        }
        else timerTrigger = false;
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);
        minutes = (int)(timer / 60);
        if (minutes == 10) {
            //Needs to really call the 'kill player' animation and then load deathscene from that script. That script should 'pause' the timer. 
            GameState.isInBattle = false;
            SceneManager.LoadScene("DeathScene");
            pauseTimer = true;
        }
    }

    internal void killPlayer()
    {
        timer = 600;
    }




}
/*

    */
