using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public EditableLine line; // Ссылка на нашу EditableLine

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ЛКМ или тап по экрану
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Убираем глубину в 2D

            line.AddPoint(mousePos); // Добавляем точку в линию
        }
    }
}
