using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform basket;
    public List<ItemData> allItems;
    public List<ItemData> validItems; 

    private void Awake()
    {
        Instance = this;
    }

    public void CheckItem(ItemObject item)
    {
        float distance = Vector3.Distance(item.transform.position, basket.position);

        if (distance < 1.5f)
        {
            if (validItems.Contains(item.itemData))
            {
                Debug.Log($"{item.itemData.itemName} начинается с буквы 'М'");
                Destroy(item.gameObject);
            }
            else
            {
                Debug.Log($"{item.itemData.itemName} не начинается с 'М'");
                item.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
            }
        }
    }
}
