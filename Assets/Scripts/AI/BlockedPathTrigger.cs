using UnityEngine;

public class BlockedPathTrigger : MonoBehaviour
{
    public string edgeFrom = "B";
    public string edgeTo = "C";

    private int blockingObjectsInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            blockingObjectsInside++;

            if (blockingObjectsInside == 1)
            {
                GraphManager.Instance.BlockEdge(edgeFrom, edgeTo);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            blockingObjectsInside--;

            if (blockingObjectsInside <= 0)
            {
                blockingObjectsInside = 0;
                GraphManager.Instance.UnblockEdge(edgeFrom, edgeTo);
            }
        }
    }
}