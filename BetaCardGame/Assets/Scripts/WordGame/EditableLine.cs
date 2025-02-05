using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EditableLine : MonoBehaviour
{
    public List<Transform> points = new List<Transform>(); // Основные точки
    public List<List<Transform>> controlPoints = new List<List<Transform>>(); // Список контрольных точек для каждой пары

    private LineRenderer lineRenderer;
    public int curveResolution = 20; // Количество сегментов на один изгиб
    public int controlPointsPerSegment = 2; // Количество контрольных точек на сегмент (2 точки)

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        UpdateLine();
    }

    void Update()
    {
        UpdateLine(); // Обновляем линию каждый кадр
    }

    public void AddPoint(Vector3 position)
    {
        GameObject newPoint = new GameObject("Point");
        newPoint.transform.position = position;
        newPoint.transform.parent = transform;
        newPoint.AddComponent<DraggablePoint>(); // Делаем точку редактируемой!

        points.Add(newPoint.transform);

        if (points.Count > 1) // Если есть хотя бы 2 точки
        {
            // Убираем контрольные точки с предыдущей пары точек, если они были
            if (controlPoints.Count >= points.Count - 1)
            {
                controlPoints.RemoveAt(controlPoints.Count - 1); 
            }

            AddControlPointsBetween(points[points.Count - 2], newPoint.transform);
        }

        UpdateLine();
    }

    private void AddControlPointsBetween(Transform start, Transform end)
    {
        List<Transform> controlPointsList = new List<Transform>();

        // Создаем только две контрольные точки между start и end
        for (int i = 1; i <= controlPointsPerSegment; i++)
        {
            float t = i / (float)(controlPointsPerSegment + 1); // Равномерное распределение контрольных точек

            Vector3 controlPointPosition = Vector3.Lerp(start.position, end.position, t); // Линейное распределение точек
            GameObject controlPoint = new GameObject("ControlPoint");
            controlPoint.transform.position = controlPointPosition;
            controlPoint.transform.parent = transform;
            controlPoint.AddComponent<DraggablePoint>(); // Можно двигать

            controlPointsList.Add(controlPoint.transform);
        }

        controlPoints.Add(controlPointsList);
    }

    public void UpdateLine()
    {
        if (points.Count < 2) return;

        List<Vector3> curvedPoints = GenerateCurve(); // Генерируем плавную кривую

        lineRenderer.positionCount = curvedPoints.Count;
        lineRenderer.SetPositions(curvedPoints.ToArray());
    }

    public List<Vector3> GenerateCurve()
    {
        List<Vector3> curvePoints = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = points[i].position;
            Vector3 p3 = points[i + 1].position;

            // Используем две контрольные точки для кривой
            List<Transform> controlPointList = controlPoints[i];
            Vector3 p1 = controlPointList[0].position;
            Vector3 p2 = controlPointList[1].position;

            for (int j = 0; j <= curveResolution; j++)
            {
                float t = j / (float)curveResolution;
                Vector3 curvedPoint = BezierCurve(p0, p1, p2, p3, t);
                curvePoints.Add(curvedPoint);
            }
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
}
