using System.Collections.Generic;
using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public GameObject lightPrefab; // Префаб света
    public int lightRadius = 1; // Радиус освещения
    private Dictionary<Vector3Int, GameObject> lightSources = new Dictionary<Vector3Int, GameObject>();

    public void InitializeLight(Vector3Int startPos)
    {
        // Осветить стартовую клетку
        if (!lightSources.ContainsKey(startPos))
        {
            GameObject newLight = Instantiate(lightPrefab, startPos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            lightSources[startPos] = newLight;
        }

        UpdateLight(startPos); // Осветить радиус вокруг стартовой точки
    }

    public void UpdateLight(Vector3Int playerPos)
    {
        // Создаём свет вокруг игрока
        for (int x = -lightRadius; x <= lightRadius; x++)
        {
            for (int y = -lightRadius; y <= lightRadius; y++)
            {
                Vector3Int tilePos = playerPos + new Vector3Int(x, y, 0);

                if (!lightSources.ContainsKey(tilePos))
                {
                    GameObject newLight = Instantiate(lightPrefab, tilePos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    lightSources[tilePos] = newLight;
                }
            }
        }
    }

    public bool IsTileLit(Vector3Int tilePos)
    {
        return lightSources.ContainsKey(tilePos);
    }
}
