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

    public int numNodes = 9;
    public String[] connectionStrings;
    private int connectionCount;
    public int[,] connections;
    public bool[] result;
    void Start()
    {

        connectionCount = connectionStrings.Length;
        connections = new int[connectionCount, 2];
        //Debug.Log("hm"+  connections.Length);
        int i = 0;
        foreach (String str in connectionStrings){
            string[] numbers = str.Split(' ');
            connections[i, 0] = System.Convert.ToInt32(numbers[0]);
            connections[i, 1] = System.Convert.ToInt32(numbers[1]);
            Debug.Log("i " + i + "  is " + connections[i, 0] + " " + connections[i, 1]);
            i++;
        }



        result = Iterate(numNodes, connections, connectionCount);
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

    bool[] Iterate(int numNodes, int [,] connections, int connectionNum)
    {
        System.Random rand = new System.Random();
        bool[] final = new bool[connectionNum];  //bool list to output
        int[] consider = new int[connectionNum];

        int i;
        int wallington = (connections.Length / 2);
        int lead;
        int check;
        int[] considered= new int[5];
        List<int> inside = new List<int>();
        List<int> walls = new List<int>();
        List<int> takeout = new List<int>();

        int walliter = 0;

        lead = rand.Next(numNodes);
        inside.Add(lead);

        
        for (i=0; i<wallington; i++)
        {
            if (connections[i, 0] == lead || connections[i, 1] == lead)
            {
                walls.Add(i);
            }
        }

        Debug.Log("first room" + lead);

        i = 0;

        //
//        foreach (int wall in walls)
//        {
//            Debug.Log("walls #" + walliter + "    " + wall + "  is " + connections[wall, 0] + " " + connections[wall, 1]);
//        }


 //       while (inside.Count<numNodes)
        while (walls.Count>0)
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
                Debug.Log("final " + walls[check] + " is false");

            }
            else
            {
                final[walls[check]] = true;
                Debug.Log("final " + walls[check] + " is true");
                if (inside.Contains(connections[walls[check], 0]))
                {
                    Debug.Log("final : inside true");
                    //                    Debug.Log("second if");
                    //                    Console.WriteLine(connections[walls[check], 1]);
                    ///////if (walls.Contains(connections[walls[check], 1]) == false)
                    if (inside.Contains(connections[walls[check], 1])==false)  ////is it breaking here?
                    {
                        inside.Add(connections[walls[check], 1]);
                        Debug.Log("new inside = " + connections[walls[check], 1]);
                    }

                    for (i = 0; i < wallington; i++)
                    {
                        if (connections[i, 0] == connections[walls[check], 1] || connections[i, 1] == connections[walls[check], 1])
                        {
                            if (walls.Contains(i)==false)
                                walls.Add(i);
                        }
                    }

                }
                else
                {
                    //                   Debug.Log("second else");
                    //                   Console.WriteLine(connections[walls[check], 0]);
                    if (inside.Contains(connections[walls[check], 0]) == false)
                    {
                        inside.Add(connections[walls[check], 0]);
                        Debug.Log("new inside = " + connections[walls[check], 0]);
                    }

                    for (i = 0; i < wallington; i++)
                    {
                        if (connections[i, 0] == connections[walls[check], 0] || connections[i, 1] == connections[walls[check], 0])
                        {
                            if (walls.Contains(i) == false)
                                walls.Add(i);
                        }
                    }
                }
            }


            //           foreach (int wall in walls)
            //           {
            //               Debug.Log("preremove " + walliter + "    " + wall + "  is " + connections[wall, 0] + " " + connections[wall, 1]);
            //           }


            foreach (int room in inside)
            {
                Debug.Log("insides #" + walliter + "    " + room);
            }

            foreach (int wall in walls)
            {
                Debug.Log("walls #" + walliter + "    " + wall + "  is " + connections[wall, 0] + " " + connections[wall, 1]);
            }
            walliter++;

            Debug.Log("removing   " + walls[check]);
            walls.Remove(walls[check]);


        }





        Debug.Log("it ends");
        for (i = 0; i < wallington; i++)
            Debug.Log("i is " + i + " " + final[i]);
        return final;
    }
}
