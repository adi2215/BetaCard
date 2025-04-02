using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveAndFadeImage : MonoBehaviour
{
    public Image targetImage;  // Аймедж, который будет двигаться
    public Button triggerButton; // Баттон, который запускает анимацию
    private Vector3 originalPosition; // Запомним исходную позицию

    void Start()
    {
        if (targetImage == null || triggerButton == null)
        {
            Debug.LogError("Назначь Image и Button в инспекторе!");
            return;
        }

        originalPosition = targetImage.rectTransform.position;
        triggerButton.onClick.AddListener(AnimateImage);
    }

    void AnimateImage()
    {
        Vector3 centerScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Двигаем к центру за 0.5 секунды
        targetImage.rectTransform.DOMove(centerScreen, 0.5f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // После движения делаем плавное исчезновение
                targetImage.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    // Можно сбросить в исходное положение и вернуть прозрачность
                    targetImage.rectTransform.position = originalPosition;
                    targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 1f);
                });
            });
    }
}
