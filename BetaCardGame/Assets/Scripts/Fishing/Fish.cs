using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 vec = new Vector3(0, 0, 0);
    private float leftEdge;
    private string letter;
    public bool fishTook = false;
    public Image letter_Img;
    private Transform _pointFish;

    public void fishDirection(Vector3 _vec) => vec = _vec;

    public void giveLetter(string _letter) => letter = _letter;

    public void giveImageLetter(Sprite _letter) => letter_Img.sprite = _letter;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        if (fishTook)
        {
            transform.position = Vector3.Lerp(transform.position, _pointFish.position, 2f * Time.deltaTime);
        }
        else
        {
            transform.position += vec * speed * Time.deltaTime;

            if (transform.position.x < leftEdge) {
                Destroy(gameObject, 1);
            }
        }
    }

    void OnMouseDown()
    {
        TakeFish takeFish = FindObjectOfType<TakeFish>();
        takeFish.CheckFishName(this, letter);
    }

    public void CorrectFish(GameObject pointFish)
    {
        fishTook = true;
        _pointFish = pointFish.transform;
        Destroy(gameObject, 2f);
    }
}
