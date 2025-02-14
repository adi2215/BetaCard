using UnityEngine;
using System.Collections.Generic;

public class TrailBall : MonoBehaviour
{
    public GameObject trailPrefab; // Префаб следа
    private List<GameObject> trails = new List<GameObject>(); // Хранит все следы
    private LineRenderer currentTrail;
    private List<Vector3> points = new List<Vector3>();

    public void StartNewTrail()
    {
        GameObject newTrail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        currentTrail = newTrail.GetComponent<LineRenderer>();

        trails.Add(newTrail); 
        currentTrail.positionCount = 0;
        points.Clear(); 
    }

    public void AddPoint(Vector3 position)
    {
        if (currentTrail == null) return;

        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], position) > 0.05f)
        {
            points.Add(position);
            currentTrail.positionCount = points.Count;
            currentTrail.SetPositions(points.ToArray());
        }
    }

    public void ClearTrails()
    {
        foreach (GameObject trail in trails)
        {
            Destroy(trail);
        }
        trails.Clear();
    }

    public void RemoveLastPoint()
    {
        if (currentTrail.positionCount > 1)
        {
            int newCount = currentTrail.positionCount - 1;
            Vector3[] newPositions = new Vector3[newCount];
            
            for (int i = 0; i < newCount; i++)
            {
                newPositions[i] = currentTrail.GetPosition(i);
            }

            currentTrail.positionCount = newCount;
            currentTrail.SetPositions(newPositions);
        }
    }

}
