using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutPlayer : MonoBehaviour
{

    protected int phaseNumber = 0;
    public GameObject cutscenePlayerPrefab;
    protected GameObject instantiatedCutscenePlayer;
    protected GameObject playerObject;
    protected Camera mainCamera;
    public FadeIn fadeInController;
    protected bool waiting;
    protected float waitTime = 0;
    public List<bool> phases = new List<bool>();
    internal bool playingScene;
    public bool playDebug;

    public virtual void initialiseShortcutCutscene() {

    }
    protected void setupPlayerObject()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        mainCamera = playerObject.GetComponentInChildren<Camera>();
    }

    protected void setupBackInDungeon()
    {
        FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.UNLOCK_SHORTCUT);
        FinalWinterAchievementManager.Instance.CheckForAllShortcuts();
        Destroy(instantiatedCutscenePlayer);
        GameData.Instance.isInDialogue = false;
        mainCamera.enabled = true;
    }

    protected void setupCutsceneLocation(Vector3 transfromForCamera)
    {
        GameData.Instance.isInDialogue = true;
        mainCamera.enabled = false;
        instantiatedCutscenePlayer = Instantiate(cutscenePlayerPrefab, transfromForCamera, Quaternion.identity);
    }

}
