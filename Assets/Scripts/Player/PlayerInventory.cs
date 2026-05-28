using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private bool hasKeycard = false;

    public void AddKeycard()
    {
        hasKeycard = true;
        Debug.Log("Keycard collected.");
    }

    public bool HasKeycard()
    {
        return hasKeycard;
    }
}