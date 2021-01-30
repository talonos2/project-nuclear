using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController_McDermit : Enemy
{
    private SpriteMovement playerObject;
    public string speakToMcDermitScript;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        if (testMonster)
        {
            return;
        }
        if (GameData.Instance.McDermit == 0 || GameData.Instance.bestTimes[7] > 600 || GameData.Instance.RunNumber <=3) {
            Destroy(this.gameObject);
        }

        //playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteMovement>();

    }
    /*void Update()
    {
        if (!GameData.Instance.spokenToMcDermit) {
            if (playerObject.distanceToEntity(this.transform) < 4)
            {
                GameData.Instance.spokenToMcDermit = true;
                playFoundMcDermitScript();
            }
        }

    }
    public async void playFoundMcDermitScript()
    {

        GameData.Instance.isInDialogue = true;
        await RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(speakToMcDermitScript);


    }*/
    override public void doUponDeath()
    {
        GameData.Instance.McDermit = 0;
        GameObject.Find("Canvas_VillagersMissing").GetComponent<MissingVillagerDropdownController>().SetAnimateUponVillagerDeath();
        FinalWinterAchievementManager.Instance.GiveAchievement(FWBoolAchievement.KILL_VILLAGER);
    }



}
