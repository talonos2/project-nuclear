using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public SpriteMovement.DirectionMoved facing = SpriteMovement.DirectionMoved.UP;
    public GameObject [] Players;
    private GameObject newPlayer;
    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {

        gameData = GameData.Instance;
        CharacterMovement characterMovement;

        if (gameData.isCutscene) {
            runCutscene();
            return;
        }

        if (gameData.nextLocationSet)
        {
            if (GameData.Instance.FloorNumber == 0) {
                int runNumber = GameData.Instance.RunNumber;
                if (CutsceneLoader.runTownBackDialogue) SetInTownPositions();

                if (runNumber==2) newPlayer = Instantiate(Players[1], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber>=3 && runNumber<=5) newPlayer = Instantiate(Players[4], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 6 && runNumber <= 10) newPlayer = Instantiate(Players[9], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 11 && runNumber <= 12) newPlayer = Instantiate(Players[11], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 13 && runNumber <= 14) newPlayer = Instantiate(Players[13], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 15 && runNumber <= 17) newPlayer = Instantiate(Players[16], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber == 18) newPlayer = Instantiate(Players[17], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 19 && runNumber <= 24) newPlayer = Instantiate(Players[23], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 25 && runNumber <= 29) newPlayer = Instantiate(Players[28], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber ==30) newPlayer = Instantiate(Players[29], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                facing = gameData.nextFacing;
                newPlayer.GetComponent<EntityData>().isMainCharacter = true;
                characterMovement = newPlayer.GetComponent<CharacterMovement>();
                characterMovement.SetRenderer();
                characterMovement.facedDirection = facing;
                characterMovement.SetLookDirection();
                if (!CutsceneLoader.postRun1Cutscene && CutsceneLoader.runTownBackDialogue) CutsceneLoader.SetNextDialogue("enterTown");
                return;
            }
            else {
                newPlayer = Instantiate(Players[gameData.RunNumber - 1], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
            }
        }
        else {
          
          newPlayer = Instantiate(Players[gameData.RunNumber - 1], new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);
        }
        facing = gameData.nextFacing;      
        newPlayer.GetComponent<EntityData>().isMainCharacter = true ;
        characterMovement =newPlayer.GetComponent<CharacterMovement>();
        characterMovement.SetRenderer();
        characterMovement.facedDirection = facing;
        characterMovement.SetLookDirection();

        if (gameData.hasted)
        {
            characterMovement.TurnHasteOn();
        }

    }

    private void SetInTownPositions()
    {

        switch (gameData.RunNumber) {
            case 2:
                gameData.nextLocaiton.x = -12;
                gameData.nextLocaiton.y = -11;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 3:
                gameData.nextLocaiton.x = 2;
                gameData.nextLocaiton.y = 25;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 4:
                gameData.nextLocaiton.x = 5;
                gameData.nextLocaiton.y = 21;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 5:
                gameData.nextLocaiton.x = 10;
                gameData.nextLocaiton.y = 19;
                gameData.nextFacing = SpriteMovement.DirectionMoved.UP;
                break;
            case 6:
                gameData.nextLocaiton.x = -5;
                gameData.nextLocaiton.y = -22;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 7:
                gameData.nextLocaiton.x = -4;
                gameData.nextLocaiton.y = -20;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 8:
                gameData.nextLocaiton.x = -7;
                gameData.nextLocaiton.y = -17;
                gameData.nextFacing = SpriteMovement.DirectionMoved.UP;
                break;
            case 9:
                gameData.nextLocaiton.x = -3;
                gameData.nextLocaiton.y = -17;
                gameData.nextFacing = SpriteMovement.DirectionMoved.UP;
                break;
            case 10:
                gameData.nextLocaiton.x = -1;
                gameData.nextLocaiton.y = -15;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 11:
                gameData.nextLocaiton.x = -7;
                gameData.nextLocaiton.y = -15;
                gameData.nextFacing = SpriteMovement.DirectionMoved.UP;
                break;
            case 12:
                gameData.nextLocaiton.x = 8;
                gameData.nextLocaiton.y = 18;
                gameData.nextFacing = SpriteMovement.DirectionMoved.RIGHT;
                break;
            case 13:
                gameData.nextLocaiton.x = -6;
                gameData.nextLocaiton.y = -12;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 14:
                gameData.nextLocaiton.x = -6;
                gameData.nextLocaiton.y = -12;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 15:
                gameData.nextLocaiton.x = -2;
                gameData.nextLocaiton.y = -15;
                gameData.nextFacing = SpriteMovement.DirectionMoved.RIGHT;
                break;
            case 16:
                gameData.nextLocaiton.x = 14;
                gameData.nextLocaiton.y = 22;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 17:
                gameData.nextLocaiton.x = 7;
                gameData.nextLocaiton.y = -9;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 18:
                gameData.nextLocaiton.x = -4;
                gameData.nextLocaiton.y = -13;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 19:
                gameData.nextLocaiton.x = -4;
                gameData.nextLocaiton.y = -13;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 20:
                gameData.nextLocaiton.x = 2;
                gameData.nextLocaiton.y = 17;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 21:
                gameData.nextLocaiton.x = -8;
                gameData.nextLocaiton.y = -19;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 22:
                gameData.nextLocaiton.x = -19;
                gameData.nextLocaiton.y = -12;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 23:
                gameData.nextLocaiton.x = 6;
                gameData.nextLocaiton.y = -13;
                gameData.nextFacing = SpriteMovement.DirectionMoved.LEFT;
                break;
            case 24:
                gameData.nextLocaiton.x = -11;
                gameData.nextLocaiton.y = -6;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 25:
                gameData.nextLocaiton.x = 8;
                gameData.nextLocaiton.y = -19;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 26:
                gameData.nextLocaiton.x = -10;
                gameData.nextLocaiton.y = -8;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 27:
                gameData.nextLocaiton.x = -10;
                gameData.nextLocaiton.y = -8;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 28:
                gameData.nextLocaiton.x = -4;
                gameData.nextLocaiton.y = -13;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 29:
                gameData.nextLocaiton.x = 9;
                gameData.nextLocaiton.y = 19;
                gameData.nextFacing = SpriteMovement.DirectionMoved.DOWN;
                break;
            case 30:
                gameData.nextLocaiton.x = 9;
                gameData.nextLocaiton.y = 19;
                gameData.nextFacing = SpriteMovement.DirectionMoved.RIGHT;
                break;

        }



    }

    private void runCutscene()
    {
        this.GetComponentInParent<CutsceneLoader>().RunCutscene();
    }
}
