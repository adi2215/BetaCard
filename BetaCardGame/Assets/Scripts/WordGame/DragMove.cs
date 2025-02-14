using UnityEngine;

public class DragMove : MonoBehaviour
{
    public EditableLine editableLine;
    public Transform arrow;

    private int currentPairIndex = 0;
    private bool isDragging = false;
    private float t = 0f;

    private TrailBall trailManager;

    void Start()
    {
        trailManager = GetComponent<TrailBall>();
        ResetPosition(); 
    }

    void Update()
    {
        if (isDragging)
        {
            DragAlongCurve();
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        trailManager.StartNewTrail();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        CheckNextPair();
    }

    private void DragAlongCurve()
    {
        if (currentPairIndex >= editableLine.pointPairs.Count) return;

        PointPair pair = editableLine.pointPairs[currentPairIndex];

        if (pair == null) return; 
        if (pair.pointA == null || pair.pointB == null) return;
        if (BezierQulity.GetCurvePoints(pair).Count < 2) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        t = BezierQulity.GetClosestT(pair, mousePos, t);
        transform.position = BezierQulity.GetPointOnCurve(pair, t);

        trailManager.AddPoint(transform.position);

        UpdateArrowDirection(pair);
    }


    private void UpdateArrowDirection(PointPair pair)
    {
        if (arrow == null) return;

        Vector3 direction = BezierQulity.GetDirection(pair, t);
        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, angle - 45);
        }
    }

    private void CheckNextPair()
    {
        if (t >= 1f && currentPairIndex < editableLine.pointPairs.Count - 1)
        {
            currentPairIndex++; 
            t = 0f; 

            PointPair newPair = editableLine.pointPairs[currentPairIndex];
            transform.position = newPair.pointA.position;
            trailManager.StartNewTrail();

            if (arrow != null)
            {
                UpdateArrowDirection(newPair);
            }
        }
        else if (t >= 1f && currentPairIndex == editableLine.pointPairs.Count - 1)
        {
            GameStageManager.Instance.NextStage();
        }
    }


    public void ResetPosition()
    {
        if (editableLine.pointPairs.Count > 0)
        {
            PointPair firstPair = editableLine.pointPairs[0];
            transform.position = firstPair.pointA.position;
            UpdateArrowDirection(firstPair);
        }

        currentPairIndex = 0;
        t = 0f;
    }
}
