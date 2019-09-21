using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("fwmovepawn")]
public class MoveActorCommand : Naninovel.Commands.Command
{
    [CommandParameter]
    public string pawnName { get; set; }

    [CommandParameter]
    public string direction { get; set; }

    public override Task ExecuteAsync()
    {
        Debug.Log("Trying to move " + pawnName);
        GameObject go = GameObject.Find(pawnName);
        if (go == null)
        {
            Debug.LogWarning("GO " + pawnName + "Not found to move with fwmovepawn!");
            return Task.CompletedTask;
        }
        PawnMover mover = go.GetComponent<PawnMover>();
        if (mover == null)
        {
            Debug.LogWarning("No PawnMover on " + pawnName + " to move with fwmovepawn!");
            return Task.CompletedTask;
        }
        mover.EnqueueMovement(direction);

        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
