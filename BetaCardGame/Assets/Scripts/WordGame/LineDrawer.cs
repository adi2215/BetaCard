using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public EditableLine line; 
    private Vector3? lastPoint = null; 

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (lastPoint == null)
            {
                lastPoint = mousePos; 
            }
            else
            {
                line.AddPointPair(lastPoint.Value, mousePos); 
                lastPoint = null; 
            }
        }
    }
}
