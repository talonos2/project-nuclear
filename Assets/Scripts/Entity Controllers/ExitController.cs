using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : DoodadData
{
    // Start is called before the first frame update


    public string mapToLoad;
    public Vector2Int exitPosition;
    public bool townMap;
    public bool dungeonEntrance;
    public SpriteMovement.DirectionMoved exitFacing;
    public AudioClip soundToPlayOnTransition;
    public bool exitIsTeleporter;
    public bool fromBuilding;
    public GameObject arrowAnimator;

    public void TransitionMap()
    {
        //Entering the dungeon's first level spawns a special cutscene, and does not count towards leaving.
        if (GameData.Instance.FloorNumber==0 && dungeonEntrance) {

            GameState.setFullPause(false);
            CutsceneLoader.LoadCutsceneAndWorldSpaceFade(.5f);
            return;
        }


        GameData gameData= GameData.Instance;
        if (gameData.FloorNumber != 0)
        {
            //Keep track of your run time.
            gameData.timesThisRun[gameData.FloorNumber - 1] = gameData.timer;

            //Was it fast?
            float timeTakenThisFloor = gameData.timer;
            if (gameData.FloorNumber > 1)
            {
                timeTakenThisFloor -= gameData.timesThisRun[gameData.FloorNumber - 2];
            }
            if (timeTakenThisFloor < 10.5f)
            {
                FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.COMPLETE_LEVEL_FAST);
            }

            //Do we trigger "No pickup" achievements?
            if (GameData.Instance.itemsFoundThisRun.Count==0)
            {
                FinalWinterAchievementManager.Instance.SetStatAndGiveAchievement(FWStatAchievement.REACH_LEVEL_4_NO_PICKUPS, gameData.FloorNumber);
                FinalWinterAchievementManager.Instance.SetStatAndGiveAchievement(FWStatAchievement.REACH_LEVEL_16_NO_PICKUPS, gameData.FloorNumber);
            }

            //Do we trigger "Low Kill" achievements?
            if (GameData.Instance.monstersKilledInThisRun <= 15)
            {
                FinalWinterAchievementManager.Instance.SetStatAndGiveAchievement(FWStatAchievement.REACH_LEVEL_16_FEW_MONSTERS, gameData.FloorNumber);
            }

            gameData.FloorNumber += 1;         
        }

        if (townMap)
        {
            gameData.FloorNumber = 0;
            if (arrowAnimator != null) { arrowAnimator.SetActive(false); }
            //Debug.Log("is in building? " + gameData.isInBuilding+" from building flag on? "+fromBuilding);
        }

        gameData.SetNextLocation(exitPosition, exitFacing);

        if (soundToPlayOnTransition != null)
        {
            SoundManager.Instance.PlayPersistentSound(soundToPlayOnTransition.name, 1f);
        }
        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        if (exitIsTeleporter) { GameData.Instance.teleportingIn = true; }
        fadeout.InitNext(mapToLoad,.2f);
        GameObject.Destroy(this);
    }

    public void removeExit() {
        MapGrid.GetComponent<DoodadGrid>().grid[DoodadLocation.x, DoodadLocation.y] = null;
    }
}
