using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public AudioClip sound;
}
