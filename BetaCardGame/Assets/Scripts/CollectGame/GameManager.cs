using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform basket;
    public List<ItemData> allItems;
    public char word;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckItem(ItemObject item)
    {
        float distance = Vector3.Distance(item.transform.position, basket.position);

        if (word == item.itemData.firstLetter)
        {
            Debug.Log($"{item.itemData.itemName} начинается с буквы 'C'");
            Destroy(item.gameObject);
        }
        else
        {
            Debug.Log($"{item.itemData.itemName} не начинается с 'С'");
        }
    }
}
