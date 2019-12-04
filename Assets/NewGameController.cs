using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameController : MonoBehaviour
{

    public GameObject RunNumberTextField;
    public Vector2Int Map1EntrancePoint;
    public SpriteMovement.DirectionMoved Map1Facing;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartNewGame (){
        Text runVariable=RunNumberTextField.GetComponent <Text> ();
        Debug.Log("hmm " + runVariable.text);
        if (runVariable.text == "")
        {
            GameData.Instance.SetNextLocation(Map1EntrancePoint, Map1Facing);
            SceneManager.LoadScene("Map1-1");
        }
        else {
            GameData.Instance.RunNumber = Convert.ToInt32(runVariable.text);
            SceneManager.LoadScene("TownMap_1");
        }
            
    }


}
