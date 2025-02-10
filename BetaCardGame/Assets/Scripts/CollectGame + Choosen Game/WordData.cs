using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Word")]
public class WordData : ScriptableObject
{
    public string WordName;
    public ItemData[] words;
}
