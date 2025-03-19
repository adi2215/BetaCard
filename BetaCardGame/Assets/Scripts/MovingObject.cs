using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}