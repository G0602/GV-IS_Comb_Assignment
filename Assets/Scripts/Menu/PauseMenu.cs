using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;
    private Button[] menuButtons = new Button[0];
    private int selectedButtonIndex = -1;
    private CursorLockMode previousCursorLockState;
    private bool previousCursorVisible;

    private void Awake()
    {
        if (GameSceneNames.IsMainMenu(SceneManager.GetActiveScene()))
        {
            enabled = false;
            return;
        }

        HideUnusedLegacyPauseMenu();

        if (pauseMenuUI == null)
        {
            pauseMenuUI = MenuInputUtility.FindSceneObjectByName("PauseMenu");
        }

        if (pauseMenuUI == null)
        {
            pauseMenuUI = CreateDefaultPauseMenu();
        }

        menuButtons = MenuInputUtility.PrepareButtons(pauseMenuUI);
        ApplyButtonLabels();
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        if (GameOverMenu.IsGameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (isPaused)
        {
            selectedButtonIndex = MenuInputUtility.HandleKeyboardSelection(menuButtons, selectedButtonIndex);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        selectedButtonIndex = -1;
        Cursor.lockState = previousCursorLockState;
        Cursor.visible = previousCursorVisible;
    }

    public void Pause()
    {
        previousCursorLockState = Cursor.lockState;
        previousCursorVisible = Cursor.visible;
        pauseMenuUI.SetActive(true);
        MenuInputUtility.ShowCursor();
        selectedButtonIndex = 0;
        MenuInputUtility.SelectFirst(menuButtons);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void HideForGameOver()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        isPaused = false;
        selectedButtonIndex = -1;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
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

        if (FindAnyObjectByType<PauseMenu>(FindObjectsInactive.Include) == null)
        {
            var controller = new GameObject("PauseMenuController");
            controller.AddComponent<PauseMenu>();
        }
    }

    private static void HideUnusedLegacyPauseMenu()
    {
        var legacyMenu = GameObject.Find("PauseMenu");
        if (legacyMenu != null && legacyMenu.GetComponent<PauseMenu>() == null)
        {
            legacyMenu.SetActive(false);
        }
    }

    private GameObject CreateDefaultPauseMenu()
    {
        var canvas = GetOrCreateCanvas();
        var panel = CreatePanel(canvas.transform, "PauseMenuPanel");
        CreateTitle(panel.transform, "Paused", 105f);
        CreateButton(panel.transform, "ResumeButton", "Resume", 35f, Resume);
        CreateButton(panel.transform, "RestartButton", "Restart", -30f, Restart);
        CreateButton(panel.transform, "ExitButton", "Exit", -95f, QuitToMainMenu);

        return panel;
    }

    private static Canvas GetOrCreateCanvas()
    {
        var canvas = FindAnyObjectByType<Canvas>(FindObjectsInactive.Include);
        if (canvas != null)
        {
            MenuInputUtility.EnsureGraphicRaycaster(canvas);
            MenuInputUtility.EnsureEventSystem();
            return canvas;
        }

        var canvasObject = new GameObject("Canvas");
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        MenuInputUtility.EnsureGraphicRaycaster(canvas);
        MenuInputUtility.EnsureEventSystem();
        return canvas;
    }

    private static void EnsureEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include) != null)
        {
            return;
        }

        var eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
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
        image.color = new Color(0f, 0f, 0f, 0.72f);

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
        rectTransform.sizeDelta = new Vector2(320f, 70f);

        var label = title.GetComponent<TextMeshProUGUI>();
        label.text = text;
        label.fontSize = 40f;
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
            if (buttonName.Contains("continue") || buttonName.Contains("resume"))
            {
                label.text = "Resume";
            }
            else if (buttonName.Contains("restart"))
            {
                label.text = "Restart";
            }
            else if (buttonName.Contains("exit") || buttonName.Contains("quit"))
            {
                label.text = "Exit";
            }
        }
    }
}
