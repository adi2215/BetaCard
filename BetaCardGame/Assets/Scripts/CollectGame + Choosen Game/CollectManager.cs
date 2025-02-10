using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CollectManager : MonoBehaviour
{
    public static CollectManager Instance;
    public GameObject prefab;
    public List<WordData> allWords;
    private List<string> correctWords = new List<string> {};
    public int wordCount;

    public WordSlot[] slots;

    private void Awake()
    {
        Instance = this;
    }

    private void Start() => GenerateQuiz();

    private void GenerateQuiz()
    {
        List<ItemData> options = new List<ItemData>();

        if (allWords.Count < wordCount)
        {
            Debug.LogError("Недостаточно слов в allWords для выбора " + wordCount + " уникальных!");
            return;
        }

        List<WordData> selectedWords = new List<WordData>();

        int i = 0;
        while (i < wordCount)
        {
            WordData randomWord = allWords[Random.Range(0, allWords.Count)];
            if (!selectedWords.Contains(randomWord))
            {
                selectedWords.Add(randomWord);
                correctWords.Add(randomWord.WordName);
                i++;
            }
        }

        foreach (var word in selectedWords)
        {
            options.AddRange(word.words); 
        }

        options.Shuffle();

        foreach (var item in options)
        {
            CreateItemObject(item);
        }
    }

    private void CreateItemObject(ItemData item)
    {
        GameObject newCard = Instantiate(prefab, transform);
        ItemObject itemCard = newCard.GetComponentInChildren<ItemObject>(); 

        if (itemCard != null)
        {
            itemCard.Setup(item); 
        }
        else
        {
            Debug.LogError("ItemObject не найден в префабе!");
        }
    }

    public void CheckSlots()
    {
        if (slots[0].item != null && slots[1].item != null)
        {
            string combinedWord = slots[0].item.itemData.itemName + slots[1].item.itemData.itemName;

            if (correctWords.Contains(combinedWord)) 
            {
                Debug.Log("Правильное слово: " + combinedWord);
                foreach(var slot in slots)
                {
                    Destroy(slot.item.gameObject);
                    slot.item = null;
                }
            }
            else
            {
                Debug.Log("Неправильное слово! Возвращаем на место.");
                slots[0].ResetSlot();
                slots[1].ResetSlot();
            }
        }
    }
}
