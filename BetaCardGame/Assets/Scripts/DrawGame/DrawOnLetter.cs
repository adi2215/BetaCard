using UnityEngine;
using System.Collections.Generic;

public class DrawOnLetter : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brushPrefab;
    public SpriteRenderer letterSprite; // Спрайт буквы
    public Transform letterArea; // Родитель для линий

    private List<LineRenderer> lines = new List<LineRenderer>();
    private LineRenderer currentLineRenderer;
    private Vector2 lastPos;
    
    private float totalMaskArea = 0;
    private float paintedArea = 0;

    private void Start()
    {
        CalculateTotalMaskArea();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            PointToMousePos();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush() 
    {
        Vector2 mousePos = GetMouseWorldPosition();
        if (!IsInsideLetter(mousePos)) return; // Ограничение рисования

        GameObject brushInstance = Instantiate(brushPrefab, letterArea);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        lines.Add(currentLineRenderer);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void AddAPoint(Vector2 pointPos) 
    {
        if (currentLineRenderer == null || !IsInsideLetter(pointPos)) return;

        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);

        CalculatePaintedArea();
    }

    void PointToMousePos() 
    {
        Vector2 mousePos = GetMouseWorldPosition();
        if (lastPos != mousePos) 
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    Vector2 GetMouseWorldPosition()
    {
        return m_camera.ScreenToWorldPoint(Input.mousePosition);
    }

    bool IsInsideLetter(Vector2 position)
    {
        if (letterSprite == null) return false;
        return letterSprite.bounds.Contains(new Vector3(position.x, position.y, letterSprite.transform.position.z));
    }

    void CalculateTotalMaskArea()
    {
        if (letterSprite == null) return;
        Bounds bounds = letterSprite.bounds;
        totalMaskArea = bounds.size.x * bounds.size.y;
    }

    void CalculatePaintedArea()
    {
        float sum = 0;
        foreach (var line in lines)
        {
            for (int i = 1; i < line.positionCount; i++)
            {
                Vector2 p1 = line.GetPosition(i - 1);
                Vector2 p2 = line.GetPosition(i);
                sum += Vector2.Distance(p1, p2) * 0.1f; 
            }
        }
        paintedArea = Mathf.Clamp(sum, 0, totalMaskArea);
        float fillPercentage = (paintedArea / totalMaskArea) * 100;
        Debug.Log("Закрашено: " + fillPercentage.ToString("F1") + "%");
    }
}
