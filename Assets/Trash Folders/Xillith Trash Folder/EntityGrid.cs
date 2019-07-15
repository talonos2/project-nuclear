using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGrid : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[,] grid;
    private int Debugdelay = 1;
    private float resetDeb = 0;
    void Start()
    {
        InitializeEntityMap();
    }

    public void InitializeEntityMap()
    {
        grid = new GameObject[this.GetComponentInParent<PassabilityGrid>().width, this.GetComponentInParent<PassabilityGrid>().height];
    }

    public bool IsThereAMonster(int xLoc, int yLoc) {
        if (grid[xLoc,yLoc]==null)
            return false;
        if (grid[xLoc, yLoc].GetComponent<EntityData>().isAMonster)
            return true;
        //grid[xLoc, yLoc]=this.transform.parent.gameObject;
        return false;
        

    }

    // Update is called once per frame
    void Update()
    {
        //resetDeb += Time.deltaTime;
       // if (resetDeb > Debugdelay) {
       //     printEntityGridToScreen();
       //     resetDeb = 0;

       // }
    }

    private void printEntityGridToScreen()
    {
        int width = this.GetComponentInParent<PassabilityGrid>().width;
        int height = this.GetComponentInParent<PassabilityGrid>().height;
        if (grid == null)
        {
            return;
        }
        float xOffset = width / 2;
        float yOffset = height / 2;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null) { Gizmos.color = Color.red; }
                Debug.Log("x " + x + " y " + y);

                Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, .25f);
                Gizmos.DrawCube(new Vector3(x - xOffset + .5f, y - yOffset, 0f), new Vector3(1, 1, .2f));
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        int width = this.GetComponentInParent<PassabilityGrid>().width;
        int height = this.GetComponentInParent<PassabilityGrid>().height;
        if (grid == null)
        {
            return;
        }
        float xOffset = width / 2;
        float yOffset = height / 2;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null) { Gizmos.color = Color.red;
                    Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, .25f);
                    Gizmos.DrawCube(new Vector3(x - xOffset + .5f, y - yOffset, 0f), new Vector3(1, 1, .2f));
                }
                 
                

            }
        }
    }
}
