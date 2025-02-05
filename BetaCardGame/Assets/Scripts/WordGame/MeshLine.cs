using UnityEngine;
using System.Collections.Generic;

public class MeshLine : MonoBehaviour
{
    public Material material;
    public float lineWidth = 0.3f;
    
    private List<Vector3> points = new List<Vector3>();
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    public void AddPoint(Vector3 point)
    {
        points.Add(point);
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        if (points.Count < 2) return;

        mesh = new Mesh();
        Vector3[] vertices = new Vector3[points.Count * 2];
        int[] triangles = new int[(points.Count - 1) * 6];

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 dir = i == points.Count - 1 ? (points[i] - points[i - 1]).normalized : (points[i + 1] - points[i]).normalized;
            Vector3 normal = new Vector3(-dir.y, dir.x, 0) * lineWidth / 2;

            vertices[i * 2] = points[i] + normal;
            vertices[i * 2 + 1] = points[i] - normal;

            if (i < points.Count - 1)
            {
                int start = i * 6;
                triangles[start] = i * 2;
                triangles[start + 1] = i * 2 + 1;
                triangles[start + 2] = i * 2 + 2;
                triangles[start + 3] = i * 2 + 2;
                triangles[start + 4] = i * 2 + 1;
                triangles[start + 5] = i * 2 + 3;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
