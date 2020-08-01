﻿using Naninovel;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathSceneControl : MonoBehaviour
{
    private GameData gameData;
    public GameObject textObject;
    private Text textField;
    private float delay;
    public GameObject deathDiaglogPanel;
    public GameObject button;
    private bool waitAFrame=true;

    public TextMeshProUGUI textMeshToPrint;
    public string deathDialogueScript;
    public string townGrowingSmallerText;

    public async void playDeathDialogueAsync()
    {

        GameData.Instance.isInDialogue = true;
        await RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(deathDialogueScript);


    }
    // Start is called before the first frame update
    void Start()
    {
        gameData = GameData.Instance;
        delay = 2f;
        string villagersLeft = "" + (31 - gameData.RunNumber);
        townGrowingSmallerText = "The Town Grows Smaller.\n<size=125>" + villagersLeft + "</size>\nVillagers Remain";
        textMeshToPrint.enabled = false;
        textMeshToPrint.text = townGrowingSmallerText;
        //textField = textObject.GetComponent<Text>();
        //textField.text = villagersLeft;
        playDeathDialogueAsync();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.isInDialogue) return;
        textMeshToPrint.enabled = true;
        if (waitAFrame) { waitAFrame = false; return; }

        if (Input.GetButtonDown("Submit"))
        {
            if (delay > 0) return;
            LoadEndRunScene();
        }

        //deathDiaglogPanel.SetActive(true);
        //button.SetActive(true);

        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            townGrowingSmallerText= "The Town Grows Smaller.\n<color=red><size=125>" + (30 - gameData.RunNumber) + 
                "</color></size>\nVillagers Remain";
            textMeshToPrint.text = townGrowingSmallerText;
            
            //textField.text = "" + (30 - gameData.RunNumber);
            // textField.color = Color.red;
            //Play Sound Effect Here
        }
        if (delay <= -4)
        {
            LoadEndRunScene();
        }
    }

    public void LoadEndRunScene()
    {
        SceneManager.LoadScene("EndRunScreen");
    }
}
