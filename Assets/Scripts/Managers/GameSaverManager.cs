using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaverManager
{
    
        public int HealhCrystalBonus;
        public int ManaCrystalBonus;
        public int AttackCrystalBonus;
        public int DefenseCrystalBonus;

        public int HealhCrystalTotal = 0;
        public int ManaCrystalTotal = 0;
        public int AttackCrystalTotal = 0;
        public int DefenseCrystalTotal = 0;

        public int FloorNumber = 0;

        public int RunNumber = 0;
        public int PowersGained = 0;
        internal int furthestFloorAchieved;
        //internal float[] bestTimes=new float [20];
    internal float[] bestTimes = new float[] {Mathf.Infinity, Mathf.Infinity, Mathf.Infinity, Mathf.Infinity, Mathf.Infinity,
        Mathf.Infinity,Mathf.Infinity,Mathf.Infinity,Mathf.Infinity,Mathf.Infinity,
        Mathf.Infinity,Mathf.Infinity,Mathf.Infinity,Mathf.Infinity,Mathf.Infinity,
        Mathf.Infinity,Mathf.Infinity,Mathf.Infinity,Mathf.Infinity,Mathf.Infinity };
    public bool[] gabNumbers = new bool[32];
    public bool iceBoss1;
    public bool earthBoss1;
    public bool fireBoss1;
    public bool airBoss1;
    private bool earthBoss2;
    private bool fireBoss2;
    private bool airBoss2;
    private bool deathBoss;
    private bool deathBoss2; 
    
    public List<String> townWeapons = new List<String>();
        public List<String> townArmor = new List<String>();
        public List<String> townAccessories = new List<String>();

    public bool map1_3Shortcut;
    public bool map2_2Shortcut;
    public bool map2_4Shortcut;
    public bool map3_2Shortcut;
    public bool map3_3Shortcut;
    public bool map3_4Shortcut;
    public bool map4_1Shortcut;
    public bool map4_3Shortcut;
    public bool map4_4Shortcut;
    public bool map5_1Shortcut;
    public bool map5_2Shortcut;
    public bool map5_3Shortcut;
    public bool map5_4Shortcut;
    public bool map1_3toMap2_3Shortcut;

    public int Douglass;
    public int Sara;
    public int McDermit;
    public int Todd;
    public int Norma;
    public int Derringer;
    public int Melvardius;
    public int Mara;
    public int Devon;
    public int Pendleton;
    internal KeymapType sneakyKeyMap = KeymapType.UNDEFINED;


    public void SetupSave()
        {


        gabNumbers = GameData.Instance.gabNumbers;


            HealhCrystalBonus = GameData.Instance.HealhCrystalBonus;
            ManaCrystalBonus = GameData.Instance.ManaCrystalBonus;
            AttackCrystalBonus = GameData.Instance.AttackCrystalBonus;
            DefenseCrystalBonus = GameData.Instance.DefenseCrystalBonus;
            HealhCrystalTotal = GameData.Instance.HealhCrystalTotal;
            ManaCrystalTotal = GameData.Instance.ManaCrystalTotal;
            AttackCrystalTotal = GameData.Instance.AttackCrystalTotal;
            DefenseCrystalTotal = GameData.Instance.DefenseCrystalTotal;
            FloorNumber = 0;
            RunNumber = GameData.Instance.RunNumber;
            PowersGained = GameData.Instance.PowersGained;
            furthestFloorAchieved = GameData.Instance.furthestFloorAchieved;    
        bestTimes = GameData.Instance.bestTimes;

        Douglass =GameData.Instance.Douglass;
      Sara=GameData.Instance.Sara;
      McDermit=GameData.Instance.McDermit;
      Todd=GameData.Instance.Todd;
      Norma=GameData.Instance.Norma;
      Derringer=GameData.Instance.Derringer;
      Melvardius=GameData.Instance.Melvardius;
      Mara=GameData.Instance.Mara;
      Devon=GameData.Instance.Devon;
      Pendleton=GameData.Instance.Pendleton;

    iceBoss1=GameData.Instance.iceBoss1;
        earthBoss1 = GameData.Instance.earthBoss1;
        fireBoss1 = GameData.Instance.fireBoss1;
        airBoss1 = GameData.Instance.airBoss1;
        earthBoss2 = GameData.Instance.earthBoss2;
        fireBoss2 = GameData.Instance.fireBoss2;
        airBoss2 = GameData.Instance.airBoss2;
        deathBoss = GameData.Instance.deathBoss1;
        deathBoss2 = GameData.Instance.deathBoss2;

        map1_3toMap2_3Shortcut = GameData.Instance.map1_3toMap2_3Shortcut;
        map1_3Shortcut = GameData.Instance.map1_3Shortcut;
        map2_2Shortcut = GameData.Instance.map2_2Shortcut;
        map2_4Shortcut = GameData.Instance.map2_4Shortcut;
        map3_2Shortcut = GameData.Instance.map3_2Shortcut;
        map3_3Shortcut = GameData.Instance.map3_3Shortcut;
        map3_4Shortcut = GameData.Instance.map3_4Shortcut;
        map4_1Shortcut = GameData.Instance.map4_1Shortcut;
        map4_3Shortcut = GameData.Instance.map4_3Shortcut;
        map4_4Shortcut = GameData.Instance.map4_4Shortcut;
        map5_1Shortcut = GameData.Instance.map5_1Shortcut;
        map5_2Shortcut = GameData.Instance.map5_2Shortcut;
        map5_3Shortcut = GameData.Instance.map5_3Shortcut;
        map5_4Shortcut = GameData.Instance.map5_4Shortcut;

        sneakyKeyMap = GameData.Instance.sneakyKeyMap;

        foreach (Weapon wpnItem in GameData.Instance.townWeapons) {
            townWeapons.Add(wpnItem.name);
        }
        foreach (Armor armItem in GameData.Instance.townArmor)
        {
            townArmor.Add(armItem.name);
        }
        foreach (Accessory accItem in GameData.Instance.townAccessories)
        {
            townAccessories.Add(accItem.name);
        }

       
    }


    public void PushSaveToGameData()
        {

        GameData.Instance.gabNumbers =gabNumbers;

        GameData.Instance.HealhCrystalBonus = HealhCrystalBonus;
            GameData.Instance.ManaCrystalBonus = ManaCrystalBonus;
            GameData.Instance.AttackCrystalBonus = AttackCrystalBonus;
            GameData.Instance.DefenseCrystalBonus = DefenseCrystalBonus;
            GameData.Instance.HealhCrystalTotal = HealhCrystalTotal;
            GameData.Instance.ManaCrystalTotal = ManaCrystalTotal;
            GameData.Instance.AttackCrystalTotal = AttackCrystalTotal;
            GameData.Instance.DefenseCrystalTotal = DefenseCrystalTotal;
            GameData.Instance.FloorNumber = 0;
            GameData.Instance.RunNumber = RunNumber;
            GameData.Instance.PowersGained = PowersGained;
            GameData.Instance.furthestFloorAchieved = furthestFloorAchieved;
            GameData.Instance.bestTimes = bestTimes;

        GameData.Instance.Douglass=Douglass ;
        GameData.Instance.Sara=Sara;
         GameData.Instance.McDermit= McDermit;
         GameData.Instance.Todd=Todd;
        GameData.Instance.Norma= Norma ;
        GameData.Instance.Derringer=Derringer  ;
         GameData.Instance.Melvardius=Melvardius;
        GameData.Instance.Mara= Mara ;
        GameData.Instance.Devon=Devon  ;
        GameData.Instance.Pendleton=Pendleton  ;

        GameData.Instance.iceBoss1= iceBoss1 ;
         GameData.Instance.earthBoss1= earthBoss1 ;
        GameData.Instance.fireBoss1= fireBoss1 ;
         GameData.Instance.airBoss1= airBoss1 ;
        GameData.Instance.earthBoss2 = earthBoss2;
        GameData.Instance.fireBoss2 = fireBoss2;
        GameData.Instance.airBoss2 = airBoss2;
        GameData.Instance.deathBoss1 = deathBoss;
        GameData.Instance.deathBoss2 = deathBoss2;

        GameData.Instance.map1_3Shortcut = map1_3Shortcut;
        GameData.Instance.map2_2Shortcut = map2_2Shortcut;
        GameData.Instance.map2_4Shortcut = map2_4Shortcut;
        GameData.Instance.map3_2Shortcut= map3_2Shortcut;
        GameData.Instance.map3_3Shortcut=map3_3Shortcut;
        GameData.Instance.map3_4Shortcut = map3_4Shortcut;
        GameData.Instance.map4_1Shortcut = map4_1Shortcut;
        GameData.Instance.map4_3Shortcut = map4_3Shortcut;
        GameData.Instance.map4_4Shortcut = map4_4Shortcut;
        GameData.Instance.map5_1Shortcut = map5_1Shortcut;
        GameData.Instance.map5_2Shortcut = map5_2Shortcut;
            GameData.Instance.map5_3Shortcut = map5_3Shortcut;
        GameData.Instance.map5_4Shortcut = map5_4Shortcut;
        GameData.Instance.map1_3toMap2_3Shortcut = map1_3toMap2_3Shortcut;

        GameData.Instance.sneakyKeyMap = sneakyKeyMap;

           GameObject equipmentData = GameObject.Find("EquipmentData");

        GameData.Instance.townWeapons.Clear();
        GameData.Instance.townArmor.Clear();
        GameData.Instance.townAccessories.Clear();


        foreach (String itemName in townWeapons) {
            foreach (Transform item in equipmentData.transform)
            {
                if (itemName == item.name) {
                    GameData.Instance.townWeapons.Add(item.GetComponent<Weapon>());
                    break;
                }
            }            
        }

        foreach (String itemName in townArmor)
        {
            foreach (Transform item in equipmentData.transform)
            {
                if (itemName == item.name)
                {
                    GameData.Instance.townArmor.Add(item.GetComponent<Armor>());
                    break;
                }
            }
        }

        foreach (String itemName in townAccessories)
        {
            foreach (Transform item in equipmentData.transform)
            {
                if (itemName == item.name)
                {
                    GameData.Instance.townAccessories.Add(item.GetComponent<Accessory>());
                    break;
                }
            }
        }



    }


    
}
