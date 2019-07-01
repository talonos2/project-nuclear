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

    public bool IsThereAMonster(int xLoc, int yLoc)
    {
        if (grid[xLoc, yLoc] == null)
            return false;
        if (grid[xLoc, yLoc].GetComponent<EntityData>().isAMonster)
            return true;
        //grid[xLoc, yLoc]=this.transform.parent.gameObject;
        return false;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
