using UnityEngine;

namespace CardSystem 
{  
    [CreateAssetMenu(fileName = "New Card", menuName = "Card System")]
    public class CardType : ScriptableObject
    {
        public string cardName;
        public Sprite sprite;
        public AudioClip audio;
    }
}
