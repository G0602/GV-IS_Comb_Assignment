using UnityEngine;

public class mainScript : MonoBehaviour
{
    public static mainScript Instance { get; private set; }

    [Header("Mode")]
    public static bool isDebugMode = false;
    public KeyCode toggleDebugKey = KeyCode.Tab;

    private void Update()
    {
        if (Input.GetKeyDown(toggleDebugKey))
        {
            isDebugMode = !isDebugMode;
            Debug.Log(isDebugMode ? "Debug mode ON" : "Gameplay mode ON");
        }
    }
}
