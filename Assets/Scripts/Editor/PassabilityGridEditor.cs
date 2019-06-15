/*
 * Arthur Cousseau, 2017
 * https://www.linkedin.com/in/arthurcousseau/
 * Please share this if you enjoy it! :)
*/

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PassabilityGrid))]
public class PassabilityGrid : Editor
{
    private const int defaultCellSize = 25; // px

    private SerializedProperty grid;
    private SerializedProperty width;
    private SerializedProperty height;

    private Rect lastRect;

    void OnEnable()
    {
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");
        grid = serializedObject.FindProperty("cells");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Always do this at the beginning of InspectorGUI.

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(width);
        EditorGUILayout.PropertyField(height);
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck()) // Code to execute if grid size changed
        {
            InitNewGrid(width.intValue, height.intValue);
        }

        EditorGUILayout.Space();

        if (Event.current.type == EventType.Repaint)
        {
            lastRect = GUILayoutUtility.GetLastRect();
        }

        DisplayGrid(lastRect);

        serializedObject.ApplyModifiedProperties(); // Apply changes to all serializedProperties - always do this at the end of OnInspectorGUI.
    }

    private void InitNewGrid(int newX, int newY)
    {
        grid.ClearArray();

        for (int y = 0; y < newY; y++)
        {
            grid.InsertArrayElementAtIndex(y);
            SerializedProperty row = GetRowAt(y);

            for (int x = 0; x < newX; x++)
            {
                row.InsertArrayElementAtIndex(y);
            }
        }
    }

    private void DisplayGrid(Rect startRect)
    {
        Rect cellPosition = startRect;

        cellPosition.y += 5; // Same as EditorGUILayout.Space(), but in Rect

        cellPosition.size = Vector2.one * defaultCellSize;

        float startLineX = cellPosition.x;

        for (int i = 0; i < height.intValue; i++)
        {
            SerializedProperty row = GetRowAt(i);
            cellPosition.x = startLineX; // Get back to the beginning of the line

            for (int j = 0; j < width.intValue; j++)
            {
                EditorGUI.PropertyField(cellPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
                cellPosition.x += defaultCellSize;
            }

            cellPosition.y += defaultCellSize;
            GUILayout.Space(defaultCellSize); // If we don't do this, the next things we're going to draw after the grid will be drawn on top of the grid
        }
    }

    private SerializedProperty GetRowAt(int idx)
    {
        return grid.GetArrayElementAtIndex(idx).FindPropertyRelative("row");
    }
}