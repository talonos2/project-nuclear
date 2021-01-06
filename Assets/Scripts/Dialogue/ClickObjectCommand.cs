using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("pawnclickobject")]
public class ClickObjectCommand : Naninovel.Commands.Command
{
    [CommandParameter]
    public string n { get; set; }

    public override Task ExecuteAsync()
    {
        GameObject go = GameObject.Find(n);
        if (go == null)
        {
            Debug.LogWarning("GO " + n + "Not found to move with pawnclickobject!");
            return Task.CompletedTask;
        }
        PawnInteraction clickedObject = go.GetComponent<PawnInteraction>();
        if (clickedObject == null)
        {
            Debug.LogWarning("No PawnInteraction on " + n + " to move with pawnclickobject!");
            return Task.CompletedTask;
        }
        //clickedObject.EnqueueMovement(d);
        clickedObject.SetPunchAnyway() ;

        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
