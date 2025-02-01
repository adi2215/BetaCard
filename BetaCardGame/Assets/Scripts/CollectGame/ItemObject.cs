using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isDragging = false;

    public ItemData itemData; 

    private void Start()
    {
        cam = Camera.main;
        GetComponent<SpriteRenderer>().sprite = itemData.itemSprite;
    }

    private void OnMouseDown()
    {
        offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = cam.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0;
            transform.position = newPosition;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Basket"))
        {
            Debug.Log($"🎯 {itemData.itemName} попал в корзину!");
            GameManager.Instance.CheckItem(gameObject.GetComponent<ItemObject>());
        }
    }
}
