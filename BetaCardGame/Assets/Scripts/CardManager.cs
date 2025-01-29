using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardSystem
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private Card_Display cardPrefab;
        [SerializeField] private Transform GridContainer;
        [SerializeField] private List<CardType> availableCards; 

        public int cardCount;
        [SerializeField] private TextMeshProUGUI textCountCard;

        private List<_Card> gameCards;

        private Card_Display firstCard = null;
        private Card_Display secondCard = null;
        private bool canFlip = true;

        void Start() => PrepareCards();

        private void PrepareCards()
        {
            gameCards = new List<_Card>();

            HashSet<int> cardIndices = new HashSet<int>();

            System.Random randomCard = new System.Random();

            textCountCard.text = $"COUNT: {cardCount}";

            while (cardIndices.Count < cardCount)
            {
                int randomIndex = randomCard.Next(availableCards.Count);

                if (!cardIndices.Contains(randomIndex))
                {
                    cardIndices.Add(randomIndex);
                    CardType template = availableCards[randomIndex];

                    gameCards.Add(new _Card(template)); 
                    gameCards.Add(new _Card(template)); 
                }
            }

            gameCards = Shuffle(gameCards);

            CreateCards();
        }

        private void CreateCards()
        {
            foreach (var card in gameCards)
            {
                Card_Display cardObj = Instantiate(cardPrefab, GridContainer);
                cardObj.Setup(card, this);
            }
        }

        internal bool CanFlipCard() { return canFlip && (firstCard == null || secondCard == null); }

        internal void OnCardRevealed(Card_Display card)
        {
            if (firstCard == null)
            {
                firstCard = card;
                return;
            }

            if (secondCard == null)
            {
                secondCard = card;
                StartCoroutine(CheckForMatch());
            }
        }

        private IEnumerator CheckForMatch()
        {
            canFlip = false;

            yield return new WaitForSeconds(0.8f);

            if (firstCard.cardData.card.cardName == secondCard.cardData.card.cardName)
            {
                firstCard.cardData.isMatched = true;
                secondCard.cardData.isMatched = true;

                StartCoroutine(firstCard.PlayMatchAnimation());
                StartCoroutine(secondCard.PlayMatchAnimation());

                CheckForWin();
            }

            else
            {
                firstCard.Hide();
                secondCard.Hide();
            }

            firstCard = null;
            secondCard = null;
            canFlip = true;
        }

        //Fisher–Yates shuffle
        private List<_Card> Shuffle(List<_Card> list)
        {
            System.Random randCard = new System.Random();
            int n = list.Count;
            
            while (n > 1)
            {
                n--;
                int k = randCard.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }

        void CheckForWin()
        {
            bool allMatched = gameCards.TrueForAll(card => card.isMatched);
            
            if (allMatched)
            {
                Debug.Log("Все пары найдены!");
            }
        }
    }
}
