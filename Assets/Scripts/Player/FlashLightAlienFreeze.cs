using UnityEngine;
using System.Collections.Generic;

public class FlashlightAlienFreeze : MonoBehaviour
{
    [Header("Flashlight Detection")]
    public Transform flashlightOrigin;
    public Transform aimReference;
    public float freezeRange = 15f;
    [Range(1f, 179f)]
    public float influenceConeAngle = 35f;
    public LayerMask detectionLayers = ~0;
    public bool requireLineOfSight = true;

    private PlayerFlashlightController flashlightController;
    private readonly HashSet<AlienFreezeTest> currentlyFrozenAliens = new HashSet<AlienFreezeTest>();

    void Awake()
    {
        flashlightController = GetComponentInParent<PlayerFlashlightController>();

        if (aimReference == null && Camera.main != null)
        {
            aimReference = Camera.main.transform;
        }
    }

    void Update()
    {
        if (flashlightOrigin == null)
        {
            return;
        }

        if (flashlightController != null &&
            (!flashlightController.HasFlashlight || !flashlightController.IsFlashlightOn))
        {
            UnfreezeCurrentAlien();
            return;
        }

        if (flashlightController == null && !flashlightOrigin.gameObject.activeInHierarchy)
        {
            UnfreezeCurrentAlien();
            return;
        }

        DetectAliensInFlashlightCone();
    }

    void DetectAliensInFlashlightCone()
    {
        Collider[] hits = Physics.OverlapSphere(
            flashlightOrigin.position,
            freezeRange,
            detectionLayers,
            QueryTriggerInteraction.Collide
        );

        HashSet<AlienFreezeTest> aliensInCone = new HashSet<AlienFreezeTest>();

        foreach (Collider hit in hits)
        {
            // Ignore the player itself
            if (hit.transform.root == transform.root)
            {
                continue;
            }

            AlienFreezeTest alien = hit.GetComponentInParent<AlienFreezeTest>();

            if (alien == null)
            {
                continue;
            }

            if (!IsInsideFlashlightCone(alien.transform))
            {
                continue;
            }

            if (requireLineOfSight && !HasLineOfSightTo(hit, alien))
            {
                continue;
            }

            aliensInCone.Add(alien);
        }

        Debug.DrawRay(flashlightOrigin.position, GetAimForward() * freezeRange, Color.yellow);

        foreach (AlienFreezeTest alien in aliensInCone)
        {
            alien.SetFrozen(true);
        }

        foreach (AlienFreezeTest alien in currentlyFrozenAliens)
        {
            if (!aliensInCone.Contains(alien))
            {
                alien.SetFrozen(false);
            }
        }

        currentlyFrozenAliens.Clear();

        foreach (AlienFreezeTest alien in aliensInCone)
        {
            currentlyFrozenAliens.Add(alien);
        }
    }

    bool IsInsideFlashlightCone(Transform target)
    {
        Vector3 directionToTarget = target.position - flashlightOrigin.position;

        if (directionToTarget.sqrMagnitude > freezeRange * freezeRange)
        {
            return false;
        }

        float angleToTarget = Vector3.Angle(GetAimForward(), directionToTarget);
        return angleToTarget <= influenceConeAngle * 0.5f;
    }

    Vector3 GetAimForward()
    {
        if (aimReference != null)
        {
            return aimReference.forward;
        }

        return flashlightOrigin.forward;
    }

    bool HasLineOfSightTo(Collider hitCollider, AlienFreezeTest alien)
    {
        Vector3 rayTarget = hitCollider.bounds.center;
        Vector3 directionToTarget = rayTarget - flashlightOrigin.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget <= 0f)
        {
            return false;
        }

        RaycastHit[] hits = Physics.RaycastAll(
            flashlightOrigin.position,
            directionToTarget.normalized,
            distanceToTarget,
            detectionLayers,
            QueryTriggerInteraction.Collide
        );

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.root == transform.root)
            {
                continue;
            }

            return hit.collider.GetComponentInParent<AlienFreezeTest>() == alien;
        }

        return false;
    }

    void UnfreezeCurrentAlien()
    {
        foreach (AlienFreezeTest alien in currentlyFrozenAliens)
        {
            if (alien != null)
            {
                alien.SetFrozen(false);
            }
        }

        currentlyFrozenAliens.Clear();
    }
}
