using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;
    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Transform afterDrag;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;
    public Image imageItem;
    public TextMeshProUGUI _text;
    public AudioSource audioSound;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(ItemData data)
    {
        this.itemData = data;
        originalParent = transform.parent;
        
        imageItem.sprite = itemData.itemSprite;

        if (_text != null)
        {
            _text.text = itemData.itemName;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        afterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        /*RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            eventData.position, 
            canvas.worldCamera, 
            out offset
        );

        offset = rectTransform.anchoredPosition - offset;*/
        
        imageItem.raycastTarget = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        /*Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            eventData.position, 
            canvas.worldCamera, 
            out localPoint
        );*/

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(afterDrag);
        imageItem.raycastTarget = true;
    }

    public void PlaySound() { audioSound.clip = itemData.sound;  audioSound.Play(); }
}
