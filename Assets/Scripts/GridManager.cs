using UnityEngine;
using System;
public class GridManager : MonoBehaviour
{
    public static GridManager instance { get; private set; }
    public event EventHandler OnGridGenerated;
    [Header("Grid Settings")]
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector3 gridPadding;
    [Header("References")]
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform gridParent;

    [Header("Number Sprites")]
    [SerializeField] private Sprite[] numberSprites;


    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;

    private LevelSO levelData;
    private Cell[,] grid;
    private int width;
    private int height;
    private void Awake()
    {
        instance = this;
    }

    public void GenerateGrid(LevelSO level)
    {
        ClearGrid();

        levelData = level;
        width = levelData.width;
        height = levelData.height;
        grid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = GridToWorld(x, y);

                Cell cell = Instantiate(cellPrefab, pos, Quaternion.identity, gridParent);

                cell.Initialize(new Vector2Int(x, y));

                grid[x, y] = cell;
            }
        }

        ApplyLevelData(levelData);
        OnGridGenerated?.Invoke(this, EventArgs.Empty);

    }

    public void GenerateEmptyGrid(int width, int height)
    {
        LevelSO temp = ScriptableObject.CreateInstance<LevelSO>();
        temp.width = width;
        temp.height = height;
        temp.numberedCells = new LevelSO.NumberedCell[0];

        GenerateGrid(temp);
    }

    private void ClearGrid()
    {
        grid = null;

        if (gridParent == null)
            return;

        for (int i = gridParent.childCount - 1; i >= 0; i--)
        {
            Destroy(gridParent.GetChild(i).gameObject);
        }
    }
    private void ApplyLevelData(LevelSO level)
    {
        foreach (var item in level.numberedCells)
        {
            Cell cell = GetCell(item.position);

            if (cell == null)
                continue;

            int index = item.number;

            if (index >= 0 && index < numberSprites.Length)
            {
                cell.SetNumber(item.number, numberSprites[index]);
            }
            else
            {
                Debug.LogWarning($"Missing sprite for number {item.number}");
            }
        }
    }
    public Vector3 GridToWorld(int x, int y)
    {
        Vector3 offset = new Vector3(
            -(width * cellSize) / 2f + cellSize / 2f,
            -(height * cellSize) / 2f + cellSize / 2f,
            0f
        );

        Vector3 pos = new Vector3(
            x * cellSize,
            y * cellSize,
            0f
        );

        return pos + offset + gridPadding;
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        Vector3 offset = new Vector3(
            -(width * cellSize) / 2f,
            -(height * cellSize) / 2f,
            0f
        );

        Vector3 adjusted = worldPosition - offset - gridPadding;

        int x = Mathf.FloorToInt(adjusted.x / cellSize);
        int y = Mathf.FloorToInt(adjusted.y / cellSize);

        return new Vector2Int(x, y);
    }

    public bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public Cell GetCell(int x, int y)
    {
        if (!IsInsideGrid(x, y))
            return null;

        return grid[x, y];
    }

    public Cell GetCell(Vector2Int coords)
    {
        return GetCell(coords.x, coords.y);
    }

    public Vector2 GetGridSize()
    {
        return new Vector2(width, height) * cellSize;
    }
}