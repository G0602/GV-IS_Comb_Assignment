using UnityEngine;

public class PushableObject : MonoBehaviour
{
    [Header("Push Settings")]
    public float minPushSpeed = 0.1f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning("PushableObject needs a Rigidbody.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player touched pushable obstacle.");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (rb != null && rb.linearVelocity.magnitude > minPushSpeed)
            {
                Debug.Log("Pushable obstacle is moving.");
            }
        }
    }
}