using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;

    private Button[] menuButtons = new Button[0];
    private int selectedButtonIndex = -1;

    private void Awake()
    {
        Time.timeScale = 1f;
        MenuInputUtility.ShowCursor();

        if (mainMenuUI == null)
        {
            mainMenuUI = MenuInputUtility.FindSceneObjectByName("MainMenu");
        }

        if (mainMenuUI == null)
        {
            mainMenuUI = MenuInputUtility.FindSceneObjectByName("MainMenue");
        }

        if (mainMenuUI == null)
        {
            Canvas canvas = FindAnyObjectByType<Canvas>(FindObjectsInactive.Include);
            mainMenuUI = canvas != null ? canvas.gameObject : gameObject;
        }

        Canvas menuCanvas = mainMenuUI.GetComponentInParent<Canvas>();
        MenuInputUtility.EnsureGraphicRaycaster(menuCanvas);

        menuButtons = MenuInputUtility.PrepareButtons(mainMenuUI);
        ApplyButtonLabels();
        selectedButtonIndex = 0;
        MenuInputUtility.SelectFirst(menuButtons);
    }

    private void Update()
    {
        selectedButtonIndex = MenuInputUtility.HandleKeyboardSelection(menuButtons, selectedButtonIndex);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameSceneNames.Gameplay);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    private void ApplyButtonLabels()
    {
        foreach (Button button in menuButtons)
        {
            TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>(true);
            if (label == null)
            {
                continue;
            }

            string buttonName = button.gameObject.name.ToLowerInvariant();
            if (buttonName.Contains("play") || buttonName.Contains("start"))
            {
                label.text = "Start";
            }
            else if (buttonName.Contains("exit") || buttonName.Contains("quit"))
            {
                label.text = "Exit";
            }
        }
    }
}
