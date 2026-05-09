using UnityEngine;

public class PlayerPushInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float pushDistance = 5f;
    public float pushCooldown = 0.3f;

    private float nextPushTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextPushTime)
        {
            TryPush();
            nextPushTime = Time.time + pushCooldown;
        }
    }

    private void TryPush()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Player camera is not assigned.");
            return;
        }

        Debug.Log("E pressed. Trying to push...");

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pushDistance))
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            PushableObject pushable = hit.collider.GetComponent<PushableObject>();

            if (pushable != null)
            {
                pushable.Push(playerCamera.transform.forward);
                Debug.Log("Pushed object: " + hit.collider.name);
            }
            else
            {
                Debug.Log("Hit object is not pushable.");
            }
        }
        else
        {
            Debug.Log("Ray hit nothing.");
        }
    }
}