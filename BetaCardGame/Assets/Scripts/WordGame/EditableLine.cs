using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointPair
{
    public Transform pointA;
    public Transform pointB;
    public Transform controlPoint1;
    public Transform controlPoint2;
    public LineRenderer lineRenderer;
}

public class EditableLine : MonoBehaviour
{
    public List<PointPair> pointPairs = new List<PointPair>();
    public GameObject linePrefab;
    public int curveResolution = 20;

    public PathData pathData;

    void Start()
    {
        if (pathData != null)
        {
            pathData.LoadPath(this);
        }
    }

    public void AddPointPair(Vector3 posA, Vector3 posB)
    {
        GameObject pointA = CreatePoints(posA);
        GameObject pointB = CreatePoints(posB);

        GameObject ctrl1 = CreatePoints(Vector3.Lerp(posA, posB, 0.3f));
        GameObject ctrl2 = CreatePoints(Vector3.Lerp(posA, posB, 0.7f));

        LineRenderer newLine = new GameObject("LineRenderer").AddComponent<LineRenderer>();
        newLine.transform.SetParent(transform);
        SetupLineRenderer(newLine);

        PointPair pair = new PointPair
        {
            pointA = pointA.transform,
            pointB = pointB.transform,
            controlPoint1 = ctrl1.transform,
            controlPoint2 = ctrl2.transform,
            lineRenderer = newLine
        };

        pointPairs.Add(pair);
        UpdateLine(pair);
    }

    private GameObject CreatePoints(Vector3 position)
    {
        GameObject point = new GameObject("Point");
        point.transform.position = position;
        point.transform.SetParent(transform);

        CircleCollider2D collider = point.AddComponent<CircleCollider2D>();
        collider.radius = 0.2f;

        point.AddComponent<DraggablePoint>();
        return point;
    }

    private void SetupLineRenderer(LineRenderer lr)
    {
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.positionCount = 0;
        lr.useWorldSpace = true;
    }

    public void UpdateLine(PointPair pair)
    {
        List<Vector3> curvePoints = new List<Vector3>();

        for (int i = 0; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            Vector3 point = BezierQulity.BezierCurve(
                pair.pointA.position,
                pair.controlPoint1.position,
                pair.controlPoint2.position,
                pair.pointB.position, t);
            curvePoints.Add(point);
        }

        pair.lineRenderer.positionCount = curvePoints.Count;
        pair.lineRenderer.SetPositions(curvePoints.ToArray());
    }

    private void Update()
    {
        foreach (var pair in pointPairs)
        {
            UpdateLine(pair);
        }
    }

    public void SetLineVisibility(bool visible)
    {
        foreach (var pair in pointPairs)
        {
            pair.lineRenderer.enabled = visible;
        }
    }



    public void ClearPairs()
    {
        foreach (var pair in pointPairs)
        {
            if (pair.lineRenderer != null)
            {
                Destroy(pair.lineRenderer.gameObject);
            }
        }
        pointPairs.Clear();
    }

    public void AddPair(Vector3 pointA, Vector3 pointB, Vector3 controlPoint1, Vector3 controlPoint2)
    {
        GameObject pointAObj = CreatePoint("PointA", pointA);
        GameObject pointBObj = CreatePoint("PointB", pointB);
        GameObject cp1Obj = CreatePoint("ControlPoint1", controlPoint1);
        GameObject cp2Obj = CreatePoint("ControlPoint2", controlPoint2);

        LineRenderer lineRenderer = new GameObject("LineRenderer").AddComponent<LineRenderer>();
        lineRenderer.positionCount = curveResolution;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        PointPair newPair = new PointPair
        {
            pointA = pointAObj.transform,
            pointB = pointBObj.transform,
            controlPoint1 = cp1Obj.transform,
            controlPoint2 = cp2Obj.transform,
            lineRenderer = lineRenderer
        };

        pointPairs.Add(newPair);
        UpdateLineRenderer(newPair);
    }
    
    private GameObject CreatePoint(string name, Vector3 position)
    {
        GameObject point = new GameObject(name);
        point.transform.position = position;

        CircleCollider2D collider = point.AddComponent<CircleCollider2D>();
        collider.radius = 0.2f;

        point.AddComponent<DraggablePoint>();

        return point;
    }

    private void UpdateLineRenderer(PointPair pair)
    {
        Vector3[] points = new Vector3[curveResolution + 1];
        for (int i = 0; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            points[i] = BezierQulity.BezierCurve(
                pair.pointA.position,
                pair.controlPoint1.position,
                pair.controlPoint2.position,
                pair.pointB.position, t);
        }
        pair.lineRenderer.positionCount = points.Length;
        pair.lineRenderer.SetPositions(points);
    }

}
