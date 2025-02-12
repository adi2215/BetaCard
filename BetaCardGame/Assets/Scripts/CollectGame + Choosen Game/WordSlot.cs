using UnityEngine;
using UnityEngine.EventSystems;

public class WordSlot : MonoBehaviour, IDropHandler
{
    public ItemObject item;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            Debug.Log("fef");
            GameObject droppedItem = eventData.pointerDrag;
            item = droppedItem.GetComponent<ItemObject>();
            item.afterDrag = transform;

            CollectManager.Instance.CheckSlots();
        }
    }

    public void ResetSlot()
    {
        if (item != null)
        {
            Debug.Log("Сegeт!");
            item.transform.SetParent(item.originalParent);
            item.afterDrag = item.originalParent;
            item = null;
        }
    }
}
