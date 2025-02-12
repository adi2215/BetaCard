using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static GameManage Instance;
    public GameObject prefabButton;
    public GameObject prefabIcon;
    public Transform panel;
    public List<ItemData> allItems;

    private ItemData randomItem;

    void Start() => GenerateQuiz();

    private void GenerateQuiz()
    {
        allItems.Shuffle();

        foreach (ItemData item in allItems)
        {
            GameObject newCard = Instantiate(prefabButton, transform);
            CheckWord itemCard = newCard.GetComponentInChildren<CheckWord>();
            itemCard.Setup(item);
        }

        randomItem = allItems[Random.Range(0, allItems.Count)];

        GameObject newIcon = Instantiate(prefabIcon, panel);
        Image icon = newIcon.GetComponent<Image>();

        icon.sprite = randomItem.itemSprite;
    }

    public void Checking(ItemData item)
    {
        if (item.itemName == randomItem.itemName)
            Debug.Log("You Win");
        else
            Debug.Log("Try Again");
    }
}
