using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    private ItemData letter;
    public SpriteRenderer letter_Img;

    public void giveLetter(ItemData _letter) => letter = _letter;

    public void giveImageLetter(Sprite _letter) => letter_Img.sprite = _letter;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Объект коснулся героя!");

            CollectorManager player = collision.GetComponent<CollectorManager>();
            if (player != null)
            {
                player.CatchLetter(letter);
            }

            Destroy(gameObject, 0.1f);
        }
    }
}