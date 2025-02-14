using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPathData", menuName = "Path/PathData")]
public class PathData : ScriptableObject
{
    [System.Serializable]
    public class PointPairData
    {
        public Vector3 pointA;
        public Vector3 pointB;
        public Vector3 controlPoint1;
        public Vector3 controlPoint2;
    }

    public List<PointPairData> pointPairs = new List<PointPairData>();

    public void SavePath(EditableLine line)
    {
        pointPairs.Clear();
        foreach (var pair in line.pointPairs)
        {
            pointPairs.Add(new PointPairData
            {
                pointA = pair.pointA.position,
                pointB = pair.pointB.position,
                controlPoint1 = pair.controlPoint1.position,
                controlPoint2 = pair.controlPoint2.position
            });
        }
    }

    public void LoadPath(EditableLine editableLine)
    {
        editableLine.ClearPairs();
        foreach (var pairData in pointPairs)
        {
            editableLine.AddPair(pairData.pointA, pairData.pointB, pairData.controlPoint1, pairData.controlPoint2);
        }
    }
}
