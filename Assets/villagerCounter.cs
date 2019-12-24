using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class villagerCounter : MonoBehaviour
{
    private GameData gameData;
    public GameObject textObject;
    private Text textField;
    private float delay;
    // Start is called before the first frame update
    void Start()
    {
        gameData=GameData.Instance;
        delay = 2;
        string villagersLeft = "" + (31 - gameData.RunNumber);
        textField=textObject.GetComponent<Text>();
        textField.text = villagersLeft;


    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0) {
            textField.text = ""+(30 - gameData.RunNumber);
        }
    }
}
