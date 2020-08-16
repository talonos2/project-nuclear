using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CommandAlias("shakeScarecrow")]
public class shakeScarecrowCommand : Naninovel.Commands.Command
{


    public override Task ExecuteAsync()
    {
        GameObject go = GameObject.Find("PawnScarecrow");
        if (go == null)
        {
            Debug.LogWarning("Not found to move with shakeScarecrow!");
            return Task.CompletedTask;
        }
        Scarecrow_Interaction scarecrow = go.GetComponent<Scarecrow_Interaction>();
        if (scarecrow == null)
        {
            Debug.LogWarning("No PawnInteraction on to move with pawnclickobject!");
            return Task.CompletedTask;
        }
        //clickedObject.EnqueueMovement(d);

        scarecrow.ProcessClick(null);

        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
