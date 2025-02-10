using UnityEngine;

public class EraseOnLetter1 : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;
    public SpriteRenderer letterSprite; // Спрайт буквы
    public int brushSize = 5; // Размер кисти
    public float eraseThreshold = 90f; // Процент стирания для победы

    private Texture2D eraseTexture;
    private int totalLetterPixels = 0;
    private int erasedPixels = 0;
    
    LineRenderer currentLineRenderer;
    Vector2 lastPos;

    private void Update()
    {
        Erasing();
    }

    void Erasing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            PointToMousePos();
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush() 
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

        // Настройка ширины TrailRenderer
        if (currentLineRenderer  != null)
        {
            currentLineRenderer.positionCount = 1;
            currentLineRenderer.SetPosition(0, m_camera.ScreenToWorldPoint(Input.mousePosition));

            float worldBrushSize = (brushSize / letterSprite.sprite.pixelsPerUnit) * letterSprite.transform.lossyScale.x;
            currentLineRenderer.startWidth = worldBrushSize;
            currentLineRenderer.endWidth = worldBrushSize;
        }
    }

    void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        if (lastPos != mousePos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    // 🎨 Создаём слой краски (только в области буквы)


    // 🧼 Стираем пиксели, но только в области буквы


    // ✅ Проверяем процент стирания буквы
}
