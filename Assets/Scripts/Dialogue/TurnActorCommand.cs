using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("fwturnpawn")]
public class TurnActorCommand : Naninovel.Commands.Command
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
            Debug.LogWarning("GO " + n + "Not found to move with fwturnpawn!");
            return Task.CompletedTask;
        }
        PawnMover mover = go.GetComponent<PawnMover>();
        foreach (MonoBehaviour m in go.GetComponents<MonoBehaviour>())
        {
            //Debug.Log(m);
        }
        if (mover == null)
        {
            Debug.LogWarning("No PawnMover on " + n + " to move with fwturnpawn!");
            return Task.CompletedTask;
        }
        mover.EnqueueTurn(d);

        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
