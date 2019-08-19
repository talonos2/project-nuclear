using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    // Start is called before the first frame update


    internal static void initiateFight(GameObject Player, GameObject Monster) {
        //Player.GetComponent<CharacterStats>.;
        Enemy MonsterStats = Monster.GetComponent<Enemy>();
        CharacterStats PlayerStats = Player.GetComponent<CharacterStats>();
        
        //Player attacks:

        //Animate Attack
        //Check for timed bonus and elements.
        bool inCombat = true;
        while (inCombat)
        {

            //Debug.Log("Mhp " + MonsterStats.HP + " PlyAtk " + PlayerStats.Attack);
            //Debug.Log("Php " + PlayerStats.HP + " MonAtk " + MonsterStats.Attack);
            MonsterStats.HP -= (int)PlayerStats.attack + PlayerStats.weaponBonusAttack - MonsterStats.defense<0?0: (int)PlayerStats.attack + PlayerStats.weaponBonusAttack - MonsterStats.defense;
            if (MonsterStats.HP <= 0)
            {
                Debug.Log("You win!");
                switch (MonsterStats.crystalType)
                {
                    case CrystalType.All:
                        PlayerStats.AttackCrystalsGained += MonsterStats.crystalDropAmount;
                        PlayerStats.defenseCrystalsGained += MonsterStats.crystalDropAmount;
                        PlayerStats.HealthCrystalsGained += MonsterStats.crystalDropAmount;
                        PlayerStats.ManaCrystalsGained += MonsterStats.crystalDropAmount;
                        break;
                    case CrystalType.ATTACK:
                        PlayerStats.AttackCrystalsGained += MonsterStats.crystalDropAmount;
                        break;
                    case CrystalType.DEFENSE:
                        PlayerStats.defenseCrystalsGained += MonsterStats.crystalDropAmount;
                        break;
                    case CrystalType.HEALTH:
                        PlayerStats.HealthCrystalsGained += MonsterStats.crystalDropAmount;
                        break;
                    case CrystalType.MANA:
                        PlayerStats.ManaCrystalsGained += MonsterStats.crystalDropAmount;
                        break;
                    default:
                        break;
                }
                PlayerStats.AddExp(MonsterStats.ExpGiven);
                PlayerStats.PushCharacterData();
                Destroy(Monster);
                inCombat = false;
            }
            else {
                PlayerStats.HP -= MonsterStats.attack - (int)PlayerStats.defense- PlayerStats.armorBonusDefense < 0 ? 0 : MonsterStats.attack - (int)PlayerStats.defense- PlayerStats.armorBonusDefense;
                if (PlayerStats.HP <= 0) {
                    //Animate Attack
                    //Check for timed bonus and elements.
                    Debug.Log("You lose");
                    inCombat = false;
                }
            }

        }




        //(CharacterStats)hmm.

    }


}
