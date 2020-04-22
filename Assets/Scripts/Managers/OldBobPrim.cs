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


/*
var numNodes = 9;
var connections = [
        [0, 1],
        [1, 2],
        [3, 4],
        [4, 5],
        [6, 7],
        [7, 8],
        [0, 3],
        [3, 6],
        [1, 4],
        [4, 7],
        [2, 5],
        [5, 8]]
*/
/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobPrim : MonoBehaviour
{
    void Start()
    {

    }

    bool[] Iterate(int numNodes, int [][,] connections)
    {
        System.Random rand = new System.Random();

        int i;
        int u;


        int next;
        int[,] nextTwo = new int[0,0];
        int neighbors = 0;

        int side;
        int[][,] sideCells= new int[6][,];// = new int[];

        {
            sideCells[0] = new int[5, 5];
            sideCells[1] = new int[1, 1];
            sideCells[2] = new int[1, 1];
            sideCells[3] = new int[1, 1];
            sideCells[4] = new int[1, 1];
            sideCells[5] = new int[1, 1];
        };


        //        int inside = [];
        List<int[,]> inside = new List<int[,]>();
        ArrayList outside = new ArrayList(numNodes);

        for (i = 0; i < numNodes; i++)   //array storing its own length
        {
            outside.Add(i);
        }

        


        bool[] final = new bool[connections.Length];  //bool list to output

        for (i = 0; i < connections.Length; i++)
        {
            final[i] = false;
        }

        int[][,] outlinks = connections;

        List<int[,]> frontier = new List<int[,]>();
        //       var outlinks = connections;
        //       var links = [];
        //       var frontier = [];

        next = rand.Next(numNodes + 1);  //Math.Floor((Math.random() * numNodes));

        inside.Add(nextTwo);
        ///outside.splice(outside.indexOf(next), 1);

        outside.Insert(1, outside.IndexOf(next));

        //Array.IndexOf(outside, next);

        for (i = 0; i < outlinks.Length; i++)
        {
            ///if (outlinks[i][0] == next && frontier.indexOf(outlinks[i][1]) == -1)
            if (outlinks[i] == null)
            {
                frontier.Add(outlinks[i]);
                outside.Insert(1, outside.IndexOf(outlinks[i]));
            }
            else if (outlinks[i] == null)
            {
                frontier.Add(outlinks[i]);
                outside.Insert(1, outside.IndexOf(outlinks[i]));
            }
        }



        //    console.log("next is " + next);
        //    console.log("frontier is " + frontier);

        while (inside.Count < numNodes)
        {
            nextTwo = frontier[rand.Next(frontier.Count + 1)]; 

            inside.Add(nextTwo);
            frontier.Insert(1, frontier.IndexOf(nextTwo));

            //    console.log("new next is " + next);


            // finds all cells next to 'next' that are already mapped.  checks to see if there is more than one.
            for (i = 0; i < outlinks.Length; i++)
            {
                if (outlinks[i] == nextTwo)
                {
                    for (u = 0; u < inside.Count; u++)
                    {
                        if (outlinks[i] == inside[u])
                        {
                            //sideCells[neighbors] = { next, inside[u]};
                            sideCells[neighbors] = new int[next, inside[u]];////
                            neighbors++;
                        }
                    }


                }
                else if (outlinks[i] == nextTwo)
                {
                    for (u = 0; u < inside.Count; u++)
                    {
                        if (outlinks[i] == inside[u])
                        {
                            sideCells[neighbors] = new int[inside[u], next];
                            neighbors++;
                        }
                    }
                }
            }

            //  console.log("neighbors is  " + neighbors);


            if (neighbors > 1)
                side = rand.Next(neighbors+1);
            else
                side = 0;

        neighbors = 0;

            for (i = 0; i < connections.Length; i++)
            {
                if (sideCells[side] == connections[i])
                {
                    //          console.log("found it?");
                    final[i] = true;
                }
            }

            //    console.log("output list so far " + final);


            for (i = 0; i < outlinks.Length; i++)
            {
                if (outlinks[i] == nextTwo && inside.IndexOf(outlinks[i][0,0]) == -1 && frontier.IndexOf(outlinks[i]) == -1)  //-------should frontier's type be changed?
                {
                    frontier.Add(outlinks[i]);
                    outside.Insert(1, outside.IndexOf(outlinks[i]));
                }
                else if (outlinks[i] == nextTwo && inside.IndexOf(outlinks[i][0,0]) == -1 && frontier.IndexOf(outlinks[i]) == -1)
                {
                    frontier.Add(outlinks[i]);
                    outside.Insert(1, outside.IndexOf(outlinks[i]));
                }
            }

            //    console.log("new frontier is " + frontier);
        }

        return final;
    }
}
*/