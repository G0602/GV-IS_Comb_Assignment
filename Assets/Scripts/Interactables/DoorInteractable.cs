using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    [Header("Door Part")]
    public Transform doorPart;

    [Header("Door Rotation")]
    public Vector3 closedRotation = new Vector3(0f, 0f, 0f);
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);

    [Header("Settings")]
    public float openSpeed = 5f;

    [Header("UI")]
    public InteractionPromptUI interactionPromptUI;
    public string promptMessage = "Press E to open/close door";

    private bool isOpen = false;
    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }

        RotateDoor();
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
            Debug.Log("Door opened");
        else
            Debug.Log("Door closed");
    }

    void RotateDoor()
    {
        if (doorPart == null) return;

        Quaternion targetRotation = isOpen
            ? Quaternion.Euler(openRotation)
            : Quaternion.Euler(closedRotation);

        doorPart.localRotation = Quaternion.Slerp(
            doorPart.localRotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (interactionPromptUI != null)
            {
                interactionPromptUI.ShowPrompt(promptMessage);
            }

            Debug.Log("Press E to open/close door");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (interactionPromptUI != null)
            {
                interactionPromptUI.HidePrompt();
            }

            Debug.Log("Player left door area");
        }
    }
}