using UnityEngine;

public class GoalDoorInteractable : MonoBehaviour
{
    [Header("Door Part")]
    public Transform doorPart;

    [Header("Door Rotation")]
    public Vector3 closedRotation = new Vector3(0f, 0f, 0f);
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);

    [Header("Settings")]
    public float openSpeed = 5f;

    [Header("Lock Settings")]
    public bool requiresKeycard = false;
    public string lockedMessage = "Need keycard";

    [Header("UI")]
    private InteractionPromptUI interactionPromptUI;
    public string promptMessage = "Press E to open/close door";

    [Header("Win Condition")]
    public bool goal = false;

    private bool isOpen = false;
    private bool playerNearby = false;
    private PlayerInventory playerInventory;

    //audio
    public AudioSource audioSource;
	public AudioClip openDoor,closeDoor;

    void Start () {
        interactionPromptUI = FindAnyObjectByType<InteractionPromptUI>();
		audioSource = GetComponent<AudioSource> ();
	}

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            TryToggleDoor();
        }

        RotateDoor();
    }

    void TryToggleDoor()
    {
        if (requiresKeycard)
        {
            if (playerInventory == null || !playerInventory.HasKeycard())
            {
                Debug.Log("Door locked. Need keycard.");

                if (interactionPromptUI != null)
                {
                    interactionPromptUI.ShowPrompt(lockedMessage);
                }

                return;
            }
        }

        ToggleDoor();

        if (goal && isOpen)
        {
            Debug.Log("Goal reached! You win!");
            if (interactionPromptUI != null)
             {
                 interactionPromptUI.hidePrompt();
             }
            GameOverMenu.ShowGameOverScreen("You Won");
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
            Debug.Log("Door opened");
        else
            Debug.Log("Door closed");

        audioSource.clip = isOpen?openDoor:closeDoor;
		audioSource.Play ();
    }

    void RotateDoor()
    {
        if (doorPart == null) return;

        Quaternion targetRotation = isOpen
            ? Quaternion.Euler(openRotation)
            : Quaternion.Euler(closedRotation);

        doorPart.localRotation = Quaternion.Slerp(
            doorPart.localRotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            playerInventory = other.GetComponentInParent<PlayerInventory>();

            if (interactionPromptUI != null)
            {
                interactionPromptUI.ShowPrompt(promptMessage);
            }

            Debug.Log("Press E to open/close door");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            playerInventory = null;

            if (interactionPromptUI != null)
            {
                interactionPromptUI.HidePrompt();
            }

            Debug.Log("Player left door area");
        }
    }
}
