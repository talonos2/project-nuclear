using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_ChaseEverywhere : MonsterMovement
{
    // Update is called once per frame
    private GameObject spawningCrystal;
    new void Start() {
        base.Start();
        waitTimer = SpotWaitTimer;
    }
    void Update()
    {
        if (GameState.isInBattle || GameState.fullPause)
        {
            return;
        }

        if (waitTimer >= 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }



        if (!currentlyMoving)
        {
            SetCurrentLocation();
        


        if (!gameData.stealthed)
            {
                NextStep = GetChaseStep();
                SetNextLocation(NextStep);//
                facedDirection = NextStep;
                CheckForFight(characterNextLocation.x, characterNextLocation.y);//

                if (!IsLocationEntityPassible(characterNextLocation.x, characterNextLocation.y) && stuck >= 15)
                {
                    NextStep = GetRandomStep();
                    SetNextLocation(NextStep);
                    facedDirection = NextStep;
                }

                if (IsLocationEntityPassible(characterNextLocation.x, characterNextLocation.y))
                {
                    UpdateNewEntityGridLocation();
                    RemoveOldEntityGridLocation();
                    characterLocation = characterNextLocation;
                    currentlyMoving = true;
                    stuck = 0;
                }
                else
                {
                    stuck += 1;
                }
            }
            else { ChangeMonsterFacing(); }
                
                //Debug.Log("FightCheckLocation " + CharacterNextLocation.x + " " + CharacterNextLocation.y);
                //CheckForFight(CharacterNextLocation.x, CharacterNextLocation.y);            
        }

        if (currentlyMoving == true)
        {
            float finishedMoving = MoveToNextSquare();
            if (finishedMoving == 0)
            {
                currentlyMoving = false;
                TiePositionToGrid();
            }
        }
    }

    void OnDestroy()
    {
        if (spawningCrystal == null)
            return;
        spawningCrystal.GetComponent<PurpleCrystalInteraction>().StartSpawningProcess();
        //spawningCrystal.StartSpawningProcess();

    }

    public void SetSpawningCrystal(GameObject spawnCrystal) {
        spawningCrystal = spawnCrystal;

    }

}
