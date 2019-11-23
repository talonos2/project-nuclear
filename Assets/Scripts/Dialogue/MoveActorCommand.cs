using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("fwmovepawn")]
public class MoveActorCommand : Naninovel.Commands.Command
{
    [CommandParameter]
    public string n { get; set; }

    [CommandParameter]
    public string d { get; set; }

    public override Task ExecuteAsync()
    {
        GameObject go = GameObject.Find(n);
        if (go == null)
        {
            Debug.LogWarning("GO " + n + "Not found to move with fwmovepawn!");
            return Task.CompletedTask;
        }
        PawnMover mover = go.GetComponent<PawnMover>();
        if (mover == null)
        {
            Debug.LogWarning("No PawnMover on " + n + " to move with fwmovepawn!");
            return Task.CompletedTask;
        }
        mover.EnqueueMovement(d);

        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
