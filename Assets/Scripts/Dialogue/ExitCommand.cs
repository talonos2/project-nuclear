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
        Naninovel.Engine.Reset();
        GameData.Instance.isInDialogue = false;
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

        if (GameData.Instance.isCutscene && GameData.Instance.RunNumber == 1 && !CutsceneLoader.introCutscene) {
            GameData.Instance.isCutscene = false;
            StartDungeonRun.StartRun();
        }

    }

    public override Task UndoAsync() => Task.CompletedTask;
}