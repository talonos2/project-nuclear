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
