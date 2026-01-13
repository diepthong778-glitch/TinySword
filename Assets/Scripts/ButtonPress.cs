using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonPressEffect : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public RectTransform textTransform;
    public Vector2 pressedOffset = new Vector2(0, -5);

    private Vector2 originalPos;

    void Awake()
    {
        originalPos = textTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        textTransform.anchoredPosition = originalPos + pressedOffset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetPosition();
    }

    void ResetPosition()
    {
        textTransform.anchoredPosition = originalPos;
    }
}
