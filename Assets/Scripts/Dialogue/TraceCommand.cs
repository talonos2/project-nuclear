using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("trace")]
public class TraceCommand : Naninovel.Commands.Command
{
    [CommandParameter]
    public string word { get; set; }

    public override Task ExecuteAsync()
    {
        Debug.Log("Naninovel Trace: " + word);
        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
