using Naninovel;
using System.Collections;
using System.Collections.Generic;
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

    public string deathDialogueScript;

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
        delay = 2;
        string villagersLeft = "" + (31 - gameData.RunNumber);
        textField = textObject.GetComponent<Text>();
        textField.text = villagersLeft;
        playDeathDialogueAsync();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.isInDialogue) return;
        if (waitAFrame) { waitAFrame = false; return; }

        if (Input.GetButtonDown("Submit"))
        {
            LoadEndRunScene();
        }

        deathDiaglogPanel.SetActive(true);
        button.SetActive(true);

        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            textField.text = "" + (30 - gameData.RunNumber);
            textField.color = Color.red;
        }
        if (delay <= -8)
        {
            LoadEndRunScene();
        }
    }

    public void LoadEndRunScene()
    {
        SceneManager.LoadScene("EndRunScreen");
    }
}
