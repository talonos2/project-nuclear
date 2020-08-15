using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : DoodadData
{
    // Start is called before the first frame update
    //  void Start()
    //  {
    //       
    //   }
    public bool primBoulder;
    //public bool primShortcutB;
    public int primConnectionNumber;
    private BobPrim primToCheck;

    new void Start()
    {
        base.Start();
        primToCheck = GameObject.Find("Grid").GetComponent<BobPrim>();
        if (isOnCutsceneMap) primToCheck = GameObject.Find("Grid2").GetComponent<BobPrim>();
        if (primBoulder)
        {
            RunPrimAlgorythm();
        }



    }

    public void RunPrimAlgorythm()
    {
        if (primToCheck.result[primConnectionNumber])
        {
            SetBoulder(false);
        }
        else
        {
            
            SetBoulder(true);
        }
    }

    public void SetBoulder(bool active)
    {

        if (active)
        {
            this.isBlockableTerrain = true;
            this.isPassable = false;
            this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
        else {
            this.isBlockableTerrain = false;
            this.isPassable = true;
            this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
