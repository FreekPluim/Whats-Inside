using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeReciever))]

public class CustomMazeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MazeReciever maze = (MazeReciever)target;

        if(GUILayout.Button("Generate Maze"))
        {
            maze.CreateMaze();
        }

        if (GUILayout.Button("DestroyMaze"))
        {
            maze.ClearMaze();
        }
    }
}
