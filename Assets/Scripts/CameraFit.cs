using UnityEngine;
using System;
public class CameraFit : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GridManager grid;

    private void Start()
    {
        grid.OnGridGenerated += GridManager_OnGridGenerated;
    }

    private void GridManager_OnGridGenerated(object s, EventArgs e)
    {
        FitToGrid();
    }
    public void FitToGrid()
    {
        Vector2 gridSize = grid.GetGridSize();

        float screenRatio = (float)Screen.width / Screen.height;
        float gridRatio = gridSize.x / gridSize.y;

        float size;
        if (screenRatio >= gridRatio)
        {
            size = gridSize.y / 2f;
        }
        else
        {
            float widthSize = (gridSize.x / 2f) / screenRatio;
            size = widthSize;
        }

        float padding = 1.2f;
        size *= padding;

        cam.orthographicSize = size;
    }
}