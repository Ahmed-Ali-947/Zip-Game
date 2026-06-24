using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "ZIP/Level")]
public class LevelSO : ScriptableObject
{
    [System.Serializable]
    public struct NumberedCell
    {
        public Vector2Int position;
        public int number;
    }

    public Vector2Int[] solutionPath;
    public int width;
    public int height;

    public NumberedCell[] numberedCells;

    public int GetMaxNumber()
    {
        int max = 0;
        foreach (NumberedCell numberedCell in numberedCells)
        {
            if (max <= numberedCell.number)
            {
                max = numberedCell.number;
            }
        }

        return max;
    }
}
