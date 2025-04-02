using UnityEngine;

[CreateAssetMenu(fileName = "NewLetter", menuName = "Letters/LetterData")]
public class LetterData : ScriptableObject
{
    public string letter;
    public Sprite letterSprite;
    public AudioClip sound;
}
