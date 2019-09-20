using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public SpriteMovement.DirectionMoved facing = SpriteMovement.DirectionMoved.UP;
    public GameObject [] Players;
    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {

        gameData = GameObject.Find("GameStateData").GetComponent<GameData>();
        GameObject newPlayer=Instantiate(Players[gameData.RunNumber-1], this.transform.position, Quaternion.identity);
        newPlayer.GetComponent<EntityData>().isMainCharacter = true ;
        newPlayer.GetComponent<SpriteMovement>().FacedDirection=facing;
        newPlayer.GetComponent<SpriteMovement>().SetLookDirection();
    }

}
