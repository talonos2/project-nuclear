﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("fwdeletego")]
public class DeleteGOCommand : Naninovel.Commands.Command
{
    [CommandParameter]
    public string n { get; set; }

    public override Task ExecuteAsync()
    {
        IEnumerable<GameObject> matches = Object.FindObjectsOfType<GameObject>().Where(obj => obj.name == n);

        if (matches.Count() == 0)
        {
            Debug.LogWarning("There are no GOs named " + n + " to delete fwdeletego.");
            return Task.CompletedTask;
        }
        if (matches.Count() > 1)
        {
            Debug.LogWarning("There are multiple GOs named " + n + " that could be deleted with fwdeletego. I will delete them all. This might not be what you intend.");
        }

        foreach (GameObject go in matches)
        {
            GameObject.Destroy(go);
        }

        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
