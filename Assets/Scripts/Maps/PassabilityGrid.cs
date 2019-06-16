using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassabilityGrid : MonoBehaviour
{
    public TextAsset passabilityMap;
    public int width = 3;
    public int height = 3;
    [HideInInspector]
    public PassabilityType[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new PassabilityType[width, height];
        string[] yRows = passabilityMap.text.Split('\n');
        if (yRows.Length < height)
        {
            Debug.LogWarning(passabilityMap.name + " does not have enough rows; all missing rows will be filled with empty ground!");
            return;
        }
        if (yRows.Length > height)
        {
            Debug.LogWarning(passabilityMap.name + " has too many rows. All rows past #"+height+" will be ignored.");
        }
        int rowNum = 0;
        foreach (string rowString in yRows)
        {
            Debug.Log(rowString);
            if (rowString.Length < width)
            {
                Debug.LogWarning("Row " + rowNum + " does not have enough squares; all missing squares will be filled with empty ground!");
            }
            if (rowString.Length > width)
            {
                Debug.LogWarning("Row " + rowNum + " does has too many squares; all extra squares will be ignored!");
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
                        Debug.LogWarning("Got unexpected type " + c + ": Ignoring and replacing with blank ground!");
                        grid[charNum, rowNum] = PassabilityType.NORMAL;
                        break;
                }
                charNum++;
                if (charNum >= width) break;
            }
            rowNum++;
            if (rowNum >= height) break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
