using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;
    private Transform afterDrag;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;
    public Image imageItem;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        afterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    // Вычисляем смещение между курсором и позицией объекта
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            eventData.position, 
            canvas.worldCamera, 
            out offset
        );

        offset = rectTransform.anchoredPosition - offset;
        
        imageItem.raycastTarget = false; // Корректируем смещение
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            eventData.position, 
            canvas.worldCamera, 
            out localPoint
        );

        rectTransform.anchoredPosition = localPoint + offset; // Добавляем смещение
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(afterDrag);
        imageItem.raycastTarget = true;
    }
}
