using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HerosSurvivedController : MonoBehaviour
{
    public Image hiro;
    public Image guard;
    public Image squire;
    public Image greenMaid;
    public Image priest;
    public Image cultest;
    public Image redMaid;
    public Image blueMaid;
    public Image trapper;
    public Image blacksmith;
    private float delay = 5;
    public SwitchMapMusicOnMapStart switchMusicManager;

    // Start is called before the first frame update
    void Start()
    {
        GameData.Instance.inDungeon = false;
        int deadPeople = 0;
        if (GameData.Instance.Douglass == 0) { hiro.enabled = false; deadPeople++; }
        if (GameData.Instance.Sara == 0) {blueMaid.enabled = false; deadPeople++; }
        if (GameData.Instance.McDermit == 0) {guard.enabled = false; deadPeople++; }
        if (GameData.Instance.Todd == 0) {cultest.enabled = false; deadPeople++; }
        if (GameData.Instance.Norma == 0) {redMaid.enabled = false; deadPeople++; }
        if (GameData.Instance.Derringer == 0) {blacksmith.enabled = false; deadPeople++; }
        if (GameData.Instance.Melvardius == 0) {priest.enabled = false; deadPeople++; }
        if (GameData.Instance.Mara == 0) {greenMaid.enabled = false; deadPeople++; }
        if (GameData.Instance.Devon == 0) {trapper.enabled = false; deadPeople++; }
        if (GameData.Instance.Pendleton == 0) {squire.enabled = false; deadPeople++; }
        FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.WIN_GAME);
        if (GameData.Instance.RunNumber == 4)
        {
            FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.WIN_GAME_AS_TODD);
        }
        if (GameData.Instance.RunNumber == 30)
        {
            FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.WIN_GAME_AS_ELDER);
        }
        if (deadPeople == 0)
        {
            FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.WIN_GAME_NO_VILLAGE_KILLS);
        }

    }

    private void Update()
    {
        if (GameData.Instance.isInDialogue || GameData.Instance.isCutscene) return;
        delay -= Time.deltaTime;

        if (delay < 0) {
            SceneManager.LoadScene("Credits");
        }

    }

}
