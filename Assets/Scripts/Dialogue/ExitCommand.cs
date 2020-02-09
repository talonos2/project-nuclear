using Naninovel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[CommandAlias("exit")]
public class ExitDialogue : Naninovel.Commands.Command
{
    public override async Task ExecuteAsync()
    {
        //await Engine.GetService<StateManager>().ResetStateAsync();
        CutsceneLoader.waitingForScriptPlyr = false;
        Naninovel.Engine.Reset();
        GameData.Instance.isInDialogue = false;

        if (CutsceneLoader.postRun1Cutscene && GameData.Instance.isCutscene)
        {
            GameData.Instance.isCutscene = false;
            CutsceneLoader.postRun1Cutscene = false;
            SceneManager.LoadScene("TownMap_1");
            return;
        }

        //Assuming that this task is to exit cutscenes only:
        if (GameData.Instance.isCutscene && GameData.Instance.RunNumber >= 2)
        {
            GameData.Instance.isCutscene = false;
            SceneManager.LoadScene("ChooseItemScreen");
        }

        if (CutsceneLoader.introCutscene)
        {
            CutsceneLoader.LoadCutscene();
        }


        if (GameData.Instance.isCutscene && GameData.Instance.RunNumber == 1 && !CutsceneLoader.introCutscene&& !CutsceneLoader.postRun1Cutscene) {
            GameData.Instance.isCutscene = false;
            StartDungeonRun.StartRun();
        }

    }

    public override Task UndoAsync() => Task.CompletedTask;
}