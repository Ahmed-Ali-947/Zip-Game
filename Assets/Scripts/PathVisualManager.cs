using UnityEngine;
using System.Collections.Generic;

public class PathVisualManager : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Gradient[] gradients;
    [SerializeField] private float lineDefaultWidth;
    [SerializeField] private float lineSingleWidth;

    private void Start()
    {
        if (gradients.Length == 0)
            return;

        int randomIndex = Random.Range(0, gradients.Length);
        lineRenderer.colorGradient = gradients[randomIndex];
    }
    public void UpdatePathVisuals(List<Cell> currentPath)
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        // Special case: only one selected cell
        if (currentPath.Count == 1)
        {
            Vector3 startPos = currentPath[0].transform.position;
            startPos.z = 0f;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, startPos);
            lineRenderer.startWidth = lineSingleWidth;
            return;
        }

        lineRenderer.startWidth = lineDefaultWidth;

        lineRenderer.positionCount = currentPath.Count;

        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector3 pos = currentPath[i].transform.position;
            pos.z = 0f;
            lineRenderer.SetPosition(i, pos);
        }
    }
}