using System.Collections.Generic;
using UnityEngine;

public static class  BFSPathfinder
{
    public static List<GraphNode> FindPath(GraphNode startNode, GraphNode goalNode)
    {
        if (startNode == null || goalNode == null || startNode.isBlocked || goalNode.isBlocked)
        {
            return null;
        }

        Queue<GraphNode> frontier = new Queue<GraphNode>();
        Dictionary<GraphNode, GraphNode> cameFrom = new Dictionary<GraphNode, GraphNode>();
        HashSet<GraphNode> visited = new HashSet<GraphNode>();

        frontier.Enqueue(startNode);
        visited.Add(startNode);
        cameFrom[startNode] = null;

        while (frontier.Count > 0)
        {
            GraphNode currentNode = frontier.Dequeue();

            if (currentNode == null || currentNode.isBlocked)
            {
                continue;
            }

            if (currentNode == goalNode)
            {
                return BuildPath(cameFrom, goalNode);
            }

            foreach (GraphNode neighbour in currentNode.neighbours)
            {
                if (neighbour == null || neighbour.isBlocked)
                    continue;

                if (!visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    cameFrom[neighbour] = currentNode;
                    frontier.Enqueue(neighbour);
                }
            }
        }

        return null;
    }

    private static List<GraphNode> BuildPath(Dictionary<GraphNode, GraphNode> cameFrom, GraphNode goalNode)
    {
        List<GraphNode> path = new List<GraphNode>();
        GraphNode currentNode = goalNode;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = cameFrom[currentNode];
        }

        path.Reverse();
        return path;
    }
}