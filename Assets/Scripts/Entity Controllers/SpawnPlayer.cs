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

        gameData = GameObject.Find("GameStateData").GetComponent<GameData>();

        if (gameData.nextLocationSet)
        { newPlayer = Instantiate(Players[gameData.RunNumber - 1], new Vector3(gameData.nextLocaiton.x, gameData.nextLocaiton.y, 0), Quaternion.identity); }
        else {
          newPlayer = Instantiate(Players[gameData.RunNumber - 1], this.transform.position, Quaternion.identity);
        }
        facing = gameData.nextFacing;      
        newPlayer.GetComponent<EntityData>().isMainCharacter = true ;
        CharacterMovement characterMovement =newPlayer.GetComponent<CharacterMovement>();
        characterMovement.SetRenderer();
        characterMovement.FacedDirection = facing;
        characterMovement.SetLookDirection();
    }

}
