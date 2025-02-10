using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{
    public EditableLine editableLine; // Ссылка на EditableLine, которая содержит кривую
    public float speed = 5f; // Скорость шарика
    public Transform arrow; // Стрелка направления

    private float t = 0f; // Прогресс по кривой (0 - начало, 1 - конец)
    private bool isMoving = false;
    private bool isDragging = false;
    
    private void Update()
    {
        if (isMoving && !isDragging)
        {
            MoveAlongCurve();
        }

        if (isDragging)
        {
            DragAlongCurve();
        }
    }

    /// <summary>
    /// Двигаем шарик по линии
    /// </summary>
    private void MoveAlongCurve()
    {
        t += Time.deltaTime * speed;
        if (t >= 1f)
        {
            t = 1f;
            isMoving = false;
        }

        Vector3 position = GetPointOnCurve(t);
        transform.position = position;

        UpdateArrowDirection();
    }

    /// <summary>
    /// Перетаскиваем шар по кривой
    /// </summary>
    private void DragAlongCurve()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Находим ближайшую точку на кривой
        t = GetClosestT(mousePos);
        transform.position = GetPointOnCurve(t);

        UpdateArrowDirection();
    }

    /// <summary>
    /// Получаем ближайший t для точки
    /// </summary>
    private float GetClosestT(Vector3 position)
    {
        List<Vector3> curvePoints = editableLine.GenerateCurve();
        float closestT = 0f;
        float minDistance = float.MaxValue;

        for (int i = 0; i < curvePoints.Count; i++)
        {
            float t = i / (float)(curvePoints.Count - 1);
            float distance = Vector3.Distance(position, curvePoints[i]);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestT = t;
            }
        }

        return closestT;
    }

    /// <summary>
    /// Получаем точку на кривой на основе t
    /// </summary>
    private Vector3 GetPointOnCurve(float t)
    {
        List<Vector3> curvePoints = editableLine.GenerateCurve();
        int pointCount = curvePoints.Count;

        if (pointCount < 2) return transform.position;

        t = Mathf.Clamp01(t);

        if (t == 1f) return curvePoints[pointCount - 1];

        int segmentCount = pointCount - 1;
        float segmentProgress = t * segmentCount;
        int segmentIndex = Mathf.FloorToInt(segmentProgress);
        float segmentT = segmentProgress - segmentIndex;

        return Vector3.Lerp(curvePoints[segmentIndex], curvePoints[segmentIndex + 1], segmentT);
    }

    /// <summary>
    /// Обновляем направление стрелки
    /// </summary>
    private void UpdateArrowDirection()
    {
        if (arrow == null) return;

        Vector3 direction = GetDirection(t);
        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, angle - 45);
        }
    }

    /// <summary>
    /// Получаем направление движения
    /// </summary>
    private Vector3 GetDirection(float t)
    {
        List<Vector3> curvePoints = editableLine.GenerateCurve();
        int pointCount = curvePoints.Count;

        if (pointCount < 2) return Vector3.zero;

        int segmentCount = pointCount - 1;
        float segmentProgress = t * segmentCount;
        int segmentIndex = Mathf.FloorToInt(segmentProgress);

        if (segmentIndex >= pointCount - 1) return Vector3.zero;

        return (curvePoints[segmentIndex + 1] - curvePoints[segmentIndex]).normalized;
    }

    /// <summary>
    /// Запускаем движение
    /// </summary>
    public void StartMoving()
    {
        isMoving = true;
        isDragging = false;
        t = 0f;
    }

    /// <summary>
    /// Перетаскивание: Начало
    /// </summary>
    private void OnMouseDown()
    {
        isDragging = true;
        isMoving = false;
    }

    /// <summary>
    /// Перетаскивание: Конец
    /// </summary>
    private void OnMouseUp()
    {
        isDragging = false;
    }
}
