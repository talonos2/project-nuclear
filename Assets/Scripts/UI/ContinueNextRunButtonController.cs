using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueNextRunButtonController : MonoBehaviour
{
    
    public Canvas canvas;
    private bool isActive;

    void Start()
    {

    }
    public void StartRunButtonClicked()
    {
        SoundManager.Instance.PlaySound("MenuOkay", 1f);
        GameState.fullPause = false;
        CutsceneLoader.LoadCutsceneAndFade(canvas.GetComponent<Canvas>(), .5f);
        //StartDungeonRun.StartRun();
        //Load 'load game' ui screen
    }

    private void Update()
    {

        if (GameData.Instance.isInDialogue || GameData.Instance.isCutscene)
        {
            canvas.enabled = false;
        }
        else canvas.enabled = true;

    }
}
