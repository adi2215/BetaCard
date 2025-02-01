using UnityEngine;

public class Letter : MonoBehaviour
{
    private char letterValue;
    public SpriteRenderer spriteRenderer;
    private float fallSpeed = 10f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    public void SetLetterData(LetterData data)
    {
        if (data == null)
        {
            Debug.LogError("Ошибка: LetterData пустое!");
            return;
        }

        letterValue = data.letter;

        if (spriteRenderer == null)
        {
            Debug.LogError("Ошибка: SpriteRenderer не найден!");
            return;
        }

        spriteRenderer.sprite = data.letterSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) 
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player")) 
        {
            collision.GetComponent<PlayerCatcher>().CatchLetter(letterValue);
            Destroy(gameObject);
        }
    }
}
