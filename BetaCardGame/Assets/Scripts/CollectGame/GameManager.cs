using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject prefab;
    public List<ItemData> allItems;
    public char word;

    private void Awake()
    {
        Instance = this;
    }

    public void CountItem()
    {
        
    }
}
