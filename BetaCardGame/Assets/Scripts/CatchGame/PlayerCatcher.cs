using UnityEngine;

public class PlayerCatcher : MonoBehaviour
{
    private Vector3 offset;
    private float minX, maxX;

    private void Start()
    {
        float halfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        
        minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + halfWidth;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - halfWidth;
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        newPosition.y = transform.position.y; 
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX); 
        transform.position = newPosition;
    }

    public void CatchLetter(string caughtLetter)
    {
        if (caughtLetter == CatchManager.Instance.GetCurrentTarget())
        {
            CatchManager.Instance.UpdateScore();
        }
    }
}
