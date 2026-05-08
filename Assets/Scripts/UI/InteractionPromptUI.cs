using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    public TextMeshProUGUI promptText;

    public void ShowPrompt(string message)
    {
        if (promptText == null) return;

        promptText.text = message;
        promptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        if (promptText == null) return;

        promptText.gameObject.SetActive(false);
    }
}