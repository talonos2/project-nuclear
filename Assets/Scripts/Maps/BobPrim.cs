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

/*
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

        

        bool[] ending = new bool[connections.Length / 2];


        ending = Iterate(numNodes, connections);
        */
    }

    bool[] Iterate(int numNodes, int [,] connections)
    {
        System.Random rand = new System.Random();
        bool[] final = new bool[connections.Length];  //bool list to output
        int[] consider = new int[connections.Length];

        int i;
        int wallington = (connections.Length / 2);
        int lead;
        int check;
        int[] considered= new int[5];
        List<int> inside = new List<int>();
        List<int> walls = new List<int>();

        lead = rand.Next(numNodes);
        inside.Add(lead);


        
        for (i=0; i<wallington; i++)
        {
            if (connections[i, 0] == lead || connections[i, 1] == lead)
            {
                walls.Add(i);
            }
        }



       while (inside.Count<numNodes)
       {

            //check = walls[rand.Next(walls.Count)];
            check = rand.Next(walls.Count);

 /*           Debug.Log("check is " + check);
            Debug.Log("connections " + connections.Length);
            Debug.Log("walls count " + walls.Count);
            Debug.Log("walls has " + walls[0]);
            Debug.Log("walls check " + walls[check]);

            Debug.Log("I don't break things " + connections[walls[check], 0]);
            Debug.Log("I don't break things " + inside.Contains(connections[walls[check], 0]));*/

            if (inside.Contains(connections[walls[check], 0]) && inside.Contains(connections[walls[check], 1]))
            {
 //               Debug.Log("main if");
                final[walls[check]] = false;
            }
            else
            {
                final[walls[check]] = true;
                if (inside.Contains(connections[walls[check], 0]))
                {
//                    Debug.Log("second if");
                    Console.WriteLine(connections[walls[check], 1]);
                    inside.Add(connections[walls[check], 1]);

                    for (i = 0; i < wallington; i++)
                    {
                        if (connections[i, 0] == connections[walls[check], 1] || connections[i, 1] == connections[walls[check], 1])
                        {
                            walls.Add(i);
                        }
                    }

                }
                else
                {
 //                   Debug.Log("second else");
                    Console.WriteLine(connections[walls[check], 0]);
                    inside.Add(connections[walls[check], 0]);

                    for (i = 0; i < wallington; i++)
                    {
                        if (connections[i, 0] == connections[walls[check], 0] || connections[i, 1] == connections[walls[check], 0])
                        {
                            walls.Add(i);
                        }
                    }
                }
            }


            walls.Remove(walls[check]);
        }





//        Debug.Log("it ends");
//        for (i = 0; i <= wallington; i++)
//            Debug.Log("i is " + i + " " + final[i]);
        return final;
    }
}
