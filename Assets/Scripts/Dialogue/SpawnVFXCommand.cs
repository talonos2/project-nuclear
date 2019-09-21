using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("fwspawnvfx")]
public class SpawnVFXCOmmand : Naninovel.Commands.Command
{
    [CommandParameter]
    public string resourceName { get; set; }

    [CommandParameter]
    public float x { get; set; }

    [CommandParameter]
    public float y { get; set; }

    [CommandParameter]
    public float z { get; set; }

    public override Task ExecuteAsync()
    {
        GameObject vfx = GameObject.Instantiate(Resources.Load("Prefabs/VFX/"+resourceName) as GameObject);
        vfx.transform.position = new Vector3(x, y, z);
        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
