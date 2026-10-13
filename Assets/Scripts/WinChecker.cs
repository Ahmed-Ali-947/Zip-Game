using UnityEngine;
using System;
using Unity.VisualScripting;
public class WinChecker : MonoBehaviour
{
    public event EventHandler OnGameWon;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PathManager pathManager;
    [SerializeField] private UIManager uiManager;

    private LevelSO levelData;

    public void CheckWin()
    {
        bool allCellsFilled = CheckAllCellsFilled();
        bool allNumbersVisited = CheckAllNumberedCellsVisited();
        bool numbersCorrect = CheckNumbersInOrder();

        if (allNumbersVisited && !numbersCorrect)
        {
            uiManager.UpdateGameStateText("Oops! All Numbers must be filled in order");
            return;
        }

        if (allNumbersVisited && numbersCorrect && !allCellsFilled)
        {
            uiManager.UpdateGameStateText("Oops! All cells must be filled to win");
            return;
        }

        if (allCellsFilled && allNumbersVisited && numbersCorrect)
        {
            OnGameWon?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool CheckAllNumberedCellsVisited()
    {
        foreach (var numberedCell in levelData.numberedCells)
        {
            Cell cell = gridManager.GetCell(numberedCell.position);

            if (cell == null)
                return false;

            if (!cell.Visited)
                return false;
        }

        return true;
    }
    private bool CheckAllCellsFilled()
    {
        for (int x = 0; x < levelData.width; x++)
        {
            for (int y = 0; y < levelData.width; y++)
            {
                Cell cell = gridManager.GetCell(new Vector2Int(x, y));

                if (cell == null)
                    return false;

                if (!cell.Visited)
                    return false;
            }
        }

        return true;
    }

    private bool CheckNumbersInOrder()
    {
        var path = pathManager.CurrentPath;
        int expected = 1;

        foreach (Cell cell in path)
        {
            if (!cell.HasNumber)
                continue;

            if (cell.Number != expected)
                return false;

            expected++;
        }

        return expected > levelData.numberedCells.Length;
    }

    public void SetLevel(LevelSO levelSO)
    {
        levelData = levelSO;
    }

}