using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CommandAlias("fwspawnvfx")]
public class SpawnVFXCOmmand : Naninovel.Commands.Command
{
    [CommandParameter]
    public string vfx { get; set; }

    [CommandParameter]
    public float x { get; set; }

    [CommandParameter]
    public float y { get; set; }

    [CommandParameter]
    public float z { get; set; }

    public override Task ExecuteAsync()
    {
        GameObject vfxgo = GameObject.Instantiate(Resources.Load("Prefabs/VFX/"+vfx) as GameObject);
        vfxgo.name = vfx;
        vfxgo.transform.position = new Vector3(x, y, z);
        return Task.CompletedTask;
    }

    public override Task UndoAsync()
    {
        throw new System.NotImplementedException();
    }
}
