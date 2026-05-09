using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager Instance;

    public List<GraphEdge> edges = new List<GraphEdge>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        edges.Add(new GraphEdge("A", "B"));
        edges.Add(new GraphEdge("B", "C"));
        edges.Add(new GraphEdge("C", "D"));
        edges.Add(new GraphEdge("B", "E"));
        edges.Add(new GraphEdge("E", "D"));

        Debug.Log("GraphManager initialized with " + edges.Count + " edges.");
    }

    public void BlockEdge(string fromNode, string toNode)
    {
        GraphEdge edge = FindEdge(fromNode, toNode);

        if (edge == null)
        {
            Debug.LogWarning("Cannot block edge. Edge not found: " + fromNode + " - " + toNode);
            return;
        }

        if (edge.isBlocked)
            return;

        edge.isBlocked = true;
        Debug.Log("Blocked edge: " + fromNode + " - " + toNode);
        RequestAlienPathRecalculation();
    }

    public void UnblockEdge(string fromNode, string toNode)
    {
        GraphEdge edge = FindEdge(fromNode, toNode);

        if (edge == null)
        {
            Debug.LogWarning("Cannot unblock edge. Edge not found: " + fromNode + " - " + toNode);
            return;
        }

        if (!edge.isBlocked)
            return;

        edge.isBlocked = false;
        Debug.Log("Unblocked edge: " + fromNode + " - " + toNode);
        RequestAlienPathRecalculation();
    }

    public bool IsEdgeBlocked(string fromNode, string toNode)
    {
        GraphEdge edge = FindEdge(fromNode, toNode);

        if (edge == null)
            return true;

        return edge.isBlocked;
    }

    private GraphEdge FindEdge(string fromNode, string toNode)
    {
        foreach (GraphEdge edge in edges)
        {
            if (edge.Matches(fromNode, toNode))
            {
                return edge;
            }
        }

        return null;
    }
    private void RequestAlienPathRecalculation()
    {
        AlienPathController alien = FindObjectOfType<AlienPathController>();

        if (alien != null)
        {
            alien.RecalculatePath();
        }
        else
        {
            Debug.LogWarning("AlienPathController not found in scene.");
        }
    }
}