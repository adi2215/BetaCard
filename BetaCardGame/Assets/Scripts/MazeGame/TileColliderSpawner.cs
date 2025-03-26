using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileColliderSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject tileColliderPrefab; // Префаб с BoxCollider2D

    private Dictionary<Vector3Int, GameObject> tileColliders = new Dictionary<Vector3Int, GameObject>();

    void Start()
    {
        SpawnColliders();
    }

    void SpawnColliders()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                Vector3 worldPos = tilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);

                GameObject colliderObj = Instantiate(tileColliderPrefab, worldPos, Quaternion.identity);
                colliderObj.transform.parent = transform;
                
                TileCollider tileCollider = colliderObj.GetComponent<TileCollider>();
                tileCollider.SetTilePosition(pos);

                tileColliders[pos] = colliderObj;
            }
        }
    }
}
