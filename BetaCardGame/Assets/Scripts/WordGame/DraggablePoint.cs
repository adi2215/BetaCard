using UnityEngine;

public class DraggablePoint : MonoBehaviour
{
    private Vector3 offset; // Смещение для точности перетаскивания
    private bool isDragging = false; // Флаг, чтобы отслеживать, перетаскивается ли точка

    private void Start()
    {
        // Убедимся, что на объекте есть коллайдер
        if (GetComponent<Collider2D>() == null)
        {
            // Добавляем CircleCollider2D, если его нет
            gameObject.AddComponent<CircleCollider2D>();
        }
    }
    
    private void OnMouseDown()
    {
        // Проверяем, что точка действительно была нажата (перед перемещением)
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    private void OnMouseUp()
    {
        // Когда пользователь отпускает точку, прекращаем перетаскивание
        isDragging = false;
    }

    private void Update()
    {
        // Если точка перетаскивается, обновляем её позицию
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    // Конвертируем координаты мыши в мировые координаты
    private Vector3 GetMouseWorldPos()
    {
        // Получаем координаты мыши в экране
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.nearClipPlane; // Чтобы получить правильную позицию в 3D пространстве
        return Camera.main.ScreenToWorldPoint(mouseScreenPos); // Конвертируем в мировые координаты
    }
}
