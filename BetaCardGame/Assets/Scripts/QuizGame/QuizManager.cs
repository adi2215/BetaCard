using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public List<LetterData> lettersData; 
    public Transform cardsParent; 
    public GameObject cardPrefab; 
    public Button playSoundButton; 
    public TextMeshProUGUI resultText; 

    private char correctLetter;
    private AudioClip correctSound;
    public AudioSource audio;
    private List<ItemCard> generatedCards = new List<ItemCard>();
    private int attempts = 0;

    private void Start()
    {
        GenerateQuiz();
        playSoundButton.onClick.AddListener(PlayCorrectSound);
    }

    private void GenerateQuiz()
    {
        ClearOldCards();

        LetterData correctData = lettersData[Random.Range(0, lettersData.Count)];
        correctLetter = correctData.letter;
        correctSound = correctData.sound;

        List<LetterData> options = new List<LetterData> { correctData };

        int i = 0;
        while(i < lettersData.Count - 1)
        {
            LetterData randomLetter = lettersData[Random.Range(0, lettersData.Count)];
            if (!options.Contains(randomLetter))
            {
                options.Add(randomLetter);
                i++;
            }
        }

        options.Shuffle();

        foreach (LetterData letter in options)
        {
            GameObject newCard = Instantiate(cardPrefab, cardsParent);
            ItemCard itemCard = newCard.GetComponent<ItemCard>();
            itemCard.Setup(letter, this);
            generatedCards.Add(itemCard);
        }

        resultText.text = "Выбери правильную букву!";
        attempts = 0;
    }

    private void ClearOldCards()
    {
        foreach (var card in generatedCards)
        {
            Destroy(card.gameObject);
        }
        generatedCards.Clear();
    }

    public void PlayCorrectSound()
    {
        audio.PlayOneShot(correctSound);
    }

    public void CheckAnswer(char chosenLetter)
    {
        if (chosenLetter == correctLetter)
        {
            resultText.text = "You Win!";
            Invoke(nameof(GenerateQuiz), 2f);
        }
        else
        {
            resultText.text = "Попробуй ещё раз!";
            attempts++;

            if (attempts >= 2)
            {
                HighlightCorrectLetter();
            }
        }
    }

    private void HighlightCorrectLetter()
    {
        foreach (var card in generatedCards)
        {
            if (card.data.letter == correctLetter)
            {
                card.Highlight(true);
            }
        }
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
