using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulderMover : SpriteMovement
{
    internal bool moving;
    public Vector2Int shortcutLocation;
    public bool map1_3Shortcut;
    public bool map2_3Shortcut;
    public bool map3_1Shortcut;
    private bool activateShortcut;
    public ShortcutCutsceneMap1_3to2_3 shortcutPlayer;



    private new void Start()
    {
        base.Start();
        if (map1_3Shortcut && gameData.map1_3toMap2_3Shortcut) { Destroy(this.gameObject); }
        else if (map1_3Shortcut && gameData.map3_3Shortcut) { Destroy(this.gameObject); }
        else if (map2_3Shortcut && gameData.map3_3Shortcut) { Destroy(this.gameObject); }
        else if(map3_1Shortcut && gameData.map3_4Shortcut){ Destroy(this.gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.fullPause)
        {
            return;
        }
        if (activateShortcut)
        {
            processBoulderShortcut();
            activateShortcut = false;
        }

        if (moving == false) {
            return;
        }


        if (!currentlyMoving)
        {

            SetCurrentLocation();
            switch (facedDirection)
            {
                case DirectionMoved.UP:
                    characterNextLocation.x = characterLocation.x; characterNextLocation.y = characterLocation.y + 1;
                    break;
                case DirectionMoved.DOWN:
                    characterNextLocation.x = characterLocation.x; characterNextLocation.y = characterLocation.y - 1;
                    break;
                case DirectionMoved.LEFT:
                    characterNextLocation.x = characterLocation.x - 1; characterNextLocation.y = characterLocation.y;
                    break;
                case DirectionMoved.RIGHT:
                    characterNextLocation.x = characterLocation.x + 1; characterNextLocation.y = characterLocation.y;
                    break;
            }
            if (IsMoveLocationFlyingMonsterChaseable(characterNextLocation.x, characterNextLocation.y))
            {
                //SetNextLocation(facedDirection);
                UpdateNewEntityGridLocation();
                RemoveOldEntityGridLocation();
                characterLocation = characterNextLocation;
                currentlyMoving = true;
            }
            else {
                moving = false;
                GameState.isInBattle = false;
               
            }

        }

        if (currentlyMoving == true)
        {
            float finishedMoving = MoveToNextSquare();
            if (finishedMoving == 0)
            {
                currentlyMoving = false;
                TiePositionToGrid();
                //if (characterLocation==shortcutLocation) activateShortcut = true;

                //Debug.Log("charx " + characterLocation.x + " chary " + characterLocation.y + " shortx " + shortcutLocation.x + " shorty " + shortcutLocation.y);
                if ((this.transform.position.x == shortcutLocation.x) && (this.transform.position.y == shortcutLocation.y))
                {
                    activateShortcut = true;
                    moving = false;
                    GameState.isInBattle = false;
                }
            }
        }




    }

    private void processBoulderShortcut()
    {
        
        if (map1_3Shortcut) {
            //GameData.Instance.map1_3toMap2_3Shortcut = true;
            shortcutPlayer.initialiseShortcutCutscene();
            //fadeInController.enableShortcutFadeOut(.125f);
        }
        if (map2_3Shortcut) {
            gameData.map1_3toMap2_3Shortcut = false;
            gameData.map3_3Shortcut = true;
            //there needs to be a cutscene here
        }
        if (map3_1Shortcut){
            gameData.map3_4Shortcut = true;
            //there needs to be a cutscene here
        }
        
        Destroy(this.gameObject);
        //There should be a sound effect of the boulder falling here
    }

    public override void ProcessClick(CharacterStats stats)
    {
        facedDirection = stats.GetComponentInParent<SpriteMovement>().facedDirection;
        moving = true;
        GameState.isInBattle = true;

    }
}
