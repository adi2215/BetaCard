using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveImageWithPause : MonoBehaviour
{
    public Image imageToMove;           // Привязанный UI Image
    public float moveDuration = 3f;       // Общее время перемещения (без учета паузы)
    public float delay = 0f;              // Задержка перед началом анимации
    public float pauseDuration = 2f;      // Время, которое Image останавливается в середине

    private RectTransform imageRect;
    private RectTransform canvasRect;

    void Start()
    {
        imageRect = imageToMove.rectTransform;
        canvasRect = imageToMove.canvas.GetComponent<RectTransform>();

        // Получаем ширину канвы и изображения
        float canvasWidth = canvasRect.rect.width;
        float imageWidth = imageRect.rect.width;

        // Вычисляем стартовую позицию: за правым краем канвы (с учетом ширины изображения)
        Vector2 startPos = new Vector2(canvasWidth / 2 + imageWidth, imageRect.anchoredPosition.y);
        // Вычисляем конечную позицию: за левым краем канвы
        Vector2 endPos = new Vector2(-canvasWidth / 2 - imageWidth, imageRect.anchoredPosition.y);
        // Середина канвы (по X) – здесь изображение будет остановлено
        Vector2 midPos = new Vector2(0, imageRect.anchoredPosition.y);

        // Устанавливаем начальную позицию
        imageRect.anchoredPosition = startPos;

        // Разбиваем время перемещения на две части (до середины и после)
        float firstDuration = moveDuration / 2f;
        float secondDuration = moveDuration / 2f;

        // Создаем последовательность DOTween
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delay); // Начальная задержка, если нужна
        seq.Append(imageRect.DOAnchorPos(midPos, firstDuration).SetEase(Ease.Linear));
        seq.AppendInterval(pauseDuration); // Пауза в середине
        seq.Append(imageRect.DOAnchorPos(endPos, secondDuration).SetEase(Ease.Linear));
    }
}
