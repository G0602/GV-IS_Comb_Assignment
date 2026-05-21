using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MenuInputUtility
{
    public static Button[] PrepareButtons(GameObject menuRoot)
    {
        if (menuRoot == null)
        {
            return new Button[0];
        }

        Button[] buttons = menuRoot.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation navigation = buttons[i].navigation;
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnUp = buttons[(i - 1 + buttons.Length) % buttons.Length];
            navigation.selectOnDown = buttons[(i + 1) % buttons.Length];
            navigation.selectOnLeft = buttons[(i - 1 + buttons.Length) % buttons.Length];
            navigation.selectOnRight = buttons[(i + 1) % buttons.Length];
            buttons[i].navigation = navigation;
        }

        return buttons;
    }

    public static void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void SelectFirst(Button[] buttons)
    {
        SelectButton(buttons, 0);
    }

    public static int HandleKeyboardSelection(Button[] buttons, int selectedIndex)
    {
        if (buttons == null || buttons.Length == 0)
        {
            return -1;
        }

        if (selectedIndex < 0 || selectedIndex >= buttons.Length)
        {
            selectedIndex = 0;
            SelectButton(buttons, selectedIndex);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex = WrapIndex(selectedIndex - 1, buttons.Length);
            SelectButton(buttons, selectedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex = WrapIndex(selectedIndex + 1, buttons.Length);
            SelectButton(buttons, selectedIndex);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            buttons[selectedIndex].onClick.Invoke();
        }

        return selectedIndex;
    }

    private static int WrapIndex(int index, int length)
    {
        return (index + length) % length;
    }

    private static void SelectButton(Button[] buttons, int index)
    {
        if (buttons == null || buttons.Length == 0 || index < 0 || index >= buttons.Length)
        {
            return;
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        }
    }
}
