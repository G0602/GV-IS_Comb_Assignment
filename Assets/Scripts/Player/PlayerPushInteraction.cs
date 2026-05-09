using UnityEngine;

public class PlayerPushInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float pushDistance = 5f;
    public float pushCooldown = 0.3f;

    [Header("UI")]
    public InteractionPromptUI interactionPromptUI;
    public string pushPromptMessage = "Press E to push";

    private float nextPushTime;
    private bool isLookingAtPushable = false;

    private void Update()
    {
        CheckPushablePrompt();

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextPushTime)
        {
            TryPush();
            nextPushTime = Time.time + pushCooldown;
        }
    }

    private void CheckPushablePrompt()
    {
        if (playerCamera == null)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pushDistance))
        {
            PushableObject pushable = hit.collider.GetComponent<PushableObject>();

            if (pushable != null)
            {
                if (!isLookingAtPushable)
                {
                    isLookingAtPushable = true;

                    if (interactionPromptUI != null)
                    {
                        interactionPromptUI.ShowPrompt(pushPromptMessage);
                    }
                }

                return;
            }
        }

        if (isLookingAtPushable)
        {
            isLookingAtPushable = false;

            if (interactionPromptUI != null)
            {
                interactionPromptUI.HidePrompt();
            }
        }
    }

    private void TryPush()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Player camera is not assigned.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pushDistance))
        {
            PushableObject pushable = hit.collider.GetComponent<PushableObject>();

            if (pushable != null)
            {
                pushable.Push(playerCamera.transform.forward);
                Debug.Log("Pushed object: " + hit.collider.name);
            }
        }
    }
}