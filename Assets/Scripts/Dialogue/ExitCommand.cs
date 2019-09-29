using Naninovel;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("exit")]
public class ExitDialogue : Naninovel.Commands.Command
{
    public override async Task ExecuteAsync()
    {
        await Engine.GetService<StateManager>().ResetStateAsync();
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
    }

    public override Task UndoAsync() => Task.CompletedTask;
}