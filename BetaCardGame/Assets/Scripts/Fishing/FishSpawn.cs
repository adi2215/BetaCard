using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FishSpawn : MonoBehaviour
{
    public GameObject prefab;

    public float spawnRate = 1f;
        
    public float spawnTime = 2f;

    public float minHeight = -1f;

    public float maxHeight = 2f;

    public TakeFish gameLetters;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnTime, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        GameObject fishes = Instantiate(prefab, transform.position, Quaternion.identity);

        fishes.transform.localScale = new Vector3(
                fishes.transform.localScale.x * transform.localScale.x,
                fishes.transform.localScale.y * transform.localScale.y,
                fishes.transform.localScale.z * transform.localScale.z
            );

        ItemData letter_Current = gameLetters.letters[Random.Range(0, gameLetters.letters.Length)];

        fishes.GetComponent<Fish>().giveLetter(letter_Current.itemName);

        fishes.GetComponent<Fish>().giveImageLetter(letter_Current.itemSprite);

        if (transform.localScale.x == 1)
        {
            fishes.GetComponent<Fish>().fishDirection(Vector3.left);
            fishes.GetComponent<SortingGroup>().sortingOrder = 5;
        }

        else if (transform.localScale.x == -1)
        {
            fishes.GetComponent<Fish>().fishDirection(Vector3.right);
            RectTransform transformCanva = fishes.GetComponentInChildren<RectTransform>();
            transformCanva.localScale = new Vector3(
                transformCanva.localScale.x * transform.localScale.x,
                transformCanva.localScale.y * transform.localScale.y,
                transformCanva.localScale.z * transform.localScale.z
            );
        }

        fishes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }
}
