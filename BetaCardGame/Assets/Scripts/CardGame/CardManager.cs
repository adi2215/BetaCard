using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public GridLayoutGroup gridLayout;
        public RectTransform parentContainer;
        public Vector2 padding = new Vector2(25, 10);

        void Start() 
        {
            PrepareCards();
            UpdateGrid2();
        }

        void UpdateGrid()
        {
            bool isVertical = Screen.height > Screen.width;
            int columns = Mathf.CeilToInt(Mathf.Sqrt(cardCount));
            int rows = Mathf.CeilToInt((float)cardCount / columns);

            // Корректируем баланс строк и колонок
            if (isVertical && rows <= columns)
            {
                rows++;
                columns = Mathf.CeilToInt((float)cardCount / rows);
            }
            if (!isVertical && columns <= rows)
            {
                columns++;
                rows = Mathf.CeilToInt((float)cardCount / columns);
            }

            // Настройка GridLayoutGroup
            gridLayout.constraint = isVertical ? GridLayoutGroup.Constraint.FixedRowCount 
                                            : GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = isVertical ? rows : columns;

            // Доступное пространство с учетом отступов
            float parentWidth = parentContainer.rect.width - (padding.x * (columns - 1));
            float parentHeight = parentContainer.rect.height - (padding.y * (rows - 1));

            // Минимальный и максимальный размеры карт (автоматический расчет)
            float minCellSize = Mathf.Max(0.1f * Mathf.Min(parentWidth, parentHeight), 50f);
            float maxCellSize = Mathf.Min(parentWidth / 2, parentHeight / 2);

            // Вычисляем размер ячеек
            float cellWidth = parentWidth / columns;
            float cellHeight = parentHeight / rows;
            float finalCellSize = Mathf.Clamp(Mathf.Min(cellWidth, cellHeight), minCellSize, maxCellSize);

            // Применяем параметры
            gridLayout.cellSize = new Vector2(finalCellSize, finalCellSize);
            gridLayout.spacing = padding;
        }

        void UpdateGrid2()
        {
            bool isVertical = Screen.height > Screen.width;
            Debug.Log(isVertical); // Проверяем ориентацию
            int columns = Mathf.CeilToInt(Mathf.Sqrt(cardCount)); // Базовое количество колонок
            int rows = Mathf.CeilToInt((float)cardCount / columns); // Базовое количество строк

            // Корректируем, чтобы Row был чуть больше Column в вертикальном режиме
            if (isVertical && rows <= columns)
            {
                rows++;
                columns = Mathf.CeilToInt((float)cardCount / rows);
            }
            
            // Корректируем, чтобы Column был чуть больше Row в горизонтальном режиме
            if (!isVertical && columns <= rows)
            {
                columns++;
                rows = Mathf.CeilToInt((float)cardCount / columns);
            }

            // Применяем к GridLayoutGroup
            gridLayout.constraint = isVertical ? GridLayoutGroup.Constraint.FixedRowCount
                                            : GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = isVertical ? rows : columns;

            // Настроим размер ячеек
            float parentWidth = parentContainer.rect.width;
            float parentHeight = parentContainer.rect.height;
            float cellWidth = parentWidth / columns;
            float cellHeight = parentHeight / rows;
            float cellSize = Mathf.Min(cellWidth, cellHeight) * 0.45f; // Добавляем небольшой отступ

            gridLayout.cellSize = new Vector2(cellSize, cellSize + 35);
        }

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
            float startX = -Screen.width;
            float startY = GridContainer.position.y; 
            int index = 0;
            LayoutRebuilder.ForceRebuildLayoutImmediate(GridContainer as RectTransform);
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

        /*[SerializeField] private GridLayoutGroup gridLayout;
        private void AdjustGrid() => gridLayout.cellSize = 
            new Vector2(gridLayout.cellSize.x + 5 * cardCount, gridLayout.cellSize.y + 5 * cardCount);*/
    }
}
