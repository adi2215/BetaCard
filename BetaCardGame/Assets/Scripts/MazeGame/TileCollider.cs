using UnityEngine;

public class TileCollider : MonoBehaviour
{
    private Vector3Int tilePosition;
    private SpriteRenderer spriteRenderer;

    public void SetTilePosition(Vector3Int pos)
    {
        tilePosition = pos;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            SetInvisible(); 
        }
    }

    private void OnMouseEnter()
    {
        SetTransparent();
    }

    private void OnMouseExit()
    {
        SetInvisible(); 
    }

    private void OnMouseDown()
    {
        if (FindObjectOfType<PathFinderAgent>() is PathFinderAgent player)
        {
            player.MoveTo(tilePosition);
            SetVisible(); 
        }
    }

    void SetInvisible()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f); 
        }
    }

    void SetTransparent()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    void SetVisible()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); 
        }
    }
}
