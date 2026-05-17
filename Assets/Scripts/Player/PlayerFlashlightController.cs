using UnityEngine;

public class PlayerFlashlightController : MonoBehaviour
{
    public GameObject flashlightObjectToPickUp; // The flashlight object in the world that the player can pick up

    public GameObject flashlightInHandObject; // Optional: Visual representation of the flashlight in the player's hand
    public KeyCode toggleKey = KeyCode.F;

    private bool hasFlashlight = false;
    private bool isFlashlightOn = false;

    void Start()
    {
        if ((flashlightObjectToPickUp != null) || (flashlightInHandObject != null))
        {
            flashlightObjectToPickUp.SetActive(true);
            flashlightInHandObject.SetActive(false);
        }
        else
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
        isFlashlightOn = true;

        if (flashlightObjectToPickUp != null)
        {
            flashlightObjectToPickUp.SetActive(false);
            flashlightInHandObject.SetActive(true);
        }

        Debug.Log("Torch picked up. Flashlight unlocked.");
        Debug.Log("Press F to turn ON/OFF torch light");
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;

        if (flashlightObjectToPickUp != null)
        {
            flashlightObjectToPickUp.SetActive(isFlashlightOn);
        }

        Debug.Log(isFlashlightOn ? "Flashlight ON" : "Flashlight OFF");
    }
}