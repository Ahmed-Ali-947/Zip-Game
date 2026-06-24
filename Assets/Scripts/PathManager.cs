using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private GameLevelManager gameLevelManager;
    [SerializeField] private PathVisualManager pathVisualManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private WinChecker winChecker;
    public IReadOnlyList<Cell> CurrentPath => currentPath;
    public bool editorMode;

    private List<Cell> currentPath = new();
    private LevelSO levelData;

    void Start()
    {
        if (!editorMode)
        {
            levelData = gameLevelManager.GetCurrentLevelSO();
            winChecker.SetLevel(levelData);
        }
    }
    public bool BeginPath(Cell startCell)
    {
        if (startCell == null)
            return false;

        if (editorMode)
        {
            if (currentPath.Count == 0)
            {
                currentPath.Add(startCell);
                startCell.SetVisited(true);
                pathVisualManager.UpdatePathVisuals(currentPath);
                return true;
            }

            return startCell == currentPath[currentPath.Count - 1];
        }

        // No existing path -> start new
        if (currentPath.Count == 0)
        {
            bool isValidStart =
            startCell.HasNumber &&
            startCell.Number == 1;

            if (!isValidStart)
                return false;

            currentPath.Add(startCell);
            startCell.SetVisited(true);
            pathVisualManager.UpdatePathVisuals(currentPath);
            return true;
        }

        // Existing path -> only allow resume from endpoint
        Cell endCell = currentPath[currentPath.Count - 1];

        if (startCell != endCell)
            return false;

        return true;
    }

    public void TryMove(Cell newCell)
    {
        if (newCell == null || currentPath.Count == 0)
            return;

        Cell currentCell = currentPath[currentPath.Count - 1];

        if (newCell == currentCell)
            return;

        // Backtracking
        if (currentPath.Count >= 2)
        {
            Cell previous = currentPath[currentPath.Count - 2];

            if (newCell == previous)
            {
                UndoLast();
                return;
            }
        }

        if (currentCell.HasNumber && currentCell.Number == levelData.GetMaxNumber())
            return;

        if (!IsAdjacent(currentCell, newCell))
            return;

        if (newCell.Visited)
            return;

        currentPath.Add(newCell);
        newCell.SetVisited(true);

        pathVisualManager.UpdatePathVisuals(currentPath);
    }

    public void EndPath()
    {
        if (!editorMode)
            winChecker.CheckWin();
    }

    private void UndoLast()
    {
        if (currentPath.Count <= 1)
            return;

        Cell last = currentPath[currentPath.Count - 1];
        last.SetVisited(false);
        last.PathRenderer.sprite = null;

        currentPath.RemoveAt(currentPath.Count - 1);

        pathVisualManager.UpdatePathVisuals(currentPath);
    }

    private bool IsAdjacent(Cell a, Cell b)
    {
        Vector2Int diff = a.Coordinates - b.Coordinates;

        int dx = Mathf.Abs(diff.x);
        int dy = Mathf.Abs(diff.y);

        return dx + dy == 1;
    }

    public void UndoLastMoveBtn()
    {
        if (currentPath.Count >= 2)
        {
            UndoLast();
        }
    }

    public void GiveHint()
    {
        Vector2Int[] solution = levelData.solutionPath;

        if (solution == null || solution.Length == 0)
            return;

        int mismatchIndex = -1;

        // Find first incorrect step
        for (int i = 0; i < currentPath.Count; i++)
        {
            if (i >= solution.Length ||
                currentPath[i].Coordinates != solution[i])
            {
                mismatchIndex = i;
                break;
            }
        }

        // If path is incorrect, trim back to first wrong move
        if (mismatchIndex != -1)
        {
            while (currentPath.Count > mismatchIndex)
            {
                Cell last = currentPath[currentPath.Count - 1];
                last.SetVisited(false);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            pathVisualManager.UpdatePathVisuals(currentPath);
        }

        // Path already complete
        if (currentPath.Count >= solution.Length)
            return;

        // Add next correct cell
        Vector2Int nextCoords = solution[currentPath.Count];

        Cell nextCell = gridManager.GetCell(nextCoords);
        if (nextCell == null)
            return;

        currentPath.Add(nextCell);
        nextCell.SetVisited(true);

        pathVisualManager.UpdatePathVisuals(currentPath);
    }

    public void ResetPath()
    {
        foreach (Cell cell in currentPath)
        {
            cell.SetVisited(false);

            if (cell.PathRenderer != null)
                cell.PathRenderer.sprite = null;
        }

        currentPath.Clear();
        pathVisualManager.UpdatePathVisuals(currentPath);
    }
}