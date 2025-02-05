using UnityEngine;

public class BallWithTrail : MonoBehaviour
{
    private TrailRenderer trailRenderer;  // Компонент TrailRenderer

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.time = 1f;  // Время жизни следа
    }

    // Можно включать/выключать след, если нужно
    public void EnableTrail(bool enable)
    {
        trailRenderer.enabled = enable;
    }
}
