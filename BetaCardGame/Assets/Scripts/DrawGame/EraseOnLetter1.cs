using UnityEngine;

public class EraseOnLetter1 : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;
    public Material material;
    public SpriteRenderer letterSprite;
    public int brushSize = 5;

    private Texture2D eraseTexture;
    private LineRenderer line;
    private int totalLetterPixels = 0;
    private int erasedPixels = 0;
    public Color[] colors;
    public Texture2D[] textures;
    
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
        currentLineRenderer.material = new Material(currentLineRenderer.sharedMaterial);

        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

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

    public void ChangeColor(int NumberColor)
    {
        line = brush.GetComponent<LineRenderer>();

        if (colors.Length == 0 || line == null) return;
 
        line.sharedMaterial.SetColor("_Color", colors[NumberColor]);
        line.sortingOrder += 1;
    }

    public void ChangeTexture(int NumberTexture)
    {
        line = brush.GetComponent<LineRenderer>();

        if (colors.Length == 0 || line == null) return;
 
        line.sharedMaterial.SetTexture("_MainTex", textures[NumberTexture]);
        line.sortingOrder += 1;
    }
}
