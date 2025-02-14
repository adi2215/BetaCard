using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{
    public EditableLine editableLine; 
    public float speed = 5f; 
    public Transform arrow; 

    private int currentPairIndex = 0; 
    private float t = 0f; 
    private bool isMoving = false;
    private bool isDragging = false;

    private void Update()
    {
        if (isMoving && !isDragging)
        {
            MoveAlongCurve();
        }

        if (isDragging)
        {
            DragAlongCurve();
        }
    }

    private void MoveAlongCurve()
    {
        if (editableLine.pointPairs.Count == 0) return;

        PointPair pair = editableLine.pointPairs[currentPairIndex];
        t += Time.deltaTime * speed / pair.lineRenderer.positionCount;

        if (t >= 1f)
        {
            t = 0f;
            currentPairIndex++;

            if (currentPairIndex >= editableLine.pointPairs.Count)
            {
                isMoving = false; 
                return;
            }
        }

        transform.position = GetPointOnCurve(pair, t);
        UpdateArrowDirection(pair);
    }

    private void DragAlongCurve()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        PointPair pair = editableLine.pointPairs[currentPairIndex];
        t = GetClosestT(pair, mousePos);
        transform.position = GetPointOnCurve(pair, t);

        UpdateArrowDirection(pair);
    }

    private float GetClosestT(PointPair pair, Vector3 position)
    {
        List<Vector3> curvePoints = GetCurvePoints(pair);
        float closestT = 0f;
        float minDistance = float.MaxValue;

        for (int i = 0; i < curvePoints.Count; i++)
        {
            float t = i / (float)(curvePoints.Count - 1);
            float distance = Vector3.Distance(position, curvePoints[i]);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestT = t;
            }
        }

        return closestT;
    }

    private Vector3 GetPointOnCurve(PointPair pair, float t)
    {
        List<Vector3> curvePoints = GetCurvePoints(pair);
        int pointCount = curvePoints.Count;

        if (pointCount < 2) return transform.position;

        t = Mathf.Clamp01(t);

        int segmentCount = pointCount - 1;
        float segmentProgress = t * segmentCount;
        int segmentIndex = Mathf.FloorToInt(segmentProgress);
        float segmentT = segmentProgress - segmentIndex;

        return Vector3.Lerp(curvePoints[segmentIndex], curvePoints[segmentIndex + 1], segmentT);
    }

    private void UpdateArrowDirection(PointPair pair)
    {
        if (arrow == null) return;

        Vector3 direction = GetDirection(pair, t);
        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, angle - 45);
        }
    }

    private Vector3 GetDirection(PointPair pair, float t)
    {
        List<Vector3> curvePoints = GetCurvePoints(pair);
        int pointCount = curvePoints.Count;

        if (pointCount < 2) return Vector3.zero;

        int segmentCount = pointCount - 1;
        float segmentProgress = t * segmentCount;
        int segmentIndex = Mathf.FloorToInt(segmentProgress);

        if (segmentIndex >= pointCount - 1) return Vector3.zero;

        return (curvePoints[segmentIndex + 1] - curvePoints[segmentIndex]).normalized;
    }

    private List<Vector3> GetCurvePoints(PointPair pair)
    {
        List<Vector3> curvePoints = new List<Vector3>();

        for (int i = 0; i <= editableLine.curveResolution; i++)
        {
            float t = i / (float)editableLine.curveResolution;
            Vector3 point = BezierCurve(
                pair.pointA.position,
                pair.controlPoint1.position,
                pair.controlPoint2.position,
                pair.pointB.position, t);
            curvePoints.Add(point);
        }

        return curvePoints;
    }

    private Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        return (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);
    }

    public void StartMoving()
    {
        isMoving = true;
        isDragging = false;
        t = 0f;
        currentPairIndex = 0;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        isMoving = false;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
