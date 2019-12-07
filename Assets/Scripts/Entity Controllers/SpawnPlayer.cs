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

        if (gameData.nextLocationSet)
        {
            if (GameData.Instance.FloorNumber == 0) {
                int runNumber = GameData.Instance.RunNumber;
                if (runNumber==2) newPlayer = Instantiate(Players[1], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber>=3 && runNumber<=5) newPlayer = Instantiate(Players[4], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 6 && runNumber <= 10) newPlayer = Instantiate(Players[9], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 13 && runNumber <= 14) newPlayer = Instantiate(Players[13], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 15 && runNumber <= 17) newPlayer = Instantiate(Players[16], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber == 18) newPlayer = Instantiate(Players[17], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 19 && runNumber <= 24) newPlayer = Instantiate(Players[23], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber >= 25 && runNumber <= 29) newPlayer = Instantiate(Players[28], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
                if (runNumber ==30) newPlayer = Instantiate(Players[29], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
            }
            else {
                newPlayer = Instantiate(Players[gameData.RunNumber - 1], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity);
            }
        }
        else {
          newPlayer = Instantiate(Players[gameData.RunNumber - 1], this.transform.position, Quaternion.identity);
        }
        facing = gameData.nextFacing;      
        newPlayer.GetComponent<EntityData>().isMainCharacter = true ;
        CharacterMovement characterMovement =newPlayer.GetComponent<CharacterMovement>();
        characterMovement.SetRenderer();
        characterMovement.facedDirection = facing;
        characterMovement.SetLookDirection();
    }

}
