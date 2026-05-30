using System.Collections.Generic;
using UnityEngine;

public class AlienPathController : MonoBehaviour
{
    public GraphNode currentNode;
    public GraphNode targetNode;

    private List<GraphNode> currentPath = new List<GraphNode>();

    private void Start()
    {
        RecalculatePath();
    }

    private void Update()
    {
        RecalculatePath();
    }

    public void RecalculatePath()
    {
        Debug.Log("Alien recalculating path...");

        currentPath = SimplePathfinder.FindPath(currentNode, targetNode);

        if (currentPath == null || currentPath.Count == 0)
        {
            Debug.LogWarning("No path found for alien.");
            return;
        }

        Debug.Log("New alien path: " + string.Join(" -> ", currentPath));
    }
}