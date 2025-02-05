using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    public List<Transform> points = new List<Transform>(); // Точки кривой
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        UpdateLine();
    }

    public void UpdateLine()
    {
        if (points.Count < 2) return;

        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }

    public void AddPoint(Vector3 position)
    {
        GameObject newPoint = new GameObject("Point");
        newPoint.transform.position = position;
        newPoint.transform.parent = transform;
        points.Add(newPoint.transform);
        UpdateLine();
    }
}
