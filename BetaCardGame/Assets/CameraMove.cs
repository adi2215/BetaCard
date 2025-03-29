using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float dragSpeed = 2f; 
    private Vector3 lastMousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0)) 
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.position -= new Vector3(delta.x * dragSpeed * Time.deltaTime, 0, 0);
            lastMousePos = Input.mousePosition;
        }
    }
}
