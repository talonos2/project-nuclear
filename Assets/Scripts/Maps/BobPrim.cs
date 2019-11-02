/*:
*
*@plugindesc creates minimal spanning trees.  Requires number of nodes, and list of possible connections
*
*
*@author Bob
*
*@help
*
*
* 
*
*
*
*
*/


//console.log("starts");






using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public class BobPrim : MonoBehaviour
{
    void Start()
    {


        int numNodes = 9;
        int[,] connections = {
        { 0, 1},
        {1, 2},
        {3, 4},
        {4, 5},
        {6, 7},
        {7, 8},
        {0, 3},
        {3, 6},
        {1, 4},
        {4, 7},
        {2, 5},
        {5, 8}};

        bool[] ending = new bool[numNodes];


        ending = Iterate(numNodes, connections);

    }

    bool[] Iterate(int numNodes, int [,] connections)
    {
        System.Random rand = new System.Random();
        bool[] final = new bool[connections.Length];  //bool list to output
        int[] consider = new int[connections.Length];

        int i;
        int u;
        int lead;
        int check;
        int[] considered= new int[5];
        List<int> inside = new List<int>();
        List<int> walls = new List<int>();

        lead = rand.Next(numNodes);
        inside.Add(lead);


        
        for (i=0; i<numNodes; i++)
        {
            if (connections[i, 0] == lead || connections[i, 1] == lead)
                walls.Add(i);
        }



       while (inside.Count<numNodes)
       {

            check = walls[rand.Next(walls.Count)];

            if (inside.Contains(connections[walls[check], 0]) && inside.Contains(connections[walls[check], 1]))
                final[walls[check]] = false;
            else
            {
                final[walls[check]] = true;
                if (inside.Contains(connections[walls[check], 0]))
                    inside.Add(connections[walls[check], 1]);
                else
                    inside.Add(connections[walls[check], 0]);
            }


            walls.Remove(walls[check]);
        }

            




        return final;
    }
}
