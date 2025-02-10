using UnityEngine;

public class EraseOnLetter : MonoBehaviour
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

    private GameObject largerObject; // Большой объект


    private void Start()
    {
        letterSprite.GetComponent<SpriteRenderer>().enabled = false;
        GenerateEraseTexture();
    }

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

        EraseOnTexture(pointPos);
        CheckEraseProgress();
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
    void GenerateEraseTexture()
    {
        Sprite sprite = letterSprite.sprite;
        Texture2D originalTexture = sprite.texture;

        Rect textureRect = sprite.textureRect;
        int texWidth = (int)textureRect.width;
        int texHeight = (int)textureRect.height;

        eraseTexture = new Texture2D(texWidth, texHeight);

        totalLetterPixels = 0;
        erasedPixels = 0;

        for (int x = 0; x < texWidth; x++)
        {
            for (int y = 0; y < texHeight; y++)
            {
                // Берём пиксель из оригинальной текстуры
                Color pixel = originalTexture.GetPixel(x + (int)textureRect.x, y + (int)textureRect.y);

                if (pixel.a < 0.1f) 
                {
                    // Если пиксель полностью прозрачный, сразу стираем его
                    eraseTexture.SetPixel(x, y, new Color(1, 1, 1, 0));
                }
                else
                {
                    // Видимые пиксели остаются
                    eraseTexture.SetPixel(x, y, Color.black);
                    totalLetterPixels++;
                }
            }
        }

        eraseTexture.Apply();
        letterSprite.material.mainTexture = eraseTexture;
    }


    // 🧼 Стираем пиксели, но только в области буквы
    void EraseOnTexture(Vector2 worldPos)
    {
        Vector2 localPos = letterSprite.transform.InverseTransformPoint(worldPos);

        Rect textureRect = letterSprite.sprite.textureRect;
        int texWidth = (int)textureRect.width;
        int texHeight = (int)textureRect.height;

        int px = Mathf.FloorToInt((localPos.x + 0.5f) * texWidth);
        int py = Mathf.FloorToInt((localPos.y + 0.5f) * texHeight);

        if (px < 0 || px >= texWidth || py < 0 || py >= texHeight) return;

        // Стираем область размером brushSize x brushSize
        for (int i = -brushSize / 2; i < brushSize / 2; i++)
        {
            for (int j = -brushSize / 2; j < brushSize / 2; j++)
            {
                int nx = px + i;
                int ny = py + j;

                if (nx >= 0 && nx < texWidth && ny >= 0 && ny < texHeight)
                {
                    Color pixel = eraseTexture.GetPixel(nx, ny);

                    if (pixel.a >= 0.1f)
                    {
                        eraseTexture.SetPixel(nx, ny, new Color(0, 0, 0, 0));
                        erasedPixels++;
                    }
                }
            }
        }

        eraseTexture.Apply();
    }

    // ✅ Проверяем процент стирания буквы
    void CheckEraseProgress()
    {
        float erasePercentage = (erasedPixels / (float)totalLetterPixels) * 100f;
        Debug.Log("Стерто: " + erasePercentage + "%");

        if (erasePercentage >= eraseThreshold)
        {
            Debug.Log("🎉 ПОБЕДА! Буква полностью стерта!");
        }
    }
}
