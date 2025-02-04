using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject prefab;
    public List<ItemData> allItems;
    public char word;
    public int countItems;
    private int countCorrectItems;

    private void Awake()
    {
        Instance = this;
    }

    private void Start() => GenerateQuiz();

    private void GenerateQuiz()
    {
        countCorrectItems = 0;
        List<ItemData> options = new List<ItemData> {};

        int i = 0;
        while(i < allItems.Count - countItems)
        {
            ItemData randomLetter = allItems[Random.Range(0, allItems.Count)];
            if (!options.Contains(randomLetter))
            {
                options.Add(randomLetter);
                i++;
            }
        }

        options.Shuffle();
        countItems = 0;

        foreach (ItemData item in options)
        {
            GameObject newCard = Instantiate(prefab, transform);
            ItemObject itemCard = newCard.GetComponentInChildren<ItemObject>();
            itemCard.Setup(item);

            if (itemCard.itemData.firstLetter == word)
                countCorrectItems++;
        }
    }

    public void CollectItem(GameObject obj)
    {
        Destroy(obj);
        countItems++;

        if (countItems == countCorrectItems)
            Debug.Log("You win");
    }
}
