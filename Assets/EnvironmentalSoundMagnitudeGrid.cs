using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalSoundMagnitudeGrid : MonoBehaviour
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
    public int[,] grid;
    public string envSound;
    private Color[] gizColors = new Color[] { new Color (0, 1, 0),
                                             new Color (.25f,1,0),
                                             new Color (.5f,1,0),
                                             new Color (.75f,1,0),
                                             new Color (1,1,0),
                                             new Color (1,.75f,0),
                                             new Color (1,.5f,0),
                                             new Color (1,.25f,0),
                                             new Color (1,0 ,0),
                                             new Color (1, 0, .25f),
                                             new Color (1, .25f, .5f),
                                             new Color (0, .5f, 1)};

    internal void configureSoundGrid()
    {
        int width;
        int height;
        if (alternatePathabilitySetup) { width = altWidth; height = altHeight; }
        else { width = this.width; height = this.height; }

        grid = new int[width, height];

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
                grid[colNum, rowNum] = System.Convert.ToInt32(char.GetNumericValue(c));
                colNum++;
                if (colNum >= width) break;
            }
            rowNum++;
            if (rowNum >= height) break;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        configureSoundGrid();
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
                Gizmos.color = gizColors[grid[x, y]];
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
