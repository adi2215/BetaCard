using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCard : MonoBehaviour
{
    public LetterData data;
    public Button button;
    private QuizManager quizManager;
    [SerializeField] private Image img;
    [SerializeField] private Image imgHit;

    public void Setup(LetterData data, QuizManager manager)
    {
        this.data = data;
        
        img.sprite = data.letterSprite;

        quizManager = manager;
    }

    public void OnClick()
    {
        quizManager.CheckAnswer(data.letter);
    }

    public void Highlight(bool state)
    {
        imgHit.color = state ? Color.yellow : Color.white;
    }
}
