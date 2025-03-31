using UnityEngine;
public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float spawnRate = 2f;
    public float objectSpeed = 5f;
    public float destroyX = -10f;
    public float minHeight;
    public float maxHeight;

    public CollectorManager gameLetters;

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, spawnRate);
    }

    void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);
        spawnedObject.AddComponent<MovingObject>().speed = objectSpeed;

        ItemData letter_Current = gameLetters.letters[Random.Range(0, gameLetters.letters.Length)];

        spawnedObject.GetComponent<MovingObject>().giveLetter(letter_Current);

        spawnedObject.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }

}