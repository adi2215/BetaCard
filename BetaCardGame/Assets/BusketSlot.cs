using UnityEngine;
using UnityEngine.EventSystems;

public class BusketSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        ItemObject item = droppedItem.GetComponent<ItemObject>();
        if (item.itemData.firstLetter == GameManager.Instance.word)
            Destroy(item);
    }
}
