using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelectableButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectSelf();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectSelf();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSelf();
    }

    private void SelectSelf()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (button == null || !button.interactable)
        {
            return;
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }
}