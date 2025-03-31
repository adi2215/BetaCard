using TMPro;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    private ItemData letter;
    public TextMeshProUGUI letter_Img;

    private bool moveObj = true;

    public void giveLetter(ItemData _letter) { letter = _letter; letter_Img.text = _letter.itemName; }

    void Update()
    {
        if (moveObj)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        
            if (transform.position.x < -10f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && moveObj)
        {
            Debug.Log("Объект коснулся героя!");

            CircleCollider2D collider = gameObject.GetComponent<CircleCollider2D>();
            collider.enabled = false;

            CollectorManager player = collision.GetComponent<CollectorManager>();
            if (player != null)
            {
                moveObj = false;
                player.CatchLetter(this, letter);
            }
        }
    }
}