using System.Collections.Generic;
using UnityEngine;

public class PathLineRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float lineHeight = 0.3f;

    public void DrawPath(List<GraphNode> path)
    {
        if (path == null || path.Count == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 position = path[i].transform.position + Vector3.up * lineHeight;
            lineRenderer.SetPosition(i, position);
        }
    }

    public void ClearPath()
    {
        lineRenderer.positionCount = 0;
    }
}