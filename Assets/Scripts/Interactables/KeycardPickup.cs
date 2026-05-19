using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    [Header("UI")]
    public InteractionPromptUI interactionPromptUI;
    public string promptMessage = "Press E to pick up keycard";

    private bool playerNearby = false;
    private PlayerInventory playerInventory;

    void Start()
    {
        if (interactionPromptUI == null)
        {
            Debug.LogWarning("InteractionPromptUI reference is missing. Please assign it in the inspector.");
        }

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PickUpKeycard();
        }
    }

    void PickUpKeycard()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory not found on Player.");
            return;
        }

        playerInventory.AddKeycard();

        if (interactionPromptUI != null)
        {
            interactionPromptUI.HidePrompt();
        }

        Debug.Log("Keycard picked up.");

        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            playerInventory = other.GetComponent<PlayerInventory>();

            if (interactionPromptUI != null)
            {
                interactionPromptUI.ShowPrompt(promptMessage);
            }

            Debug.Log("Press E to pick up keycard");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            playerInventory = null;

            if (interactionPromptUI != null)
            {
                interactionPromptUI.HidePrompt();
            }

            Debug.Log("Player left keycard area");
        }
    }
}