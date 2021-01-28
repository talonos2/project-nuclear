using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueNextRunButtonController : MonoBehaviour
{
    
    public Canvas canvas;
    private bool isActive;
    private float delayToShow;
    private bool initialStart;

    void Start()
    {
        delayToShow = .3f;
        canvas.enabled = false;
    }
    public void StartRunButtonClicked()
    {
        SoundManager.Instance.PlaySound("MenuOkay", 1f);

        GameState.setFullPause(false);
        GameData.Instance.inDungeon = true;
        CutsceneLoader.LoadCutsceneAndFade(canvas.GetComponent<Canvas>(), .5f);
        //StartDungeonRun.StartRun();
        //Load 'load game' ui screen
    }

    public void HighlightButtonSound() {
        SoundManager.Instance.PlaySound("MenuMove", 1f);
    }

    private void Update()
    {
        if (!initialStart) {
            if (GameData.Instance.isInDialogue || GameData.Instance.isCutscene) return;
        }

            if (delayToShow > 0)
            {
                delayToShow -= Time.deltaTime;
                canvas.enabled = false;
                return;
            }
            else initialStart = true;

        if (GameData.Instance.isInDialogue || GameData.Instance.isCutscene)
        {
            canvas.enabled = false;
        }
        else canvas.enabled = true;

    }
}
