using UnityEngine;

[CreateAssetMenu(fileName = "NewLetter", menuName = "Letters/LetterData")]
public class LetterData : ScriptableObject
{
    public char letter;
    public Sprite letterSprite;
}
