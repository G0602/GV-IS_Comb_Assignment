using UnityEngine;

public class AlienFreezeTest : MonoBehaviour
{
    private bool isFrozen = false;

    public void SetFrozen(bool frozen)
    {
        if (isFrozen == frozen) return;

        isFrozen = frozen;

        if (isFrozen)
        {
            Debug.Log("Alien frozen by flashlight.");
        }
        else
        {
            Debug.Log("Alien unfrozen.");
        }
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}