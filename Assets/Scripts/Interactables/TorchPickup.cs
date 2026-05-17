using UnityEngine;

public class TorchPickup : MonoBehaviour
{
    [Header("UI")]
    public InteractionPromptUI interactionPromptUI;
    public string promptMessage = "Press E to pick up torch";

    private bool playerNearby = false;
    private PlayerFlashlightController playerFlashlightController;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PickUpTorch();
        }
    }

    void PickUpTorch()
    {
        if (playerFlashlightController == null)
        {
            Debug.LogWarning("PlayerFlashlightController not found on player.");
            return;
        }

        playerFlashlightController.GiveFlashlight();

        if (interactionPromptUI != null)
        {
            interactionPromptUI.HidePrompt();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            playerFlashlightController = other.GetComponent<PlayerFlashlightController>();

            if (interactionPromptUI != null)
            {
                interactionPromptUI.ShowPrompt(promptMessage);
            }

            Debug.Log("Press E to pick up torch");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            playerFlashlightController = null;

            if (interactionPromptUI != null)
            {
                interactionPromptUI.HidePrompt();
            }

            Debug.Log("Player left torch area");
        }
    }
}