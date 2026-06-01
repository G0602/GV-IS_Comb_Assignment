using UnityEngine;

public class debugToggle : MonoBehaviour
{
    public static debugToggle Instance { get; private set; }

    [Header("Debug Mode")]
    [SerializeField] private bool debugMode;

    public static bool IsDebugMode => Instance != null && Instance.debugMode;

    public KeyCode toggleDebugKey = KeyCode.Tab;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleDebugKey))
        {
            debugMode = !debugMode;
            Debug.Log(debugMode ? "Debug mode ON" : "Debug mode OFF");
        }
    }
}