using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public List<GraphNode> neighbours = new List<GraphNode>();

    [SerializeField] private DoorInteractable doorObject;

    public bool isDoor = false;

    public Vector3 Position => transform.position;

    public bool IsOpen()
    {
        if (doorObject != null)
        {
            return doorObject.IsOpen();
        }
        return true; // If there's no door, consider it open
    }

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
