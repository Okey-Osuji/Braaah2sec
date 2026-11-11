using UnityEngine;
using UnityEngine.EventSystems;

public class DesktopWindowDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private RectTransform window;
    private Canvas canvas;

    private void Awake()
    {
        window = transform.parent.GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        window.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        window.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}