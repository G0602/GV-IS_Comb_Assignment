using System.Collections.Generic;
using UnityEngine;

public class AlienPathController : MonoBehaviour
{
    public string currentNode = "A";
    public string targetNode = "D";

    private List<string> currentPath = new List<string>();

    private void Start()
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