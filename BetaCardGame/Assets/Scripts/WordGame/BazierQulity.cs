using System.Collections.Generic;
using UnityEngine;

public static class BezierQulity
{
    public static Vector3 GetPointOnCurve(PointPair pair, float t)
    {
        List<Vector3> curvePoints = GetCurvePoints(pair);
        int pointCount = curvePoints.Count;

        if (pointCount < 2) return Vector3.zero; 

        t = Mathf.Clamp01(t); 

        int segmentCount = pointCount - 1;
        float segmentProgress = t * segmentCount;
        int segmentIndex = Mathf.FloorToInt(segmentProgress);

        if (segmentIndex >= pointCount - 1) 
            return curvePoints[pointCount - 1];

        return Vector3.Lerp(curvePoints[segmentIndex], curvePoints[segmentIndex + 1], segmentProgress - segmentIndex);
    }

    public static Vector3 GetDirection(PointPair pair, float t)
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

    public static float GetClosestT(PointPair pair, Vector3 position, float currentT)
    {
        List<Vector3> curvePoints = GetCurvePoints(pair);
        float closestT = currentT;
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

        return Mathf.Max(currentT, closestT); 
    }

    public static List<Vector3> GetCurvePoints(PointPair pair)
    {
        List<Vector3> curvePoints = new List<Vector3>();
        int resolution = 20; 

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = BezierCurve(
                pair.pointA.position,
                pair.controlPoint1.position,
                pair.controlPoint2.position,
                pair.pointB.position, t);
            curvePoints.Add(point);
        }

        return curvePoints;
    }

    public static Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        return (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);
    }
}
