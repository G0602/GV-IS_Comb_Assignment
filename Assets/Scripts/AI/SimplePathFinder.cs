using System.Collections.Generic;

public static class SimplePathfinder
{
    private static Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>()
    {
        { "A", new List<string> { "B" } },
        { "B", new List<string> { "A", "C", "E" } },
        { "C", new List<string> { "B", "D" } },
        { "D", new List<string> { "C", "E" } },
        { "E", new List<string> { "B", "D" } }
    };

    public static List<string> FindPath(string start, string goal)
    {
        Queue<string> queue = new Queue<string>();
        Dictionary<string, string> cameFrom = new Dictionary<string, string>();

        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            string current = queue.Dequeue();

            if (current == goal)
            {
                return ReconstructPath(cameFrom, goal);
            }

            foreach (string neighbor in graph[current])
            {
                if (cameFrom.ContainsKey(neighbor))
                    continue;

                if (GraphManager.Instance.IsEdgeBlocked(current, neighbor))
                    continue;

                queue.Enqueue(neighbor);
                cameFrom[neighbor] = current;
            }
        }

        return null;
    }

    private static List<string> ReconstructPath(Dictionary<string, string> cameFrom, string goal)
    {
        List<string> path = new List<string>();
        string current = goal;

        while (current != null)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }
}