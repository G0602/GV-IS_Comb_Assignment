using UnityEngine;

public class FlashlightAlienFreeze : MonoBehaviour
{
    [Header("Flashlight Detection")]
    public Transform flashlightOrigin;
    public float freezeRange = 15f;

    private AlienFreezeTest currentlyFrozenAlien;

    void Update()
    {
        if (flashlightOrigin == null)
        {
            return;
        }

        // If flashlight object is disabled/off, unfreeze alien
        if (!flashlightOrigin.gameObject.activeInHierarchy)
        {
            UnfreezeCurrentAlien();
            return;
        }

        DetectAlienInFlashlight();
    }

    void DetectAlienInFlashlight()
    {
        Ray ray = new Ray(flashlightOrigin.position, flashlightOrigin.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, freezeRange);

        AlienFreezeTest detectedAlien = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            // Ignore the player itself
            if (hit.transform.root == transform.root)
            {
                continue;
            }

            if (hit.collider.CompareTag("Alien"))
            {
                AlienFreezeTest alien = hit.collider.GetComponent<AlienFreezeTest>();

                if (alien != null && hit.distance < closestDistance)
                {
                    detectedAlien = alien;
                    closestDistance = hit.distance;
                }
            }
        }

        Debug.DrawRay(flashlightOrigin.position, flashlightOrigin.forward * freezeRange, Color.yellow);

        if (detectedAlien != null)
        {
            if (currentlyFrozenAlien != null && currentlyFrozenAlien != detectedAlien)
            {
                currentlyFrozenAlien.SetFrozen(false);
            }

            currentlyFrozenAlien = detectedAlien;
            currentlyFrozenAlien.SetFrozen(true);
        }
        else
        {
            UnfreezeCurrentAlien();
        }
    }

    void UnfreezeCurrentAlien()
    {
        if (currentlyFrozenAlien != null)
        {
            currentlyFrozenAlien.SetFrozen(false);
            currentlyFrozenAlien = null;
        }
    }
}