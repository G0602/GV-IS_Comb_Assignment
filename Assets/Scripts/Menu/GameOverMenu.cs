using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI;

    public static bool IsGameOver { get; private set; }
    private Button[] menuButtons = new Button[0];
    private TextMeshProUGUI titleLabel;
    private int selectedButtonIndex = -1;

    private void Awake()
    {
        if (GameSceneNames.IsMainMenu(SceneManager.GetActiveScene()))
        {
            enabled = false;
            return;
        }

        if (gameOverUI == null)
        {
            gameOverUI = CreateDefaultGameOverMenu();
        }

        var menuCanvas = MenuInputUtility.GetOrCreateOverlayCanvas("GameOverMenuCanvas");
        gameOverUI.transform.SetParent(menuCanvas.transform, false);

        titleLabel = gameOverUI.GetComponentInChildren<TextMeshProUGUI>(true);
        menuButtons = MenuInputUtility.PrepareButtons(gameOverUI);
        ApplyButtonLabels();
        gameOverUI.SetActive(false);
        IsGameOver = false;
    }

    private void Update()
    {
        if (IsGameOver)
        {
            selectedButtonIndex = MenuInputUtility.HandleKeyboardSelection(menuButtons, selectedButtonIndex);
        }
    }

    public void ShowGameOver()
    {
        ShowGameOver("You Lost");
    }

    public void ShowGameOver(string message)
    {
        var pauseMenu = FindAnyObjectByType<PauseMenu>(FindObjectsInactive.Include);
        if (pauseMenu != null)
        {
            pauseMenu.HideForGameOver();
        }

        if (titleLabel == null && gameOverUI != null)
        {
            titleLabel = gameOverUI.GetComponentInChildren<TextMeshProUGUI>(true);
        }

        if (titleLabel != null)
        {
            titleLabel.text = message;
        }

        IsGameOver = true;
        gameOverUI.SetActive(true);
        MenuInputUtility.ShowCursor();
        selectedButtonIndex = 0;
        MenuInputUtility.SelectFirst(menuButtons);
        Time.timeScale = 0f;
    }

    public static void ShowGameOverScreen()
    {
        ShowGameOverScreen("You Lost");
    }

    public static void ShowGameOverScreen(string message)
    {
        var menu = FindAnyObjectByType<GameOverMenu>(FindObjectsInactive.Include);
        if (menu == null)
        {
            menu = CreateController();
        }

        menu.ShowGameOver(message);
    }

    public void Restart()
    {
        IsGameOver = false;
        selectedButtonIndex = -1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        IsGameOver = false;
        selectedButtonIndex = -1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameSceneNames.MainMenu);
    }

    private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureController(scene);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void RegisterSceneHook()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        SceneManager.sceneLoaded += HandleSceneLoaded;
        EnsureController(SceneManager.GetActiveScene());
    }

    private static void EnsureController(Scene scene)
    {
        if (!GameSceneNames.IsGameplay(scene))
        {
            return;
        }

        if (FindAnyObjectByType<GameOverMenu>(FindObjectsInactive.Include) == null)
        {
            CreateController();
        }
    }

    private static GameOverMenu CreateController()
    {
        var controller = new GameObject("GameOverMenuController");
        return controller.AddComponent<GameOverMenu>();
    }

    private GameObject CreateDefaultGameOverMenu()
    {
        var canvas = GetOrCreateCanvas();
        var panel = CreatePanel(canvas.transform, "GameOverMenuPanel");
        CreateTitle(panel.transform, "Game Lost", 90f);
        CreateButton(panel.transform, "RestartButton", "Restart", 20f, Restart);
        CreateButton(panel.transform, "QuitButton", "Quit", -45f, QuitToMainMenu);

        return panel;
    }

    private static Canvas GetOrCreateCanvas()
    {
        return MenuInputUtility.GetOrCreateOverlayCanvas("GameOverMenuCanvas");
    }

    private static GameObject CreatePanel(Transform parent, string name)
    {
        var panel = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(parent, false);

        var rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        var image = panel.GetComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0.78f);

        return panel;
    }

    private static void CreateTitle(Transform parent, string text, float y)
    {
        var title = new GameObject("Title", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        title.transform.SetParent(parent, false);

        var rectTransform = title.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(0f, y);
        rectTransform.sizeDelta = new Vector2(360f, 80f);

        var label = title.GetComponent<TextMeshProUGUI>();
        label.text = text;
        label.fontSize = 42f;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;
    }

    private static void CreateButton(Transform parent, string name, string label, float y, UnityEngine.Events.UnityAction action)
    {
        var buttonObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        var rectTransform = buttonObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(0f, y);
        rectTransform.sizeDelta = new Vector2(220f, 48f);

        buttonObject.GetComponent<Image>().color = Color.white;
        buttonObject.GetComponent<Button>().onClick.AddListener(action);

        var textObject = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(buttonObject.transform, false);

        var textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        var text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = label;
        text.fontSize = 24f;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(0.12f, 0.12f, 0.12f);
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
            if (buttonName.Contains("restart"))
            {
                label.text = "Restart";
            }
            else if (buttonName.Contains("quit") || buttonName.Contains("exit"))
            {
                label.text = "Quit";
            }
        }
    }
}
