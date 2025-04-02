using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatchManager : MonoBehaviour
{
    public static CatchManager Instance;

    [SerializeField] private List<LetterData> availableLetters;
    [SerializeField] private BoxCollider2D spawnArea; // Теперь используем BoxCollider2D
    [SerializeField] private GameObject letterPrefab;
    public TextMeshProUGUI text;

    private string currentTarget;
    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetNewTarget();
        InvokeRepeating(nameof(SpawnLetter), 1f, 1.5f);
    }

    private void SpawnLetter()
    {
        if (availableLetters.Count == 0) return;

        LetterData randomLetter = availableLetters[Random.Range(0, availableLetters.Count)];
        if (randomLetter == null) 
        {
            Debug.LogError("Ошибка: LetterData не задано!");
            return;
        }

        // Генерация случайной позиции на основе BoxCollider2D
        Vector3 spawnPos = GetRandomPositionInBox(spawnArea);

        GameObject newLetter = Instantiate(letterPrefab, spawnPos, Quaternion.identity);
        Letter letterScript = newLetter.GetComponent<Letter>();
        if (letterScript != null)
        {
            letterScript.SetLetterData(randomLetter);
        }
        else
        {
            Debug.LogError("Ошибка: Компонент Letter не найден на префабе!");
        }
        //newLetter.GetComponent<Letter>().SetLetterData(randomLetter);
    }

    private Vector3 GetRandomPositionInBox(BoxCollider2D box)
    {
        Bounds bounds = box.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = bounds.center.y; // Фиксируем Y на верхнем крае спавн-зоны

        return new Vector3(randomX, randomY, 0);
    }

    public void SetNewTarget()
    {
        if (availableLetters.Count == 0) return;

        currentTarget = availableLetters[Random.Range(0, availableLetters.Count)].letter;
        Debug.Log($"Новая цель: {currentTarget}");
    }

    public string GetCurrentTarget()
    {
        return currentTarget;
    }

    public void UpdateScore()
    {
        score++;
        Debug.Log($"Счет: {score}");
        text.text = $"COUNT: {score}";

        if (score % 5 == 0)
        {
            SetNewTarget();
        }
    }
}