using Naninovel;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("exit")]
public class ExitDialogue : Naninovel.Commands.Command
{
    public override async Task ExecuteAsync()
    {
        await Engine.GetService<StateManager>().ResetStateAsync();
        GameState.isInBattle = false;
        //Assuming that this task is to exit cutscenes only:
        if (GameData.Instance.isCutscene)
        {
            GameData.Instance.isCutscene = false;
            StartDungeonRun.StartRun();
        }
    }

    public override Task UndoAsync() => Task.CompletedTask;
}