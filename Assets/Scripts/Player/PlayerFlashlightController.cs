using UnityEngine;

public class PlayerFlashlightController : MonoBehaviour
{
    public GameObject flashlightObject;
    public KeyCode toggleKey = KeyCode.F;

    private bool hasFlashlight = false;
    private bool isFlashlightOn = false;

    void Start()
    {
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Flashlight Object is not assigned on Player.");
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

        if (flashlightObject != null)
        {
            flashlightObject.SetActive(true);
        }

        Debug.Log("Torch picked up. Flashlight unlocked.");
        Debug.Log("Press F to turn ON/OFF torch light");
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;

        if (flashlightObject != null)
        {
            flashlightObject.SetActive(isFlashlightOn);
        }

        Debug.Log(isFlashlightOn ? "Flashlight ON" : "Flashlight OFF");
    }
}