using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public List<GraphNode> neighbours = new List<GraphNode>();

    [SerializeField] private bool isDoor = false;

    public Vector3 Position => transform.position;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1.0f);

        Gizmos.color = Color.green;

        foreach (GraphNode neighbour in neighbours)
        {
            if (neighbour != null)
            {
                Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }
    }
}

