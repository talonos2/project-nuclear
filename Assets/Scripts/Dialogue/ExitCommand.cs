using Naninovel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[CommandAlias("exit")]
public class ExitDialogue : Naninovel.Commands.Command
{
    public override async Task ExecuteAsync()
    {
        await Engine.GetService<StateManager>().ResetStateAsync();
        GameState.isInBattle = false;
        //Assuming that this task is to exit cutscenes only:
        if (GameData.Instance.isCutscene && GameData.Instance.RunNumber >= 2)
        {
            GameData.Instance.isCutscene = false;
            SceneManager.LoadScene("ChooseItemScreen");
        }
        if (GameData.Instance.isCutscene && GameData.Instance.RunNumber == 1) {
            GameData.Instance.isCutscene = false;
            StartDungeonRun.StartRun();
        }
    }

    public override Task UndoAsync() => Task.CompletedTask;
}