using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class townAutoSaveController : MonoBehaviour
{
    private bool fromDungeon;
    private float timeToPlay=2.5f;
    private static int runCheck;
    private float initialWait = .3f;
    public TextMeshProUGUI textToShow;
    // Start is called before the first frame update
    void Start()
    {
        //if (GameData.Instance.deathTime > 0) {
        //    fromDungeon = true;
        //}
        if (GameData.Instance.loadingFromDungeon == false) Destroy(this);
        textToShow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.isCutscene || GameData.Instance.isInDialogue ) return;
        initialWait -= Time.deltaTime;
        if (initialWait > 0) return;
        if (runCheck != GameData.Instance.RunNumber)
        {
            
            runCheck = GameData.Instance.RunNumber;
            textToShow.enabled = true;
            GameData.Instance.loadingFromDungeon = false;
        }
        timeToPlay -= Time.deltaTime;

        if (timeToPlay < 0) {
            textToShow.enabled = false;
        }
    }
}
