[System.Serializable]
public class GraphEdge
{
    public string fromNode;
    public string toNode;
    public bool isBlocked;

    public GraphEdge(string fromNode, string toNode)
    {
        this.fromNode = fromNode;
        this.toNode = toNode;
        this.isBlocked = false;
    }

    public bool Matches(string a, string b)
    {
        return (fromNode == a && toNode == b) || (fromNode == b && toNode == a);
    }
}