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

        public void Start() 
        {
            PrepareCards();
            //UpdateGrid();
        }

        void UpdateGrid() 
        { 
            bool isVertical = Screen.height > Screen.width; // Проверяем ориентацию
            Debug.Log($"Ориентация: {(isVertical ? "Вертикальная" : "Горизонтальная")}");

            int rows, columns;
    
            
            int bestRows = 1;
            int bestColumns = cardCount; // Начинаем с самой вытянутой формы

            for (int r = 1; r <= cardCount; r++) // Перебираем строки
            {
                int c = Mathf.CeilToInt((float)cardCount / r); // Колонки для текущего r

                if (r * c >= cardCount) // Убеждаемся, что влезает всё
                {
                    // Если разница между rows и columns стала меньше — сохраняем
                    if (Mathf.Abs(r - c) < Mathf.Abs(bestRows - bestColumns))
                    {
                        bestRows = r;
                        bestColumns = c;
                    }
                }
            }

            rows = bestRows;
            columns = bestColumns;

            // Корректируем баланс строк и столбцов
            if (isVertical)
            {
                if (rows <= columns)
                {
                    rows++;
                    columns = Mathf.CeilToInt((float)cardCount / rows);
                }
            }
            else // Горизонтальная ориентация
            {
                if (columns <= rows)
                {
                    columns++;
                    rows = Mathf.CeilToInt((float)cardCount / columns);
                }
            }

            // Если мало карт (1, 2, 3, 4), делаем пропорционально
            if (cardCount <= 4)
            {
                rows = cardCount > 2 ? 2 : 1;
                columns = Mathf.CeilToInt((float)cardCount / rows);
            }

            // Применяем к GridLayoutGroup
            gridLayout.constraint = isVertical ? GridLayoutGroup.Constraint.FixedColumnCount 
                                            : GridLayoutGroup.Constraint.FixedRowCount;
            gridLayout.constraintCount = isVertical ? columns : rows;

            // Доступное пространство с учетом отступов
            float parentWidth = parentContainer.rect.width - (padding.x * (columns - 1));
            float parentHeight = parentContainer.rect.height - (padding.y * (rows - 1));

            // Минимальный и максимальный размер карт
            float minCellSize = Mathf.Max(0.1f * Mathf.Min(parentWidth, parentHeight), 50f);
            float maxCellSize = Mathf.Min(parentWidth * 0.3f, parentHeight * 0.3f); // Ограничение до 30% от контейнера

            // Вычисляем размер ячеек
            float cellWidth = parentWidth / columns;
            float cellHeight = parentHeight / rows;
            float finalCellSize = Mathf.Clamp(Mathf.Min(cellWidth, cellHeight), minCellSize, maxCellSize);

            // Применяем параметры
            gridLayout.cellSize = new Vector2(finalCellSize, finalCellSize + 35);
            gridLayout.spacing = padding;
        } 

        void CalculateGrid(int cardCount, out int rows, out int columns)
        {
            int bestRows = 1;
            int bestColumns = cardCount; // Начинаем с самой вытянутой формы

            for (int r = 1; r <= cardCount; r++) // Перебираем строки
            {
                int c = Mathf.CeilToInt((float)cardCount / r); // Колонки для текущего r

                if (r * c >= cardCount) // Убеждаемся, что влезает всё
                {
                    // Если разница между rows и columns стала меньше — сохраняем
                    if (Mathf.Abs(r - c) < Mathf.Abs(bestRows - bestColumns))
                    {
                        bestRows = r;
                        bestColumns = c;
                    }
                }
            }

            rows = bestRows;
            columns = bestColumns;
        }
        
        void UpdateGrid2() 
        { 
            bool isVertical = Screen.height > Screen.width; 
            Debug.Log(isVertical);
            int columns, rows;

            gridLayout.constraint = isVertical ? GridLayoutGroup.Constraint.FixedColumnCount
                                            : GridLayoutGroup.Constraint.FixedRowCount; 
            if (cardCount >= 4)
            {
                if (!isVertical)
                {
                    rows = Mathf.FloorToInt(cardCount / 2);
                    columns = Mathf.FloorToInt(cardCount * 2 / rows);
                }  

                else
                {
                    columns = Mathf.FloorToInt(cardCount / 2);
                    rows = Mathf.FloorToInt(cardCount * 2 / columns);
                }

                gridLayout.constraintCount = Mathf.FloorToInt(cardCount / 2); 
            }

            else
            {
                if (cardCount == 3)
                {

                    if (!isVertical)
                    {
                        rows = 2;
                        columns = 3;
                    }  

                    else
                    {
                        rows = 3;
                        columns = 2;
                    }

                    gridLayout.constraintCount = 2;
                }

                else if (cardCount == 2)
                {
                    rows = 2;
                    columns = 2;
                    gridLayout.constraintCount = 2;
                }

                else
                {
                    rows = 1;
                    columns = 2;
                    gridLayout.constraintCount = 1;
                }
            }
  
            float parentWidth = parentContainer.rect.width; 
            float parentHeight = parentContainer.rect.height; 
            float cellWidth = parentWidth / columns; 
            float cellHeight = parentHeight / rows; 
            float cellSize = Mathf.Min(cellWidth, cellHeight) * 0.6f;

            if (isVertical)
                cellSize = Mathf.Min(cellWidth, cellHeight) * 0.8f;
 
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
            /*float startX = -Screen.width;
            float startY = GridContainer.position.y; 
            int index = 0;*/
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
