using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{
    public EditableLine editableLine;  // Ссылка на EditableLine, которая содержит кривую
    public float speed = 5f;           // Скорость шарика
    public TrailRenderer trailRenderer;  // Ссылка на компонент TrailRenderer

    private float t = 0f;              // Прогресс по кривой
    private bool isMoving = false;     // Флаг движения шарика

    private void Start()
    {
        // Сначала отключаем TrailRenderer
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            // Пройдем по кривой с использованием интерполяции (Lerp или Bezier)
            t += Time.deltaTime * speed;
            if (t >= 1f)
            {
                t = 1f; // Шарик достиг конечной точки
                isMoving = false;
                if (trailRenderer != null)
                {
                    trailRenderer.enabled = false; // Отключаем TrailRenderer, когда шарик достиг цели
                }
            }

            // Получаем точку на кривой (через интерполяцию)
            Vector3 position = GetPointOnCurve(t);
            transform.position = position;

            // Поворот шарика в направлении движения
            if (editableLine.points.Count > 1)
            {
                Vector3 direction = GetDirection(t);
                transform.up = direction;  // Устанавливаем направление вектора движения
            }
        }
    }

    // Получаем точку на кривой на основе прогресса t
    private Vector3 GetPointOnCurve(float t)
    {
        List<Vector3> curvePoints = editableLine.GenerateCurve();
        int pointCount = curvePoints.Count;

        // Проверка на достаточное количество точек для интерполяции
        if (pointCount < 2)
        {
            Debug.LogError("Недостаточно точек для интерполяции. Количество точек: " + pointCount);
            return transform.position;  // Возвращаем начальную позицию, если точек меньше 2
        }

        // Нормализуем значение t в диапазоне от 0 до 1
        t = Mathf.Clamp01(t);

        // Если t == 1, возвращаем последнюю точку без интерполяции
        if (t == 1f)
        {
            return curvePoints[pointCount - 1]; // Возвращаем последнюю точку
        }

        // Количество сегментов между точками (pointCount - 1)
        int segmentCount = pointCount - 1;
        float segmentProgress = t * segmentCount;
        int segmentIndex = Mathf.FloorToInt(segmentProgress);  // Индекс сегмента
        float segmentT = segmentProgress - segmentIndex;       // Позиция внутри сегмента

        // Проверка индекса, чтобы он не выходил за пределы списка
        if (segmentIndex < 0 || segmentIndex + 1 >= pointCount)
        {
            Debug.LogError($"Индекс выходит за пределы списка. t: {t}, segmentIndex: {segmentIndex}");
            return transform.position;
        }

        // Интерполяция между точками
        return Vector3.Lerp(curvePoints[segmentIndex], curvePoints[segmentIndex + 1], segmentT);
    }

    // Получаем направление для шарика (вектор от точки к следующей)
    private Vector3 GetDirection(float t)
    {
        List<Vector3> curvePoints = editableLine.GenerateCurve();
        int pointCount = curvePoints.Count;

        if (pointCount < 2)
            return Vector3.zero;

        int segmentCount = pointCount - 1;
        float segmentProgress = t * segmentCount;
        int segmentIndex = Mathf.FloorToInt(segmentProgress);
        float segmentT = segmentProgress - segmentIndex;

        // Вектор от текущей точки к следующей
        Vector3 direction = (curvePoints[segmentIndex + 1] - curvePoints[segmentIndex]).normalized;
        return direction;
    }

    // Запускаем движение шарика по кривой
    public void StartMoving()
    {
        isMoving = true;
        t = 0f;
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true; // Включаем TrailRenderer при старте
        }
    }
}
