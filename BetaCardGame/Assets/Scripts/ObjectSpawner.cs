using UnityEngine;
public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float spawnRate = 2f;
    public float objectSpeed = 5f;
    public float destroyX = -10f; 

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, spawnRate);
    }

    void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);
        spawnedObject.AddComponent<MovingObject>().speed = objectSpeed;
    }
}