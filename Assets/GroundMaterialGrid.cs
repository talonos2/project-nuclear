using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMaterialGrid : MonoBehaviour
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
    public GroundMaterialType[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        int width;
        int height;
        if (alternatePathabilitySetup) { width = altWidth; height = altHeight; }
        else { width = this.width; height = this.height; }

        grid = new GroundMaterialType[width, height];

        string[] yRows = passabilityMap.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        System.Array.Reverse(yRows);
        if (yRows.Length < height)
        {
            Debug.LogWarning(passabilityMap.name + " does not have enough rows; all missing rows will be filled with empty ground!");
        }
        if (yRows.Length > height)
        {
            Debug.LogWarning(passabilityMap.name + " has too many rows. All rows past #" + height + " will be ignored.");
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
            int colNum = 0;
            foreach (char c in rowString.ToCharArray())
            {
                switch (c)
                {
                    case '~':
                        grid[colNum, rowNum] = GroundMaterialType.SNOW;
                        break;
                    case ',':
                        grid[colNum, rowNum] = GroundMaterialType.GRASS;
                        break;
                    case '-':
                        grid[colNum, rowNum] = GroundMaterialType.DIRT;
                        break;
                    case '.':
                        grid[colNum, rowNum] = GroundMaterialType.STONE;
                        break;
                    case '=':
                        grid[colNum, rowNum] = GroundMaterialType.METAL;
                        break;
                    case '#':
                        grid[colNum, rowNum] = GroundMaterialType.WALL;
                        break;
                    default:
                        Debug.LogWarning("Got unexpected type " + c + " at " + colNum + " in row " + rowNum + ": Ignoring and replacing with errored ground material!");
                        grid[colNum, rowNum] = GroundMaterialType.ERROR;
                        break;
                }
                colNum++;
                if (colNum >= width) break;
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
                switch (grid[x, y])
                {
                    case GroundMaterialType.SNOW:
                        Gizmos.color = Color.white;
                        break;
                    case GroundMaterialType.GRASS:
                        Gizmos.color = Color.green;
                        break;
                    case GroundMaterialType.DIRT:
                        Gizmos.color = Color.yellow;
                        break;
                    case GroundMaterialType.STONE:
                        Gizmos.color = Color.gray;
                        break;
                    case GroundMaterialType.METAL:
                        Gizmos.color = Color.cyan;
                        break;
                    case GroundMaterialType.WALL:
                        Gizmos.color = Color.black;
                        break;
                    case GroundMaterialType.ERROR:
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
        else return true;

    }
}
