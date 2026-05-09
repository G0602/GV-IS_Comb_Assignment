using UnityEngine;

public class PushableObject : MonoBehaviour
{
    public float pushForce = 4f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Push(Vector3 direction)
    {
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody found on " + gameObject.name);
            return;
        }

        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z).normalized;
        rb.AddForce(flatDirection * pushForce, ForceMode.Impulse);
    }
}