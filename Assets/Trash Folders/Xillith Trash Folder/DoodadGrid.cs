using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodadGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[,] grid;
    void Start()
    {
        InitializeDoodadMap();
    }

    public void InitializeDoodadMap()
    {
        grid = new GameObject[this.GetComponentInParent<PassabilityGrid>().width, this.GetComponentInParent<PassabilityGrid>().height];
    }



    // Update is called once per frame
    void Update()
    {

    }
}
