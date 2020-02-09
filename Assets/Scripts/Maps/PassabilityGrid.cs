using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassabilityGrid : MonoBehaviour
{
    public TextAsset passabilityMap;
    public int width = 3;
    public int height = 3;
    public bool alternatePathabilitySetup;
    public int altWidth = 0;
    public int altHeight = 0;
    //internal int width;
    //internal int height;
    [HideInInspector]
    public PassabilityType[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        int width;
        int height;
        if (alternatePathabilitySetup) { width = altWidth; height = altHeight; }
        else { width = this.width; height = this.height; }

        grid = new PassabilityType[width, height];

        string[] yRows = passabilityMap.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        System.Array.Reverse(yRows);
        if (yRows.Length < height)
        {
            Debug.LogWarning(passabilityMap.name + " does not have enough rows; all missing rows will be filled with empty ground!");
        }
        if (yRows.Length > height)
        {
            Debug.LogWarning(passabilityMap.name + " has too many rows. All rows past #"+height+" will be ignored.");
        }
        int rowNum = 0;
        foreach (string rowString in yRows)
        {
            if (rowString.Length < width)
            {
                Debug.LogWarning("Row " + rowNum + " does not have enough squares; all missing squares will be filled with empty ground!");
            }
            if (rowString.Length > width)
            {
                Debug.LogWarning("Row " + rowNum + " has too many squares; all extra squares will be ignored!");
            }
            int charNum = 0;
            foreach (char c in rowString.ToCharArray())
            {
                switch (c)
                {
                    case '-':
                        grid[charNum, rowNum] = PassabilityType.NORMAL;
                        break;
                    case '+':
                        grid[charNum, rowNum] = PassabilityType.MONSTER;
                        break;
                    case 'v':
                        grid[charNum, rowNum] = PassabilityType.AIR;
                        break;
                    case '#':
                        grid[charNum, rowNum] = PassabilityType.WALL;
                        break;
                    default:
                        Debug.LogWarning("Got unexpected type " + c + " at "+ charNum + " in row "+ rowNum +": Ignoring and replacing with errored ground!");
                        grid[charNum, rowNum] = PassabilityType.ERROR;
                        break;
                }
                charNum++;
                if (charNum >= width) break;
            }
            rowNum++;
            if (rowNum >= height) break;
        }
    }


    void OnDrawGizmosSelected()
    {
        if (grid == null)
        {
            return;
        }

        int width;
        int height;
        if (alternatePathabilitySetup) { width = altWidth; height = altHeight; }
        else { width = this.width; height = this.height; }


        float xOffset = width / 2;
        float yOffset = height / 2;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (grid[x,y])
                {
                    case PassabilityType.NORMAL:
                        Gizmos.color = Color.green;
                        break;
                    case PassabilityType.MONSTER:
                        Gizmos.color = Color.yellow;
                        break;
                    case PassabilityType.AIR:
                        Gizmos.color = Color.blue;
                        break;
                    case PassabilityType.WALL:
                        Gizmos.color = Color.black;
                        break;
                    case PassabilityType.ERROR:
                        Gizmos.color = Color.magenta;
                        break;
                }
                Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, .25f);
                Gizmos.DrawCube(new Vector3(x - xOffset + .5f, y - yOffset, 0), new Vector3(1, 1, .2f));
            }
        }
    }

    public Vector2 GridToTransform(Vector2 gridPosition)
    {
        int width;
        int height;
        if (alternatePathabilitySetup) { width = altWidth; height = altHeight; }
        else { width = this.width; height = this.height; }

        return new Vector2(gridPosition.x - width / 2, gridPosition.y - height / 2);
    }

    internal bool InRange(int locX, int locY)
    {
        int width;
        int height;
        if (alternatePathabilitySetup) { width = altWidth; height = altHeight; }
        else { width = this.width; height = this.height; }

        if (locX < 0 || locY < 0 || locX >= width || locY >= width)
        {
            return false;
        }
        else return true ;
        
    }
}
