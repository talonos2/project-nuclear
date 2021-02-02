using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateCrystalSwitches : MonoBehaviour
{
    // Start is called before the first frame update
    private Renderer sRender;
    private Vector3 startPosit;
    public float bobSpeed = 1;
    public float bobHeight = .3f;
    public float bobOffset = .3f;

    void Start()
    {
        this.sRender = this.GetComponentInChildren<MeshRenderer>();
        startPosit = sRender.transform.localPosition;
    }

    // Update is called once per frame
    public void AnimateObject()
    {
        sRender.transform.localPosition = startPosit + new Vector3(0, Mathf.Sin(Time.timeSinceLevelLoad * bobSpeed + bobOffset) * bobHeight+.5f, 0);
    }

    void Update()
    {
        if (GameState.isInBattle || GameState.getFullPauseStatus())
        {
            return;
        }
        AnimateObject();
    }
}
