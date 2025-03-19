using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RiddleSlot : MonoBehaviour, IDropHandler
{
    private ItemData _item; 
    public TextMeshProUGUI _text;

    public void SetSlot(ItemData item)
    {
        _item = item;
        _text.text = item.itemName.ToUpper();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        ItemObject itemDrop = droppedItem.GetComponent<ItemObject>();
        if (itemDrop.itemData.itemName == _item.itemName)
        {
            Destroy(droppedItem);
            Debug.Log("You win");
        }
    }
}
