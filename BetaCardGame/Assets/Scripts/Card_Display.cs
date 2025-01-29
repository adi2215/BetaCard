using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardSystem
{
    public class Card_Display : MonoBehaviour
    {
        [SerializeField] internal Image cardImage;
        [SerializeField] private Sprite hiddenCard;

        internal _Card cardData;
        private CardManager cardManager;

        private bool isSelected = false;

        public void Setup(_Card currentCard, CardManager controller) 
        { 
            cardData = currentCard;
            cardManager = controller;
            Hide();
        }

        public void Show()
        {
            transform.DORotate(new Vector3(0, 270, 0), 0.1f)
                .OnComplete(() => 
                {
                    cardImage.sprite = cardData.card.sprite;
                    transform.DORotate(new Vector3(0, 360, 0), 0.1f);
                });

            isSelected = true;
        }

        public void Hide()
        {
            transform.DORotate(new Vector3(0, 90, 0), 0.1f)
                .OnComplete(() => 
                {
                    cardImage.sprite = hiddenCard;
                    transform.DORotate(new Vector3(0, 0, 0), 0.1f);
                });
                
            isSelected = false;
        }
        
        public void OnCardClick()
        {
            if (!isSelected && !cardData.isMatched && cardManager.CanFlipCard()) 
            {
                Show();
                cardManager.OnCardRevealed(this);
            }
        }

        public IEnumerator PlayMatchAnimation()
        {
            float duration = 0.4f;

            Tween scaleTween = transform.DOScale(Vector3.zero, duration).SetEase(Ease.InFlash);

            yield return scaleTween.WaitForCompletion();
        }
    }
}
