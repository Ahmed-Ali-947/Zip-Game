using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField] private PathManager pathManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int editorWidth = 7;
    [SerializeField] private int editorHeight = 7;
    private List<LevelSO.NumberedCell> clues = new();
    private int nextNumber = 1;

    private void Start()
    {
        gridManager.GenerateEmptyGrid(editorWidth, editorHeight);
    }

    public void AddClue(Cell cell)
    {
        if (cell == null)
            return;

        if (cell.HasNumber)
            return;

        LevelSO.NumberedCell clue;
        clue.position = cell.Coordinates;
        clue.number = nextNumber;

        clues.Add(clue);

        cell.SetNumber(nextNumber);

        Debug.Log($"Added clue {nextNumber} at {cell.Coordinates}");

        nextNumber++;
    }

    public void SaveLevel()
    {
#if UNITY_EDITOR
        if (pathManager.CurrentPath.Count == 0)
        {
            Debug.LogWarning("Cannot save: no solution path.");
            return;
        }

        LevelSO level = ScriptableObject.CreateInstance<LevelSO>();

        level.width = gridManager.Width;
        level.height = gridManager.Height;
        level.numberedCells = clues.ToArray();

        List<Vector2Int> solution = new();

        foreach (Cell cell in pathManager.CurrentPath)
        {
            solution.Add(cell.Coordinates);
        }

        level.solutionPath = solution.ToArray();

        string folder = "Assets/Levels";

        if (!AssetDatabase.IsValidFolder(folder))
        {
            AssetDatabase.CreateFolder("Assets", "Levels");
        }

        string assetPath =
            $"{folder}/Level_{level.width}x{level.height}_{System.DateTime.Now.Ticks}.asset";

        AssetDatabase.CreateAsset(level, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Level saved: {assetPath}");
#endif
    }

    public void ClearLevel()
    {
        clues.Clear();
        nextNumber = 1;

        Debug.Log("Editor level data cleared.");
    }
}