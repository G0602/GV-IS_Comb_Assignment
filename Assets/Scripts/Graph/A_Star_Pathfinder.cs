using System.Collections.Generic;
using UnityEngine;

public static class A_Star_Pathfinder
{
    public static List<GraphNode> FindPath(GraphNode start, GraphNode goal)
    {
        if (start == null || goal == null)
        {
            return null;
        }

        List<GraphNode> openSet = new List<GraphNode> { start };
        HashSet<GraphNode> closedSet = new HashSet<GraphNode>();
        Dictionary<GraphNode, GraphNode> cameFrom = new Dictionary<GraphNode, GraphNode>();
        Dictionary<GraphNode, float> gScore = new Dictionary<GraphNode, float>
        {
            [start] = 0f
        };

        while (openSet.Count > 0)
        {
            GraphNode current = GetLowestScoreNode(openSet, gScore, goal);

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (GraphNode neighbor in current.neighbours)
            {
                if (neighbor == null || closedSet.Contains(neighbor))
                {
                    continue;
                }

                // if (IsBlocked(current, neighbor))
                // {
                //     continue;
                // }

                if(neighbor.isDoor && !neighbor.IsOpen())
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + Distance(current, neighbor);
                float existingGScore = gScore.TryGetValue(neighbor, out float score) ? score : float.PositiveInfinity;

                if (tentativeGScore >= existingGScore)
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    private static GraphNode GetLowestScoreNode(List<GraphNode> openSet, Dictionary<GraphNode, float> gScore, GraphNode goal)
    {
        GraphNode bestNode = openSet[0];
        float bestScore = GetEstimatedTotalCost(bestNode, gScore, goal);

        for (int i = 1; i < openSet.Count; i++)
        {
            GraphNode node = openSet[i];
            float score = GetEstimatedTotalCost(node, gScore, goal);

            if (score < bestScore)
            {
                bestNode = node;
                bestScore = score;
            }
        }

        return bestNode;
    }

    private static float GetEstimatedTotalCost(GraphNode node, Dictionary<GraphNode, float> gScore, GraphNode goal)
    {
        float costSoFar = gScore.TryGetValue(node, out float score) ? score : float.PositiveInfinity;
        return costSoFar + Distance(node, goal);
    }

    private static float Distance(GraphNode a, GraphNode b)
    {
        return Vector3.Distance(a.Position, b.Position);
    }

    // private static bool IsBlocked(GraphNode from, GraphNode to)
    // {
    //     GraphManager graphManager = GraphManager.Instance;

    //     if (graphManager == null || !graphManager.HasEdge(from.name, to.name))
    //     {
    //         return false;
    //     }

    //     return graphManager.IsEdgeBlocked(from.name, to.name);
    // }

    private static List<GraphNode> ReconstructPath(Dictionary<GraphNode, GraphNode> cameFrom, GraphNode goal)
    {
        List<GraphNode> path = new List<GraphNode>();
        GraphNode current = goal;

        while (current != null)
        {
            path.Add(current);
            cameFrom.TryGetValue(current, out current);
        }

        path.Reverse();
        return path;
    }
}
