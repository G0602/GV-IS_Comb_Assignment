using UnityEngine;

public class PlayerFlashlightController : MonoBehaviour
{
    public GameObject flashlightObjectToPickUp; // The flashlight object in the world that the player can pick up

    public GameObject flashlightInHandObject; // Optional: Visual representation of the flashlight in the player's hand

    private InteractionPromptUI interactionPromptUI;
    public KeyCode toggleKey = KeyCode.F;

    private bool hasFlashlight = false;
    private bool isFlashlightOn = false;
    private Light[] flashlightLights;

    public bool HasFlashlight => hasFlashlight;
    public bool IsFlashlightOn => hasFlashlight && isFlashlightOn;

    void Start()
    {
        interactionPromptUI = FindAnyObjectByType<InteractionPromptUI>();
        
        if (interactionPromptUI != null)
        {
            interactionPromptUI.ShowPrompt("Objective 1: Find the torch to unlock the flashlight.");
        } else
        {
            Debug.LogWarning("InteractionPromptUI not found in the scene. Flashlight pickup prompt will not be shown.");
        }

        if (flashlightObjectToPickUp != null)
        {
            flashlightObjectToPickUp.SetActive(true);
        }

        if (flashlightInHandObject != null)
        {
            CacheFlashlightLights();
            flashlightInHandObject.SetActive(false);
        }

        if (flashlightObjectToPickUp == null || flashlightInHandObject == null)
        {
            Debug.LogWarning("Flashlight Objects are not assigned on Player properly.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (!hasFlashlight)
            {
                Debug.Log("You need to pick up the torch first.");
                return;
            }

            ToggleFlashlight();
        }
    }

    public void GiveFlashlight()
    {
        hasFlashlight = true;
        isFlashlightOn = false;

        if (flashlightObjectToPickUp != null)
        {
            flashlightObjectToPickUp.SetActive(false);
        }

        if (flashlightInHandObject != null)
        {
            flashlightInHandObject.SetActive(true);
            CacheFlashlightLights();
            SetFlashlightBeamActive(false);
        }

        Debug.Log("Torch picked up. Flashlight unlocked.");
        Debug.Log("Press F to turn ON/OFF torch light");
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;

        if (flashlightInHandObject != null)
        {
            flashlightInHandObject.SetActive(true);
            SetFlashlightBeamActive(isFlashlightOn);
        }

        if (interactionPromptUI != null)
        {
            interactionPromptUI.ShowPrompt(isFlashlightOn ? "Flashlight ON" : "Flashlight OFF");
        }

        Debug.Log(isFlashlightOn ? "Flashlight ON" : "Flashlight OFF");
    }

    void CacheFlashlightLights()
    {
        if (flashlightInHandObject == null)
        {
            flashlightLights = null;
            return;
        }

        flashlightLights = flashlightInHandObject.GetComponentsInChildren<Light>(true);
    }

    void SetFlashlightBeamActive(bool active)
    {
        if (flashlightLights == null)
        {
            CacheFlashlightLights();
        }

        if (flashlightLights == null)
        {
            return;
        }

        foreach (Light flashlightLight in flashlightLights)
        {
            if (flashlightLight != null)
            {
                flashlightLight.enabled = active;
            }
        }
    }
}
