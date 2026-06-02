using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GraphNode : MonoBehaviour
{
    public List<GraphNode> neighbours = new List<GraphNode>();

    public float rad = 1.0f;


    [SerializeField] private DoorInteractable doorObject;

    public bool isDoor = false;

    public bool isBlocked = false;

    private readonly HashSet<Collider> blockingColliders = new HashSet<Collider>();
    private SphereCollider nodeCollider;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        SetupCollider();
    }

    private void OnEnable()
    {
        SetupCollider();
    }

    private void OnValidate()
    {
        SetupCollider();
    }

    private void Start()
    {
        if (isDoor && doorObject != null)
        {
            isBlocked = !doorObject.IsOpen();
        }
    }

    private void Update()
    {
        if (isDoor && doorObject != null)
        {
            isBlocked = !doorObject.IsOpen();
        }
    }

    private void SetupCollider()
    {
        nodeCollider = GetComponent<SphereCollider>();
        if (nodeCollider == null)
        {
            return;
        }

        nodeCollider.isTrigger = true;
        nodeCollider.center = Vector3.zero;
        nodeCollider.radius = Mathf.Max(0f, rad);
    }

    private static bool ShouldIgnoreCollider(Collider other)
    {
        if (other == null)
        {
            return true;
        }

        Transform root = other.transform.root;
        if (root != null && (root.CompareTag("Player") || root.CompareTag("Alien")))
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (isBlocked)
        {
            Gizmos.color = Color.red;
        } else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawSphere(transform.position, rad);

        Gizmos.color = Color.yellow;

        foreach (GraphNode neighbour in neighbours)
        {
            if (neighbour != null)
            {
                Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldIgnoreCollider(other))
        {
            return;
        }

        if (blockingColliders.Add(other))
        {
            isBlocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ShouldIgnoreCollider(other))
        {
            return;
        }

        if (blockingColliders.Remove(other))
        {
            isBlocked = blockingColliders.Count > 0;
        }
    }
}
