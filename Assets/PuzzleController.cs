using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public GameObject[] TiedEntities;
    public GameObject[] AlternateEntities;
    public bool PrimAlgorythm;
    public bool SetAlgorythm;
    private bool firstSet;
    GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        gameData= GameObject.Find("GameStateData").GetComponent<GameData>();
        if (SetAlgorythm) { Map01Setup(); }
    }

    private void Map01Setup()
    {
        if (gameData.RunNumber == 1)
        {
            firstSet = true;
        }
        else {
            int randomValue = UnityEngine.Random.Range(0, 2);
            if (randomValue == 0) firstSet = false;
            else firstSet = true;
        }

        foreach (GameObject blockage in TiedEntities){
            blockage.GetComponent<BoulderController>().SetBoulder(firstSet);
        }
        foreach (GameObject blockage in AlternateEntities) {
            blockage.GetComponent<BoulderController>().SetBoulder(!firstSet);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
