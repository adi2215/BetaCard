using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiddleManager : MonoBehaviour
{
    public List<ItemData> allItems; 
    public Transform spawnPoint; 
    public GameObject prefabItem;
    public RiddleSlot[] slots;

    private ItemData currentItem; 
    private ItemData wrongItem;

    void Start()
    {
        GenerateRiddle();
    }

    void GenerateRiddle()
    {
        currentItem = allItems[Random.Range(0, allItems.Count)];
        GameObject Item = Instantiate(prefabItem, spawnPoint.position, Quaternion.identity, spawnPoint);
        ItemObject correctItem = Item.GetComponent<ItemObject>();
        correctItem.Setup(currentItem);

        do
        {
            wrongItem = allItems[Random.Range(0, allItems.Count)];
        } 
        while (wrongItem.itemName == currentItem.itemName);

        if (Random.value > 0.5f)
        {
            slots[0].SetSlot(currentItem);
            slots[1].SetSlot(wrongItem);
        }
        else
        {
            slots[0].SetSlot(wrongItem);
            slots[1].SetSlot(currentItem);
        }
    }

    
}
